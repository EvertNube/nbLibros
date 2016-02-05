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

$.fn.datepicker.defaults.format = "dd/mm/yyyy";