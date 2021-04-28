
(function ($) {
    if (!$) {
        return;
    }
    $.fn.serializeFormToObject = function() {
        //序列化为数组
        var data = $(this).serializeArray();

        //添加包括被禁用的属性名称
        $(':disabled[name]', this)
            .each(function() {
                data.push({ name: this.name, value: $(this).val() });
            });

        //映射为对象
        var obj = {};
        data.map(function(x) { obj[x.name] = x.value; });

        return obj;
    };

})(jQuery);