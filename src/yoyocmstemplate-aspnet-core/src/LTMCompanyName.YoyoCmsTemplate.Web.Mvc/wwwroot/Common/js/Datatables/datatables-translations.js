/************************************************************************
* 给datatables 增加的多语言翻译组件                            *
*************************************************************************/
(function ($) {
    if (!$.fn.dataTable) {
        return;
    }

    function mapCultureToDatatablesTranslation(culture) {
        if (culture.name === 'zh-Hans' || culture.name === 'zh-CN') {
            return 'Chinese (Simplified, China)';
        }

        return culture.displayNameEnglish;
    }

    var translationsUrl = abp.appPath + 'Common/js/Datatables/Translations/' +
        mapCultureToDatatablesTranslation(abp.localization.currentCulture) +
        '.json';

    $.ajax(translationsUrl)
        .fail(function () {
            translationsUrl = abp.appPath + 'Common/js/Datatables/Translations/Chinese (Simplified, China).json';
            console.log('我们将采用简体作为数据的显示, 因为对应语言的 ' + abp.localization.currentCulture.displayNameEnglish + ' 不存在!');
        }).always(function () {
            $.extend(true, $.fn.dataTable.defaults, {
                language: {
                    url: translationsUrl
                },
                lengthMenu: [5, 10, 25, 50, 100, 250, 500],
                pageLength: 10,
                responsive: {
                    details: {
                        type: 'column'
                    }
                },
                dom: `<'row'<'col-sm-6 text-left'f>>
			<'row'<'col-sm-12'tr>>
			<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,
                order: []
            });
        });

})(jQuery);