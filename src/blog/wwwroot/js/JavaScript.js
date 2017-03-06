
// JavaScript Here

/// <reference path="jquery.d.ts"/>

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    var target = $(e.target).attr("href") // activated tab
    if (target === "#PreviewTab")
    {
        //alert(target);
        var c = $("#Content").val();
        $.post({
            url: "/author/GetPreviewHtml",
            data: {
                content: c
            }
        }, function (data) {
            //alert("Got data...");
            $("#PreviewTabInner").html(data);
        });
    }
});


$("#ChangePassword").change(function (e) {
    if ($(this).is(":checked")) {
        //alert('checked');
        $("#passwordfield").css('display', 'block');
    }
    else {
        //alert('unchecked');
        $("#passwordfield").css('display', 'none');
    }
});

$("#IsAuthor").change(function (e) {
    if ($(this).is(":checked")) {
        //alert('checked');
        $("#authorfields").css('display', 'block');
    }
    else {
        //alert('unchecked');
        $("#authorfields").css('display', 'none');
    }
});
