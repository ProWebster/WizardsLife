function ajaxRequest(headline, replaceSelector, action, data) {
    $(replaceSelector).html("<center><img src=\"/images/spinner.svg\" style=\"width:50px; height:50px;\" /></center>"); // set spinner!

    var maxHeight = $(document).height() - (2 * $('#mainModalView').css("top").substring(0, $('#mainModalView').css("top").length - 2)) - 80;

    $("#mainModalView .modalHeader h2").html(headline);

    $.ajax({
        url: action,
        data: data,
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result) {
        var newHtml = $('<div style="position : absolute; left : -9999px;">' + result + '</div>').appendTo('body');
        var newHeight = newHtml.height() + 20;

        var theHeight = newHeight > maxHeight ? maxHeight : newHeight;

        $('#mainModalView .modalContent').animate({ minHeight: theHeight, maxHeight: maxHeight }, 300, function () {
            $(replaceSelector).html(result);
        });
    })
}


$(document).on("submit", "form[responseType!=PartialView]", function () {
    var form = $(this);
    var formData = $(this).serialize();

    form.children("input[type=submit]").attr('disabled', 'disabled');

    $.ajax({
        url: form.attr("action"),
        data: formData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        type: form.attr("method"),
        dataType: 'json'
    })
    .success(function (result) {
        if (result.Success)
        {
            if (result.Redirect != "" && result.Redirect !== undefined) {
                location.href = result.Redirect;
            } else {
                if (form.attr("type") == 'replace') {
                    $(form.attr("replace")).html(result.Content);
                } else if (form.attr("type") == 'function') {
                    window[form.attr("function")]();
                }
            }
        } else {
            if (form.attr("type") == 'replace') {
                $(form.attr("replace")).html(result.Content);
            }
        }

        form.children("input[type=submit]").attr('disabled', null);
    })
    .error(function (xhr, status) {
        $(form.attr("replace")).html(xhr.responseText);
    });

    return false;
});


$(document).on("submit", "form[responseType=PartialView]", function () {
    var form = $(this);
    var formData = $(this).serialize();

    form.children("input[type=submit]").attr('disabled', 'disabled');

    $.ajax({
        url: form.attr("action"),
        data: formData,
        type: form.attr("method"),
        dataType: 'html'
    })
    .success(function (result) {
        if (result.startsWith("redirect:"))
            location.href = result.replace("redirect:", "");
        else
            $(form.attr("replace")).html(result);

        form.children("input[type=submit]").attr('disabled', null);
    })
    .error(function (xhr, status) {
        $(form.attr("replace")).html(xhr.responseText);
    });

    return false;
});



function showModalView(headline, width, partialView)
{
    $('#mainModalView').css({ "width" : width + "px", "margin-left":(-width / 2) + "px"  });
    $('#mainModalView .modalContent').removeAttr("style");

    // Get max-height of modal! (height of screen minus 2*margin-top-value)
    var maxHeight = $(document).height() - (2 * $('#mainModalView').css("top").substring(0, $('#mainModalView').css("top").length - 2)) - 80; // 80 is the size of the header and the 2 padding (top bottom)

    // Set headline
    $("#mainModalView .modalHeader h2").html(headline);

    // Clear current content!
    $("#mainModalView .modalContent").html("<center><img src=\"/images/spinner.svg\" style=\"width:50px; height:50px;\" /></center>"); // Set spinner!

    // Show modal (with spinner)
    $('#mainModalView, #masterOverlay').fadeIn(200);

    // Get correct content for the main dialog!
    $.ajax({
        url: partialView,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result) {
        var newHtml = $('<div style="position : absolute; left : -9999px;">' + result + '</div>').appendTo('body');
        var newHeight = newHtml.height() + 20;

        var theHeight = newHeight > maxHeight ? maxHeight : newHeight;

        $('#mainModalView .modalContent').animate({ minHeight: theHeight, maxHeight: maxHeight }, 300, function () {
            $(this).html(result);
        });
    })
    .error(function (xhr, status) {
        alert(status);
    });
}

function closeDialog(_this) {
    $(_this).parent().parent().fadeOut(200);
    $('#masterOverlay').fadeOut(200);
}

function closeDialogFromObject(_this) {
    $(_this).fadeOut(200);
    $('#masterOverlay').fadeOut(200);
}
