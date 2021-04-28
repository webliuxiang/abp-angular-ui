using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.Chat
{
    [Table(AppConsts.TablePrefix + "ChatMessages")]
    public class ChatMessage : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        public const int MaxMessageLength = 4 * 1024; //4KB

        /// <summary>
        /// 发送人Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 发送人租户Id
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 接收人用户Id
        /// </summary>
        public long TargetUserId { get; set; }
        /// <summary>
        /// 接收人租户Id
        /// </summary>
        public int? TargetTenantId { get; set; }
        /// <summary>
        /// 发送的内容
        /// </summary>
        [Required]
        [StringLength(MaxMessageLength)]
        public string Message { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 收/发
        /// </summary>
        public ChatSide Side { get; set; }

        /// <summary>
        /// 已读/未读
        /// </summary>
        public ChatMessageReadState ReadState { get; private set; }

        /// <summary>
        /// 接收器读取状态  已读/未读
        /// </summary>
        public ChatMessageReadState ReceiverReadState { get; private set; }
        /// <summary>
        /// 共享消息Id
        /// </summary>
        public Guid? SharedMessageId { get; set; }

        public ChatMessage(
            UserIdentifier user,
            UserIdentifier targetUser,
            ChatSide side,
            string message,
            ChatMessageReadState readState,
            Guid sharedMessageId,
            ChatMessageReadState receiverReadState)
        {
            UserId = user.UserId;
            TenantId = user.TenantId;
            TargetUserId = targetUser.UserId;
            TargetTenantId = targetUser.TenantId;
            Message = message;
            Side = side;
            ReadState = readState;
            SharedMessageId = sharedMessageId;
            ReceiverReadState = receiverReadState;

            CreationTime = Clock.Now;
        }

        public void ChangeReadState(ChatMessageReadState newState)
        {
            ReadState = newState;
        }

        protected ChatMessage()
        {

        }

        public void ChangeReceiverReadState(ChatMessageReadState newState)
        {
            ReceiverReadState = newState;
        }
    }
}
