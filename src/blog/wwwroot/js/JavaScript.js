
// JavaScript Here

/// <reference path="jquery.d.ts"/>


hljs.initHighlightingOnLoad();

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    var target = $(e.target).attr("href") // activated tab
    if (target === "#PreviewTab")
    {
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


$("#ChangePassword").change(function (e) {
    if ($(this).is(":checked")) {
        $("#passwordfield").css('display', 'block');
    }
    else {
        $("#passwordfield").css('display', 'none');
    }
});

$("#IsAuthor").change(function (e) {
    if ($(this).is(":checked")) {
        $("#authorfields").css('display', 'block');
    }
    else {
        $("#authorfields").css('display', 'none');
    }
});
