var item = [];
Number.prototype.round = function (places) {
    return +(Math.round(this + "e+" + places) + "e-" + places);
}
$(function () {
    getCliente();
    getConsultor();
    getItems();
    $("#ddlitem").on('change', function () { $("#txtcantidad").val(1); $("#txtprecio").val("0.00"); $("#txtdescuento").val('0.00'); });
    $("#btnagregar").on('click', AgregarItem);
    $('.input-group.date').datepicker();
    GetDetalle(Proforma.IdProforma);
    LlenarData();

});
function LlenarData()
{
    $("#txtfechaproforma").val(formatJSONDate(Proforma.FechaProforma));
    if (Proforma.ValidezOferta != 0) $("#ddlvalidezoferta").val(Proforma.ValidezOferta);
    $("#txtmetodopago").val(Proforma.MetodoPago);
    $("#txtfechaentrega").val(formatJSONDate(Proforma.FechaEntrega));
    $("#txtlugarentrega").val(Proforma.LugarEntrega);
    $("#ddlmoneda").val(Proforma.IdMoneda);
    if (Proforma.Estado != null) { $("#ddlestado").val(Proforma.Estado); }
    $("#txtfechafactura").val(formatJSONDate(Proforma.FechaFacturacion));
    $("#txtfechacobranza").val(formatJSONDate(Proforma.FechaCobranza));
    $("#txtcomentario").val(Proforma.ComenterioProforma);
    $("#txtadicional").val(Proforma.ComentarioAdiccional);
    $("#idproforma").val(Proforma.IdProforma);
    $("#codigoproforma").val(Proforma.CodigoProforma);
}
function getCliente()
{
    ajax("/Proformas/Proforma/GetClientes", {}, function (data) {
        if (typeof (data) == "object")
        {
            var selected = "";
            $.each(data, function (i, e){
                if (e.IdEntidadResponsable == Proforma.IdEntidadResponsable) selected = "selected";
                $("#ddlcliente").append('<option value="' + e.IdEntidadResponsable + '" ' + selected + '>' + e.Nombre + '</option>');
                selected = '';
            });
        }
    }).fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        alert(err);
    });
}
function getConsultor() {
    ajax("/Proformas/Proforma/GetConsultor", {}, function (data) {
        if (typeof (data) == "object") {
            var selected = "";
            $.each(data, function (i, e) {
                if (e.IdResponsable == Proforma.IdResponsable) selected = "selected";
                $("#ddlconsultor").append('<option value="' + e.IdResponsable + '" ' + selected + '>' + e.Nombre + '</option>');
                selected = '';
            });
        }
    }).fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        alert(err);
    });
}
function getItems() {
    ajax("/Proformas/Proforma/GetItem", {}, function (data) {
        if (typeof (data) == "object") {
            $.each(data, function (i, e) {
                $("#ddlitem").append('<option value="' + e.IdItem + '">' + e.Nombre + '</option>');
            });
        }
    }).fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        alert(err);
    });
}

function Save() {
    if ($("#tblitem tbody tr").length == 0) { alert('Debe registar al menos un item'); return false; }
    var proforma = {};
    proforma.IdProforma = $("#idproforma").val();
    proforma.CodigoProforma = $("#codigoproforma").val();
    proforma.IdResponsable = $("#ddlconsultor").val();
    proforma.IdEntidadResponsable = $("#ddlcliente").val();
    proforma.FechaProforma = $("#txtfechaproforma").val();
    proforma.ValidezOferta = $("#ddlvalidezoferta").val();
    proforma.MetodoPago = $("#txtmetodopago").val();
    proforma.FechaEntrega = $("#txtfechaentrega").val();
    proforma.LugarEntrega = $("#txtlugarentrega").val();
    //proforma.Estado = 1;
    proforma.IdMoneda = $("#ddlmoneda").val();
    proforma.Estado = $("#ddlestado").val();
    proforma.FechaFacturacion = $("#txtfechafactura").val();
    proforma.FechaCobranza = $("#txtfechacobranza").val();
    proforma.ComenterioProforma = $("#txtcomentario").val();
    proforma.ComentarioAdiccional = $("#txtadicional").val();
    //proforma.LugarEntrega = $("#txtlugarentrega").val();

    $.each(item, function (i, e) {
        proforma["DetalleProforma[" + i + "].IdItem"] = e.IdItem;
        proforma["DetalleProforma[" + i + "].NombreItem"] = e.NombreItem;
        proforma["DetalleProforma[" + i + "].Cantidad"] = e.Cantidad;
        proforma["DetalleProforma[" + i + "].PrecioUnidad"] = e.PrecioUnidad;
        proforma["DetalleProforma[" + i + "].Descuento"] = e.Descuento;
        proforma["DetalleProforma[" + i + "].MontoTotal"] = e.MontoTotal;
        proforma["DetalleProforma[" + i + "].TipoCambio"] = e.TipoCambio;
        proforma["DetalleProforma[" + i + "].PorcentajeIgv"] = e.PorcentajeIgv;
        proforma["DetalleProforma[" + i + "].Igv"] = e.Igv;
    });
    //myModalWait();
    $.ajax({
        url: '/Proformas/Proforma/SaveProforma',
        type: "POST",
        data: proforma,
        success: function (data) {
            if (data) {
                window.location.href = "/Proformas/Proforma/Index";
            }
        }
    }).fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        alert(err);
    }).always(function () {
        //window.setTimeout(function () { $("#myModalWait").modal('hide'); }, 5000);
    });
    return false;
}
function myModalWait() {
    $('#myModalWait').modal(
        {
            backdrop: 'static',
            keyboard: false
        }
        );
}
function GetDetalle(idproforma) {
    $.ajax({
        url: '/Proformas/Proforma/GetDetalleProforma',
        type: "POST",
        dataType: "json",
        data: { IdProforma: idproforma },
        success: function (data) {
            if (typeof (data) == 'object') {
                item = data;
                llenartabla();
            }
            //var IdCotizador = data;
        }
    }).fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        alert(err);
    }).always(function () {
        //window.setTimeout(function () { $("#myModalWait").modal('hide'); }, 5000);
    });
}
function ajax(url, data, suse)
{
    return $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: { IdProforma: idproforma },
            success: suse
        });
}
function AgregarItem()
{
    if ($('#ddlitem').val() != "")
    {
        var cadena = '';
        if ($("#txtcantidad").val() == "") cadena += "Debe de ingresar una cantidad\n";
        if ($("#txtprecio").val() == "") cadena += "Debe de ingresar un presio\n";
        if ($("#txtcantidad").val() == "0.00") cadena += "Debe de ingresar una cantidad mayor a cero\n";
        if ($("#txtprecio").val() == "0.00") cadena += "Debe de ingresar un presio mayor a cero\n";
        if (cadena != "") { alert(cadena); return;}
        var id = $('#ddlitem').val();
        var items01 = $.grep(item, function (e, i) { return (e.IdItem == id) })[0];
        if (items01 == undefined) {
            var lista = {};
            var descuento = $("#txtdescuento").val();
            
            lista.IdItem = id;
            lista.NombreItem = $('#ddlitem option:selected').text();
            lista.Cantidad = parseInt($("#txtcantidad").val());
            lista.PrecioUnidad = parseFloat($("#txtprecio").val());
            lista.TipoCambio = 3.15;
            lista.PorcentajeIgv = 18;
            lista.Igv = ((lista.Cantidad * lista.PrecioUnidad) * (lista.PorcentajeIgv / 100)).round(2);
            lista.MontoTotal = (lista.Cantidad * lista.PrecioUnidad).round(2);
            lista.Descuento = (lista.MontoTotal*(descuento/100)).round(2);
            lista.MontoTotal = (lista.MontoTotal - lista.Descuento).round(2);
            item.push(lista);
            llenartabla();
            $('#ddlitem').val('').change();
        }
        else {
            alert('este producto ya esta en la lista');
        }
    }
}
function llenartabla() {
    $("#tblitem tbody").empty();
    $.each(item, function (i, e) {
        $("#tblitem tbody").append('<tr><td>' + (i + 1) + '</td><td>' + e.IdItem + '</td><td>' + e.NombreItem + '</td><td>' + e.Cantidad
            + '</td><td></td><td>' + e.PrecioUnidad + '</td><td>' + e.Descuento +
            '</td><td>' + e.MontoTotal + '</td><td><span class="glyphicon glyphicon-remove" data-id="' + i + '" style="cursor:pointer"></span></td></tr>');
    });
    
    GetTotales();
}
function GetTotales() {
    var total = 0, igv = 0;
    $.each(item, function (i, e) { total += e.MontoTotal; igv += e.Igv; });
    $(".totaligv").text(igv.round(2));
    $(".total").text((total + igv).round(2));
}
function formatJSONDate(jsonDate) {
    if (jsonDate == null || jsonDate == '')
        return '';
    if (jsonDate.indexOf('/Date') != -1) {
        var newDate = new Date(parseInt(jsonDate.substr(6))),
         anno = newDate.getFullYear(),
         mes = newDate.getMonth() + 1,
         dia = newDate.getDate();
        //if (mes == 0) { mes = 12; anno -= 1; }
        mes = (mes < 10) ? ("0" + mes) : mes,
        dia = (dia < 10) ? ("0" + dia) : dia;
        return dia + '/' + mes + '/' + anno;
        //return newDate.toLocaleString('es-PE');
    }
    else { return jsonDate; }
}