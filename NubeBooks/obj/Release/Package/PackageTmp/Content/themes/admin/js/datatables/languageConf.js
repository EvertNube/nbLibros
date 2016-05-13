+function ($) { "use strict";

    $(function(){
        $('.dataTable').DataTable({
            pageLength: 50,
            language: {
                processing: "Procesando...",
                search: "Buscar:",
                lengthMenu: "Mostrar _MENU_ elementos",
                info: "Mostrando _START_ de _END_ total _TOTAL_",
                infoEmpty: "Mostrando 0 de 0 total 0",
                infoPostFix: "",
                loadingRecords: "Carga en curso...",
                zeroRecords: "Cero acumulados",
                emptyTable: "No existen datos en la tabla",
                paginate: {
                    first: "Primero",
                    previous: "Anterior",
                    next: "Siguiente",
                    last: "Último"
                },
                aria: {
                    sortAscending: ": Ordenar Ascendentemente",
                    sortDescending: ": Ordenar Descendentemente"
                }
            }
        });
    });
}(window.jQuery);