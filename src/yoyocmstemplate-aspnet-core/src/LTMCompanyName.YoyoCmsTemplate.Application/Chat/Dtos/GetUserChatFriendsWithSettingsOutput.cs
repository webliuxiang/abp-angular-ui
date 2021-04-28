using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using L._52ABP.Common.Extensions.Enums.Dtos;
using LTMCompanyName.YoyoCmsTemplate.Friendships.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Chat.Dtos
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }

        public List<FriendDto> Friends { get; set; }

        public GetUserChatFriendsWithSettingsOutput()
        {
            Friends = new EditableList<FriendDto>();
        }

        public EnumOutputDto FriendState { get; set; }


    }
}
