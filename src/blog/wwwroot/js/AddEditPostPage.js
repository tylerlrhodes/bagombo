
$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    var target = $(e.target).attr("href") // activated tab
    if (target === "#PreviewTab") {
        var c = $("#Content").val();
        $.post({
            url: "/author/GetPreviewHtml",
            data: {
                content: c
            }
        }, function (data) {
            $("#PreviewTabInner").html(data);
            $("#PreviewTabInner > pre code").each(function (i, block) {
                hljs.highlightBlock(block);
            });
        });
    }
});
