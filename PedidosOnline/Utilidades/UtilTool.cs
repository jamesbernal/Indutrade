using System;
using System.Collections.Generic;
using System.Linq;
using PedidosOnline.Models;
using System.Data;
using System.Web.UI.WebControls;

namespace PedidosOnline.Utilidades
{
    public class UtilTool
    {

        private static readonly Random _rng = new Random();
        private const string _chars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ClaveRandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer).Trim();
        }

        public static string CryptPasswd(string data, string cryptKey)
        {
            if (string.IsNullOrEmpty(data))
                return "";


            Crypto cpt = new Crypto(Crypto.SymmProvEnum.DES);
            return cpt.Encrypting(data, cryptKey).Trim();

        }

        public static string DeCryptPasswd(string value, string key)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            Crypto cpt = new Crypto(Crypto.SymmProvEnum.DES);
            return cpt.Decrypting(value, key);

        }

        internal static string CryptPasswdV2(string data)
        {
            if (string.IsNullOrEmpty(data))
                return "";

            CryptoV2 cpt = new CryptoV2();
            return cpt.Encriptar(data).Trim();
        }

        public static string DeCryptPasswdV2(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            CryptoV2 cpt = new CryptoV2();
            return cpt.DesEncriptar(value);

        }

        public static string ReturnGUID()
        {
            return Guid.NewGuid().ToString().ToUpper().Remove(8);
        }

        public static string GetTechMessage(Exception ex)
        {
            if (ex == null)
                return "";

            string msg = ex.Message;
            Exception tmpEx = ex.InnerException;

            while (tmpEx != null)
            {
                msg += "\n" + tmpEx.Message;
                tmpEx = tmpEx.InnerException;
            }

            return msg;
        }

        public static string ObtenerOpcionConfiguracion(string optionCode, int rowid_empresa)
        {
            string result = "";

            try
            {
                //result = (new PedidosOnlineEntities()).m_empresas_config
                //    .Where(f => f.step == optionCode && (rowid_empresa == 0 || f.rowid_empresa == rowid_empresa))
                //    .First().parametros;
            }
            catch { }

            return result;

        }

        public static string ObtenerParametro(string cod_parametro)
        {
            try
            {
                //return (new PedidosOnlineEntities()).Parametro
                //       .Where(f => f.Codigo == cod_parametro)
                //       .First().Valor;
                return "";
            }
            catch
            {
                return "";
            }
        }

        //public static string ObtenerParametroV2(PedidosOnlineEntities _db, string cod_parametro)
        //{
        //    try
        //    {
        //        return (_db.Parametro.Where(f => f.Codigo == cod_parametro).First().Valor);
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        public static string FormatDouble(double valor)
        {
            return ((double)valor).ToString("N0");
        }

        public static DateTime GetDateTime()
        {
            try
            {
                TimeZoneInfo zona = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                try
                {
                    string id_zona = ObtenerParametro("ZONA_HORARIA");
                    /**************************************************************/
                    //Permite conocer todos los id de las zonas horarias
                    /**************************************************************/
                    //var infos = TimeZoneInfo.GetSystemTimeZones();
                    //foreach (var info in infos)
                    //{
                    //    Console.WriteLine(info.Id);
                    //}
                    /**************************************************************/
                    if (id_zona.Length > 1)
                    {
                        zona = TimeZoneInfo.FindSystemTimeZoneById(id_zona);
                    }
                    else
                    {
                        zona = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                    }

                }
                catch (Exception e)
                {
                    zona = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                }

                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, zona);
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
    public class reg_calculadora
    {
        public Calculadora Calculadora { get; set; }
        public List<CalculadoraItems> items { get; set; }
    }
    public class reg_Proforma
    {
        public  Proforma Proforma { get; set; }
        public List<CalculadoraItems> items { get; set; }
    }
    public class ModalReferencias
    {
        public int rowid { get; set; }
        public string valor { get; set; }
        public string columna1 { get; set; }
        public string columna2 { get; set; }
        public string columna3 { get; set; }
        public string columna4 { get; set; }
    }
    public static class Constantes
    {

        //::::::::::::::::::::CONSTANTES REPORTE GERENCIAL PEDIDOS :::::::::::::::::::://
        //public static int[] ARREGLO_RUTERO=new int[500];
        //public const string REPORTE_GERENCIAL_PEDIDOS_campo_filtro_fechacreacion = "FECHA_CREACION";
        //public const string REPORTE_GERENCIAL_PEDIDOS_campo_filtro_fechacmod = "FECHA_MODIFICACION";

        ////::::::::::::::::::::CONSTANTES MAESTRO USUARIO :::::::::::::::::::://
        //public const string USUARIO_TIPO_call_center = "CALL_CENTER";
        //public const string USUARIO_TIPO_coordinador_canal = "COORDINADOR_CANAL";
        ////::::::::::::::::::::CONSTANTES PEDIDO :::::::::::::::::::://
        //public const string PEDIDO_MODULO_CREACION_autoservicio = "AUTOSERVICIO";
        //public const string PEDIDO_MODULO_CREACION_venta = "VENTAS";

        ////::::::::::::::::::::CONSTANTES ACTIVIDADES :::::::::::::::::::://
        //public const int ACTIVIDAD_ESTADO_programada = 1001;

        public const string ACTIVIDAD_TIPO_RELACION_cliente = "CLIENTE";
        //public const string ACTIVIDAD_TIPO_RELACION_potencial = "Prospecto";
        public const string ACTIVIDAD_TIPO_RELACION_contacto = "CONTACTO";
        //public const string ACTIVIDAD_TIPO_RELACION_pedido = "Pedido";
        //public const string ACTIVIDAD_TIPO_RELACION_pqrs = "PQRS";
        //public const string ACTIVIDAD_TIPO_RELACION_oportunidad = "Oportunidad";

        ////::::::::::::::::::::ESTADO SOPORTE:::::::::::::::::::://
        //public const string ROL_ID_PROYECT_MANGER = "PROYECT_MANAGER";//ROL DE SOPORTE ADMINISTRADOR

        //public const int SOPORTE_ESTADO_nuevo = 107;
        //public const int SOPORTE_ESTADO_cerrado = 108;
        //public const int SOPORTE_ESTADO_produccion = 101;
        //public const int SOPORTE_ESTADO_rechazado = 106;

        //public const string SOPORTE_TTIPO_RESPUESTA_seguimiento = "SEGUIMIENTO";
        //public const string SOPORTE_TTIPO_RESPUESTA_cambioestado = "CAMBIO.ESTADO";

        //public const string SOPORTE_TTIPO_Produccion = "PRODUCCION";

        //public const int SOPORTE_RESPUESTA_ESTADO_disponible = 201;
        //public const int SOPORTE_RESPUESTA_ESTADO_anulado = 202;

        ////::::::::::::::::::::ESTADO QUEMADOS:::::::::::::::::::://
        //public const int SOPORTE_ROWID_edex = 1;
        ////:::::::::::::::::::::::::::::::::::::::::::::::::::::://


        ///*OPORTUNIDADES*/
        //public const int OPORTUNIDAD_Nueva = 1201;
        //public const int OPORTUNIDAD_Ganada = 1205;
        //public const int OPORTUNIDAD_Pedida = 1206;
        ///**/


        public const string CryptString = "MBASOFTWARE";

        //public const int DOCUMENTOS_EnElaboracion = 101;
        //public const int DOCUMENTOS_Confirmado = 102;
        //public const int DOCUMENTOS_EnviadoErp = 103;
        //public const int DOCUMENTOS_ErrorErp = 104;
        //public const int DOCUMENTOS_Anulado = 105;

        //public const string FILTRO = "";


        //// ::::::::::: Estados PEDIDOS para Edex :::::::::::::::::
        //public const int PEDIDO_ESTADO_en_elaboracion = 101;


        //// ::::::::::: Estados Adicionales para Edex :::::::::::::::::
        //public const int DOCUMENTOS_PendienteAprobacion = 106;
        //public const int DOCUMENTOS_AprobacionNivel = 107;
        //public const int DOCUMENTOS_Rechazo_AprobacionNivel = 108;
        //public const int DOCUMENTOS_SolicitudEnviada = 109;
        //public const int DOCUMENTOS_Retenido = 110;
        //public const int DOCUMENTOS_Aprobado = 111;
        //public const int DOCUMENTOS_EnAlistamiento = 112;
        //public const int DOCUMENTOS_Entregado = 113;
        //public const int DOCUMENTOS_Despachado = 114;

        ////Estados PQRS Edex
        //public const int PQRS_ESTADO_Registro = 601;
        //public const int PQRS_ESTADO_validacion = 602;
        //public const int PQRS_ESTADO_aplica = 603;
        //public const int PQRS_ESTADO_no_aplica = 604;
        //public const int PQRS_ESTADO_en_proceso = 605;
        //public const int PQRS_ESTADO_despacho = 606;
        //public const int PQRS_ESTADO_cerrado = 607;

        //// ::::::::::: Estados Adicionales para Encoexpress :::::::::::::::::
        //public const int DOCUMENTOS_SOLICITUD_EnElaboracion = 701;
        //public const int DOCUMENTOS_SOLICITUD_Confirmado = 702;
        //public const int DOCUMENTOS_SOLICITUD_EnviadoErp = 703;
        //public const int DOCUMENTOS_SOLICITUD_ErrorErp = 704;
        //public const int DOCUMENTOS_SOLICITUD_Anulado = 705;
        //// ::::::::::: Estado envio de la solicitud Encoexpress :::::::::::::::::
        //public const int SOLICITUD_ENVIO_ESTADO_Cumplida = 801;

        ////public const int DOCUMENTOS_InicioNuflex = 111;

        //public const string ENTIDAD_Pedido = "PEDIDO";
        //public const string ENTIDAD_Traslado = "TRASLADO";


        //public const string ROL_Admin = "ADMIN";
        //public const string ROL_Empresa = "EMPRESA";
        //public const string ROL_Vendedor = "VENDEDOR";
        //public const string ROL_Inventarios = "INVENTARIOS";
        //public const string ROL_ClienteAutoservicio = "CLIENTE";
        //public const string ROL_ServiCliente = "SERVICLIENTE";

        ////ROLES EDEX
        //public const string ROL_Telemercaderista = "TELEMERCADERISTA";
        //public const string ROL_Asesor = "ASESOR";
        //public const string ROL_Logistica = "LOGISTICA";
        //public const string ROL_Cordinador = "COORDINADOR";
        //public const string ROL_Aprobador = "APROBADOR";
        //public const string ROL_Cliente = "CLIENTE";
        //public const string ROL_SAC = "SAC";
        //public const string ROL_ADMINISTRADOR = "ADMINISTRADOR";

        //public const string MODULOS_Solicitud = "SOLICITUD";
        //public const string MODULOS_PedidoHeader = "PEDIDOS";
        //public const string MODULOS_PedidoDetail = "PEDIDOS.DETAIL";
        //public const string MODULOS_PedidoIngresadosV2 = "PEDIDOS.INGRESADOS.V2";
        //public const string MODULOS_PedidoIngresadosV3 = "PEDIDOS.INGRESADOS.V3";
        //public const string MODULOS_Pqrs = "PQRS";

        //public const string TIPO_PQRS_Solicitud = "SOLICITUD";
        //public const string TIPO_PQRS_Pedido = "PEDIDO";


        ////EMPRESA
        //public const string ENTIDAD_encoexpress = "E";


        ////RDL
        public const string PRINT_GENERATE_DIR = "PRINT";

    }

    public class Contratos
    {
        public int RowID { get; set; }
        public string Titulo { get; set; }
        public string label { get; set; }
        public string puertoc { get; set; }
        public string puertod { get; set; }
        public int RowIDPro { get; set; }
    }

    public class Proformas
    {

        public int RowID { get; set; }

        public string vendedor { get; set; }

        public string comprador { get; set; }

        //public string producto { get; set; }

        //public string cantidad { get; set; }

        //public string empaque { get; set; }

        public string origen { get; set; }

        public string destino { get; set; }

        //public double precio { get; set; }

        //public double peso { get; set; }

        public string formaPago { get; set; }

        public string label { get; set; }


    }

    public class Ciudades
    {
        public int RowID { get; set; }
        public string label { get; set; }
    }

    public class MatricesBL
    {
        public int RowID { get; set; }
        public string cantidadC { get; set; }
        public string expedidor { get; set; }
        public string consignatario { get; set; }
        public string puertoC { get; set; }
        public string puertoD { get; set; }
        public string label { get; set; }
    }

    public class Terceros
    {
        public int RowID { get; set; }
        public string RazonSocial { get; set; }
        public string Nit { get; set; }
        public string label { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Ciudad { get; set; }

    }

    public class Puertos
    {
        public int RowID { get; set; }
        public string label { get; set; }
    }

    public class Empresas
    {
     public int RowID { get; set; }
     public string label { get; set; }
    }

public class MotoNaves
    {
        public int RowID { get; set; }
        public string label { get; set; }
        public string nombre { get; set; }
        public int numViajes { get; set; }
    }
    public class Vehiculos
    {
        public int RowID { get; set; }
        public string placa { get; set; }
        public string label { get; set; }
    }

    public class ActividadContacto
    {
        public string tipo { get; set; }
        public string tema { get; set; }
        public string ind_prioridad { get; set; }
        public string descripcion { get; set; }
        public string fecha_inicial { get; set; }
        public string fecha_final { get; set; }
        public string estado { get; set; }
    }

    public class ListadoTerceros
    {
        public int rowID { get; set; }
        public string Nit { get; set; }
        public string RazonSocial { get; set; }
        public string ContactoPrincipal { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Cliente { get; set; }
        public string Proveedor { get; set; }
        public string Accionista { get; set; }
        public string Comercial { get; set; }
    }

    public class DatoValorGrafica
    {
        public string Dato { get; set; }
        public double Valor { get; set; }
    }
    public class TerceroCompletInfo
    {
        public PedidosOnline.Models.Tercero tercero { get; set; }
        public ContactoERP contacto_principal { get; set; }
    }
    public class fullCalendar
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool allDay { get; set; }
        public bool url { get; set; }
    }
}