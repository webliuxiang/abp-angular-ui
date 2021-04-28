(function () {
    $(function () {

        var _hostCachingService = abp.services.app.hostCaching;
        var _webSiteLog = abp.services.app.webSiteLog;

        //Caching
        function clearCache(cacheName) {
            _hostCachingService.clearCache({
                id: cacheName
            }).done(function () {
                abp.notify.success(app.localize('CacheSuccessfullyCleared'));
            });
        }

        function clearAllCaches() {
            _hostCachingService.clearAllCaches().done(function () {
                abp.notify.success(app.localize('AllCachesSuccessfullyCleared'));
            });
        }

        $('.btn-clear-cache').click(function (e) {
            e.preventDefault();
            var cacheName = $(this).attr('data-cache-name');
            clearCache(cacheName);
        });

        $('#ClearAllCachesButton').click(function (e) {
            e.preventDefault();
            clearAllCaches();
        });

        //Web Logs
        function getWebLogs() {
            _webSiteLog.getLatestWebLogs({}).done(function (result) {
                var logs = getFormattedLogs(result.latestWebLogLines);
                $('#WebSiteLogsContent').html(logs);
                fixWebLogsPanelHeight();
            });
        }

        function downloadWebLogs() {
            _webSiteLog.downloadWebLogs({}).done(function (result) {
                app.downloadTempFile(result);
            });
        }

        function getFormattedLogs(logLines) {
            var resultHtml = '';
            $.each(logLines, function (index, logLine) {
                resultHtml += '<span class="log-line">' + _.escape(logLine)
                    .replace('DEBUG', '<span class="badge badge-secondary">DEBUG</span>')
                    .replace('INFO', '<span class="badge badge-info">INFO</span>')
                    .replace('WARN', '<span class="badge badge-warning">WARN</span>')
                    .replace('ERROR', '<span class="badge badge-danger">ERROR</span>')
                    .replace('FATAL', '<span class="badge badge-danger">FATAL</span>') + '</span>';
            });
            return resultHtml;
        }

        function fixWebLogsPanelHeight() {
            var windowHeight = $(window).height();
            var panelHeight = $('.full-height').height();
            var difference = windowHeight - panelHeight;
            var fixedHeight = panelHeight + difference;
            $('.full-height').css('height', (fixedHeight - 350) + 'px');
        }

        $('#DownloadAllLogsbutton').click(function (e) {
            e.preventDefault();
            downloadWebLogs();
        });

        $('#RefreshButton').click(function (e) {
            e.preventDefault();
            getWebLogs();
        });

        $(window).resize(function () {
            fixWebLogsPanelHeight();
        });

        getWebLogs();
    });
})();