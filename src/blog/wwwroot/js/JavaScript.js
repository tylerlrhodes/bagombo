
// JavaScript Here

/// <reference path="jquery.d.ts"/>


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

$("a.delpost").click(function (e) {
    var eid = $(this).attr('eid');
    if (eid === undefined)
    {
        return false;
    }
    $.ajax({
        type: 'DELETE',
        url: '/api/blogapi/deletepost/' + eid,
        data: null,
        success: function (result) {
            alert("deleted result: " + result);
            var sel = "#" + result;
            $(sel).remove();
        },
        error: function (result) {
            alert("Error!!!");
        }
    });
    e.preventDefault();
});
