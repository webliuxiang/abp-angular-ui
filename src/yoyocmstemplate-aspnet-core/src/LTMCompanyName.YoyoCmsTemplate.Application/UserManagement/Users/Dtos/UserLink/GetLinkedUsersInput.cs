﻿using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace LTMCompanyName.YoyoCmsTemplate.UserManagement.Users.Dtos.UserLink
{
    public class GetLinkedUsersInput : IPagedResultRequest, ISortedResultRequest, IShouldNormalize
    {
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Username";
            }
        }
    }
}