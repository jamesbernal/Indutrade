using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;

namespace ErpPortal.WebApp.Controllers
{
    public partial class MastersController : Controller
    {


        #region: :::: EXPORTAR PDF ::::

        public String ExportarPdf(string entidad, string parm1)
        {
            PedidosOnlineEntities db = new PedidosOnlineEntities();
            string llamada = "EXEC spPROFORMA_EXPORTAR_PDF " + parm1 + "";
            DataSet dt = SQLBase.ReturnDataSet(llamada,
            new SqlConnection(db.Database.Connection.ConnectionString));
            m_plantillas m_plantilla = db.m_plantillas.Where(f => f.Nombre == "RDL_" + entidad).First();
            return RDL_UTIL.RDL_Generate_PDF_File_VIEW(Server.MapPath("//"), dt, m_plantilla, entidad);
        }
#endregion
    }


}