function displayInactive() {
    $("#btn-showInactive").click(function () {
        $(".inactive").fadeToggle(300);
    });
    $(".btn-showInactive").click(function () {
        $(".inactive").fadeToggle(300);
        var myvar = $(this).prop('checked');
        console.log(myvar);
        $(".btn-showInactive").each(function () {
            $(this).prop('checked', myvar);
        });
    });
}

$(document).ready(function () {
    displayInactive();
});

function baseURL(pUrl) {
    var listUrl = pUrl.split('/');
    listUrl.shift();
    var neleUrl = listUrl.length;
    var miUrl = '';
    var pos = listUrl.indexOf("Admin");

    var nDel = neleUrl - (pos + 1);
    for (var i = 0; i < nDel; i++) {
        listUrl.pop();
    }
    for (var i = 0; i < listUrl.length; i++) {
        miUrl += ('/' + listUrl[i]);
    }

    return miUrl;
}

$.fn.datepicker.defaults.format = "dd/mm/yyyy";