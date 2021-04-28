using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds;
using LTMCompanyName.YoyoCmsTemplate.Modules.WebSiteSetting.BannerAds.Dtos;
using Microsoft.AspNetCore.Components;

namespace LTMCompanyName.YoyoCmsTemplate.Web.Public.Shared.Components
{
    public class BannerBase:AbpComponentBase
    {

        public List<BannerAdListDto> Model { get; set; } = new List<BannerAdListDto>();


        public int adsCounts{ get; set; }

        [Inject]
        private IBannerImgAppService _bannerImgAppService { get; set; }



        protected override async Task OnInitializedAsync()
        {
            Model  = await _bannerImgAppService.GetForReadBannerAds().ConfigureAwait(false);

            adsCounts = Model.Count;
        }



    }
}
