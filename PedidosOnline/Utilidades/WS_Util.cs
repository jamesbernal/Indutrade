using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

namespace PedidosOnline.Utilidades
{
    public static class WS_Util  
    {

        internal static string ObtenerDatosEntidad(string entidad, string parametros, string cnnStr)
        {
            StringBuilder json = new StringBuilder();

            DataTable result = SQLBase.ReturnDataTable("spMOVIL_GETDATA @OPTION='" + entidad + "', @PARM='"+parametros+"'", "RETURN", new SqlConnection(cnnStr));          
            
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            
            Dictionary<string, object> row;

            foreach (DataRow dr in result.Rows)
            {
                row = new Dictionary<string, object>();

                foreach (DataColumn col in result.Columns)
                    row.Add(col.ColumnName, dr[col]);
                
                rows.Add(row);
            }

            return new JavaScriptSerializer().Serialize(rows);
        }
    }
}

