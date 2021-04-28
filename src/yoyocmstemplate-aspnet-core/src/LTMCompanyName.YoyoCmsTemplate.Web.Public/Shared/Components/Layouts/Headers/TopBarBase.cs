using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization;
using LTMCompanyName.YoyoCmsTemplate.Web.Public.Models;
using Microsoft.AspNetCore.Components;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components.Layouts.Headers
{
    public class TopBarBase : AbpComponentBase
    {
        [Inject] 
        private   ILanguageManager LanguageManager { get; set; }
        [Parameter]
        public TopBarViewModel Model { get; set; } = new TopBarViewModel();

       
         

        protected override  void OnInitialized()
        {
          

            Model.Languages = LanguageManager.GetLanguages().Where(l => !l.IsDisabled).ToList();
            Model.CurrentLanguage = LanguageManager.CurrentLanguage;

             Model.Logined = AbpSession.UserId.HasValue;






        }


    }
}
