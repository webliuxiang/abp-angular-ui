using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LTMCompanyName.YoyoCmsTemplate.Modules.DynamicView.DomainService
{
    public class DynamicViewManager : IDynamicViewManager
    {
        readonly IHostEnvironment _hostEnv;

        readonly ILogger<DynamicViewManager> _logger;

        public DynamicViewManager(IHostEnvironment hostEnv, ILogger<DynamicViewManager> logger)
        {
            _hostEnv = hostEnv;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<DynamicPage> GetDynamicPageInfo(string name)
        {
            try
            {
                var pageFilters = await this.GetPageFiltersFromFile(name);
                var columns = await this.GetColumnsFromFile(name);

                return new DynamicPage()
                {
                    PageFilters = pageFilters,
                    Columns = columns
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PageFilterItem>> GetPageFilters(string name)
        {
            return await this.GetPageFiltersFromFile(name);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ColumnItem>> GetColumns(string name)
        {
            return await this.GetColumnsFromFile(name);
        }

        protected async Task<List<PageFilterItem>> GetPageFiltersFromFile(string name)
        {
            var configFilePath = Path.Join(
                _hostEnv.ContentRootPath,
                    "wwwroot",
                    "configs",
                    "page-filter",
                    $"{name}.json"
                );

            if (!File.Exists(configFilePath))
            {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var pageFilters = JsonConvert.DeserializeObject<List<PageFilterItem>>(fileContent);

            return pageFilters;
        }

        protected async Task<List<ColumnItem>> GetColumnsFromFile(string name)
        {
            var configFilePath = Path.Join(
                    _hostEnv.ContentRootPath,
                    "wwwroot",
                    "configs",
                    "list-view",
                    $"{name}.json"
                );
            if (!File.Exists(configFilePath))
            {
                return null;
            }

            var fileContent = await File.ReadAllTextAsync(configFilePath, Encoding.UTF8);

            var listView = JsonConvert.DeserializeObject<List<ColumnItem>>(fileContent);

            return listView;
        }
    }
}
