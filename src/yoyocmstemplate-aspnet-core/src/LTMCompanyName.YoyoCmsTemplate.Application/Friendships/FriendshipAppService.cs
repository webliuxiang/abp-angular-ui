using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.UI;
using LTMCompanyName.YoyoCmsTemplate.Chat;
using LTMCompanyName.YoyoCmsTemplate.Friendships.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Modules.Chat;
using LTMCompanyName.YoyoCmsTemplate.UserManagerment.Users;

namespace LTMCompanyName.YoyoCmsTemplate.Friendships
{
    [AbpAuthorize]
    public class FriendshipAppService : YoyoCmsTemplateAppServiceBase, IFriendshipAppService
    {
        private readonly IFriendshipManager _friendshipManager;
        private readonly IOnlineClientManager<ChatChannel> _onlineClientManager;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly ITenantCache _tenantCache;
        private readonly IChatFeatureChecker _chatFeatureChecker;

        public FriendshipAppService(
            IFriendshipManager friendshipManager,
            IOnlineClientManager<ChatChannel> onlineClientManager,
            IChatCommunicator chatCommunicator,
            ITenantCache tenantCache,
            IChatFeatureChecker chatFeatureChecker)
        {
            _friendshipManager = friendshipManager;
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
            _tenantCache = tenantCache;
            _chatFeatureChecker = chatFeatureChecker;
        }
        /// <summary>
        /// 添加好友关系
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input)
        {
            // 获取当前登录人
            var userIdentifier = AbpSession.ToUserIdentifier();
            //  组合获取要添加的好友信息
            var probableFriend = new UserIdentifier(input.TenantId, input.UserId);
            // 判断跨租户(TODO 目前猜测)
            _chatFeatureChecker.CheckChatFeatures(userIdentifier.TenantId, probableFriend.TenantId);
            // 判断双方是否是联系人
            if (await _friendshipManager.GetFriendshipOrNullAsync(userIdentifier, probableFriend) != null)
            {
                throw new UserFriendlyException(L("YouAlreadySentAFriendshipRequestToThisUser"));
            }
            // 获取到当前登录人信息
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            // 将要添加的好友信息
            User probableFriendUser;
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                probableFriendUser = await UserManager.FindByIdAsync(input.UserId.ToString());
            }
            // 好友的租户名称
            var friendTenancyName = probableFriend.TenantId.HasValue ? _tenantCache.Get(probableFriend.TenantId.Value).TenancyName : null;
            // 生成一条 登录人 添加 好友的记录
            var sourceFriendship = new Friendship(userIdentifier, probableFriend, friendTenancyName, probableFriendUser.UserName, probableFriendUser.ProfilePictureId, FriendshipState.Accepted);
            await _friendshipManager.CreateFriendshipAsync(sourceFriendship);

            // 获取登录人租户名称
            var userTenancyName = user.TenantId.HasValue ? _tenantCache.Get(user.TenantId.Value).TenancyName : null;
            // 生成一条 好友 和 当前登录人的好友记录
            var targetFriendship = new Friendship(probableFriend, userIdentifier, userTenancyName, user.UserName, user.ProfilePictureId, FriendshipState.Accepted);
            await _friendshipManager.CreateFriendshipAsync(targetFriendship);

            // 判断接收人是否在线
            var clients = _onlineClientManager.GetAllByUserId(probableFriend);
            if (clients.Any())
            {
                var isFriendOnline = _onlineClientManager.IsOnline(sourceFriendship.ToUserIdentifier());
                await _chatCommunicator.SendFriendshipRequestToClient(clients, targetFriendship, false, isFriendOnline);
            }
            // 判断发送人是否在线
            var senderClients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (senderClients.Any())
            {
                var isFriendOnline = _onlineClientManager.IsOnline(targetFriendship.ToUserIdentifier());
                await _chatCommunicator.SendFriendshipRequestToClient(senderClients, sourceFriendship, true, isFriendOnline);
            }

            var sourceFriendshipRequest = ObjectMapper.Map<FriendDto>(sourceFriendship);
            sourceFriendshipRequest.IsOnline = _onlineClientManager.GetAllByUserId(probableFriend).Any();

            return sourceFriendshipRequest;
        }
        /// <summary>
        /// 根据用户名查找用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input)
        {
            var probableFriend = await GetUserIdentifier(input.TenancyName, input.UserName);
            return await CreateFriendshipRequest(new CreateFriendshipRequestInput
            {
                TenantId = probableFriend.TenantId,
                UserId = probableFriend.UserId
            });
        }
        /// <summary>
        /// 屏蔽用户消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BlockUser(BlockUserInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            await _friendshipManager.BanFriendAsync(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                await _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Blocked);
            }
        }
        /// <summary>
        /// 接收用户消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UnblockUser(UnblockUserInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            await _friendshipManager.AcceptFriendshipRequestAsync(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                await _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Accepted);
            }
        }

        public async Task AcceptFriendshipRequest(AcceptFriendshipRequestInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            await _friendshipManager.AcceptFriendshipRequestAsync(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                await _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Blocked);
            }
        }

        private async Task<UserIdentifier> GetUserIdentifier(string tenancyName, string userName)
        {
            int? tenantId = null;
            if (!tenancyName.Equals("."))
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
                    if (tenant == null)
                    {
                        throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                    }

                    tenantId = tenant.Id;
                }
            }

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var user = await UserManager.FindByNameOrEmailAsync(userName);
                if (user == null)
                {
                    throw new UserFriendlyException(L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                }

                return user.ToUserIdentifier();
            }
        }
    }
}
