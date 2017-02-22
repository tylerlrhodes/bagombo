
// JavaScript Here

$("a.delpost").click(function (e) {
    var eid = $(this).attr('eid');
    if (eid === undefined)
    {
        return false;
    }
    $.ajax({
        type: 'DELETE',
        url: '/api/blog/deletepost/' + eid,
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
