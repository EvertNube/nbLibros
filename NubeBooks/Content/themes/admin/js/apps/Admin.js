$(function () {
    var conta = 0;
    $(".rutas").on("click", function () {
        //var identificador = $("#note-1").data("identificador");
        //alert(identificador);
        //alert(this.id);
        //$('#reporte > tr').append("");
        $.getJSON("/Rutas/GetListEstadisticoRuta", { Id: this.id }, function (data) {
            for (var i = 0; i < data.length; i++) {
                $('#reporte > tbody:last').append(
                    '<tr>' + '<td>' +
                    data[i].RiesgoGeneral + '</td>' + '<td>' +
                    data[i].Cuenta + '</td>' + '<td>' +
                    data[i].Total + '</td>' + '</tr>'
                );
            }
        }).fail(function () {
            alert("El Servicio no está diponible. Inténtelo más tarde.");
            //fallenService = true;
        });
    });

    $("#limpiar").on("click", function () {
        $('#reporte tr:last').remove();
    });
});