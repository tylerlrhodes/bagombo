
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
