using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Helpers
{
    public static class CONSTANTES
    {
        public static string DATETIME_DATABASE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string DATETIME_HUMAN_FORMAT = "ddd dd de MMMM del yyyy, a las HH:mm:ss";

        public static int SUPER_ADMIN_ROL = 1;
        public static int NivelCat = 0;
   
        public static string ERROR_MESSAGE = "<strong>Hubo un error.</strong> Por favor, llene todos los campos.";
        public static string ERROR_REPORTE_NO_MOVS = "<strong>Hubo un error.</strong> No se encontraron movimientos realizados para el periodo seleccionado, seleccione otro periodo";
        public static string SUCCESS_MESSAGE = "<strong>Actualizado.</strong> Los datos se han guardado correctamente.";
        public static string SUCCESS_REPORT = "<strong>Generado.</strong> El reporte se ha generado con éxito.";
        public static string SUCCESS = "success";
        public static string SUCCESS_FILE = "<strong>Success.</strong> Archivo generado exitosamente";
        public static string SUCCESS_DELETE = "<strong>Success.</strong> El elemento se ha eliminado exitosamente.";
        public static string SUCCESS_BAN = "<strong>Success.</strong> El elemento se ha anulado exitosamente.";
        public static string SUCCESS_UNBAN = "<strong>Success.</strong> El elemento se ha reestablecido exitosamente.";
        public static string ERROR = "error";
        public static string ERROR_UPDATE_MESSAGE = "<strong>Hubo un error al actualizar.</strong> Por favor, verifique la información a actualizar.";
        public static string ERROR_INSERT_MESSAGE = "<strong>Hubo un error al insertar.</strong> Por favor, verifique que la información ingresada.";
        public static string ERROR_INSERT_ACCOUNT = "<strong>Hubo un error al insertar.</strong> Por favor, verifique que la información ingresada";
        public static string ERROR_INSERT_DUPLICATE_USER = "<strong>Hubo un error al insertar.</strong> Ya existe un usuario con esa cuenta o correo.";
        public static string ERROR_LOGIN = "<strong>Hubo un error al realizar el Login.</strong> Por favor, verifique que su usuario y contraseña sean los correctos.";
        public static string ERROR_RECOVERY_PASSWORD = "<strong>La cuenta o correo ingresado no existe.</strong> Por favor, verifique la información.";
        public static string ERROR_FILE_DETAIL = "<strong>Error al generar el archivo.</strong> Por favor, verifique el rango de fechas.";
        public static string ERROR_EMPTY = "<strong>Error.</strong> No existe datos para exportar.";
        public static string ERROR_DELETE = "<strong>Error.</strong> No se pudo eliminar el elemento.";
        public static string ERROR_BAN = "<strong>Error.</strong> No se pudo anular el elemento.";
        public static string ERROR_UNBAN = "<strong>Error.</strong> No se pudo restablecer el elemento.";
        public static string ERROR_NO_DELETE = "<strong>Error.</strong> Se produjo un error mientras se intentaba eliminar el elemento.";
        public static string ERROR_NO_BAN = "<strong>Error.</strong> Se produjo un error mientras se intentaba anular el elemento.";
        public static string ERROR_NO_UNBAN = "<strong>Error.</strong> Se produjo un error mientras se intentaba restablecer el elemento.";
        public static string ERROR_DOCUMENTO_INGRESO_REPETIDO_1 = "<strong>Hubo un error al insertar.</strong> Ya existe un comprobante de INGRESO con el mismo Número de documento.";
        public static string ERROR_DOCUMENTO_INGRESO_REPETIDO_3 = "<strong>Hubo un error al insertar.</strong> Ya existe un comprobante de INGRESO ANULADO con el mismo Número de documento.";
        public static string ERROR_ITEMS_LIMIT = "<strong>Hubo un error.</strong> No se pueden retirar más items de los que hay en el Lote.";

        public static string SUCCESS_MESSAGE_FOR_RECOVERY_PASSWORD = "<strong>Se ha enviado un mensaje a su correo electrónico.</strong>";
        public static string SUCCESS_RECOVERY_PASSWORD = "<strong>Se ha enviado un correo con la contraseña.</strong>";
        public static string SUCCESS_PASSWORD_CHANGE = "<strong>Se ha cambiado la contraseña exitosamente.</strong>";

        public static string STATUS_FIELD = "status";
        public static string MESSAGE_FIELD = "message";

        public static int NRO_COLUMNAS = 12;

        public static string VALIDATE_MESSAGE_PRIVACIDAD = "Por favor, lea y acepte las Políticas de Privacidad y Condiciones de Uso para poder enviar su consulta";

        public static string URL_BITLY_API = "https://api-ssl.bitly.com/v3/shorten";

        public static int ROL_ADMIN = 2;
        public static int ROL_USUARIO_INT = 3;
        public static int ROL_USUARIO_EXT = 4;

        public static string ERROR_SELECT_RESPONSABLE = "<strong>Hubo un error.</strong> Por favor, seleccione al menos un responsable.";
    }

}
