$(document).ready(function () {
    var arrayResp = $.map(responsablesReporte.split(","), function (value) {
        return parseInt(value, 10);
    });
    jQuery.ajaxSettings.traditional = true;
    $.ajax({
        async: true,
        url: "/Admin/GetTareasPorResponsable",
        data: { responsables: arrayResp },
        cache: false,
        dataType: "json"
    }).done(function (data) {
        $.each(data, function (i, v) {
            if (v.start != null) {
                var dateInicio = new Date(parseInt(v.start.substr(6)));
                v.start = new Date(dateInicio.getFullYear(), dateInicio.getMonth(), dateInicio.getDate(), 00, 01);//dateInicio.getHours(), dateInicio.getMinutes());
            }
            if (v.end != null) {
                var dateFin = new Date(parseInt(v.end.substr(6)));
                v.end = new Date(dateFin.getFullYear(), dateFin.getMonth(), dateFin.getDate(), 00, 01);//dateFin.getHours(), dateFin.getMinutes());//00, 01)
            }
        });
        
        loadCalendar(data);
    }).fail(function () {
        alert('Error al intentar obtener las Tareas del Calendario. Por favor, actualice la página o presione F5.');
    });

    
});

  function loadCalendar(data){
      
    // fullcalendar
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $('.calendar').each(function() {
        var tareas = [];
        var idResponsable = $(this).data("target");
        $.each(data, function (i, v) {
            if (v.idResponsable === idResponsable) { tareas.push(v); }
        });
      $(this).fullCalendar({
        header: {
          left: 'prev',
          center: 'title',
          right: 'next'
        },
        weekends: true,
        defaultView: 'month',
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Setiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Set', 'Oct', 'Nov', 'Dic'],
        titleFormat: {
            month: 'MMMM yyyy',                             // September 2009
            week: "MMMM d[ yyyy]{ '-'[ MMMM] d', ' yyyy}", // Sep 7 - 13 2009
            day: 'dddd, MMM d, yyyy'                  // Tuesday, Sep 8, 2009
        },
        columnFormat: {
            month: 'ddd',    // Mon
            week: 'ddd d/M', // Mon 9/7
            day: 'dddd d/M'  // Monday 9/7
        },
        editable: false,
        droppable: false, // this allows things to be dropped onto the calendar !!!
        events: tareas
      });

    });

    $(document).on('click', '.dayview', function () {
        var target = $(this).data("target");
        $('#calendar-' + target).fullCalendar('changeView', 'basicDay')
    });

    $('.weekview').click(function () {
        var target = $(this).data("target");
        $('#calendar-' + target).fullCalendar('changeView', 'basicWeek')
    });

    $('.monthview').on('click', function () {
        var target = $(this).data("target");
        $('#calendar-' + target).fullCalendar('changeView', 'month')
    });

  };