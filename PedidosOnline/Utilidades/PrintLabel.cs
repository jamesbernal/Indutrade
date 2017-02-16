using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Microsoft.Reporting.WebForms;
using System.Security;
using System.Security.Permissions;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using PedidosOnline.Models;
using System.Reflection;

namespace PedidosOnline.Utilidades
{

    public static class RDL_UTIL
    {
        private static Dictionary<m_plantillas, IList<Stream>> m_streams = null;

        private static Dictionary<m_plantillas, int> m_currentPageIndex = null;

        private static m_plantillas curTemplate = null;

        private static LocalReport localReport = null;

        private static string AppPath = null;


        private static string batFile = null;

        private static string CUR_PRINTER = null;


        public static string RDL_Generate_PDF_File_VIEW(string AppPath, DataSet reportData, m_plantillas CUR_TEMPLATE, string referenceName)
        {
            DataSet dsData = new DataSet("DataSet1");
            curTemplate = CUR_TEMPLATE;

            //Report File exists
            //Report File exists
            string reportPath = "RDL_FILES\\" + curTemplate.Header;


            if (!File.Exists(AppPath + "\\" + reportPath))
                throw new Exception("Report file does not exists.");

            try
            {

                //Rendering Report
                localReport = new LocalReport();
                localReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                localReport.EnableExternalImages = true;
                //Assembly.Load(,);
                try
                {
                    localReport.AddTrustedCodeModuleInCurrentAppDomain("Contoso.Utilities, Version=1.0.271.0, Culture=neutral, PublicKeyToken=89012dab8080cc90");
                    localReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
                    //localReport.AddTrustedCodeModuleInCurrentAppDomain("Barcode, Version=1.0.5.40001, Culture=neutral, PublicKeyToken=6dc438ab78a525b3");
                    localReport.AddTrustedCodeModuleInCurrentAppDomain("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                }
                catch (Exception e) { }
                try
                {
                    localReport.AddTrustedCodeModuleInCurrentAppDomain("Barcode, Version=1.0.5.40001, Culture=neutral, PublicKeyToken=6dc438ab78a525b3");
                }
                catch (Exception)
                {
                }
                try
                {
                    localReport.AddTrustedCodeModuleInCurrentAppDomain("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                }
                catch (Exception)
                {
                }

                bool aq = localReport.IsReadyForRendering;

                localReport.ReportPath = AppPath + "\\" + reportPath;
                    //AppPath + "\\" + reportPath;//@"D:\Pangea\Documents\Pangea Group\PEDIDOSONLINE.CO\ErpPortal.WebApp\RDL_FILES\DOCUMENTO_PEDIDO.rdl";//AppPath+reportPath;
                aq = localReport.IsReadyForRendering;

                //DataTable data=SQLBase.ReturnDataSet("Select top 1 rowid from erp_terceros ", new SqlConnection("Data Source=	SQL5017.Smarterasp.net;Initial Catalog=DB_9CAC35_demos;User Id=DB_9CAC35_demos_admin;Password=2015mBAA"));
                //dsData = SQLBase.ReturnDataSet("EXEC spUTIL_EXPORTAR_PDF_2  @entidad ='PEDIDO', @rowid_entidad ='8361'", new SqlConnection("Data Source=	SQL5017.Smarterasp.net;Initial Catalog=DB_9CAC35_demos;User Id=DB_9CAC35_demos_admin;Password=2015mBAA"));

                localReport.DataSources.Add(new ReportDataSource("DataSet1", reportData.Tables[0]));


            }
            catch (Exception ex)
            {
                throw;
            }


            string mimeType;
            string encoding;
            string fileNameExtension;
            string reportType = "PDF";

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

            string deviceInfo = curTemplate.Body.Replace("EMF", reportType);

            //"<DeviceInfo>" +
            //"  <OutputFormat>" + reportType + "</OutputFormat>" +
            //"  <PageWidth>10in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            //"  <MarginTop>0.1in</MarginTop>" +
            //"  <MarginLeft>0.1in</MarginLeft>" +
            //"  <MarginRight>0.1in</MarginRight>" +
            //"  <MarginBottom>0.1in</MarginBottom>" +
            //"</DeviceInfo>";

            byte[] renderedBytes;
            Warning[] warnings;
            string[] streams;
            bool a = localReport.IsReadyForRendering;
            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                // null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            string url_generated = Constantes.PRINT_GENERATE_DIR + "\\" + referenceName + "_" + UtilTool.ReturnGUID() + "_" + curTemplate.Nombre + "." + reportType;
            //1
            string fileGenerated = Path.Combine(AppPath, url_generated);

            BinaryWriter bn = new BinaryWriter(File.Open(fileGenerated, FileMode.Create));

            bn.Write(renderedBytes);

            bn.Flush();

            bn.Close();

            return "/" + url_generated;
            //return AppPath + url_generated;
        }


        public static void RDL_Generate_PDF_File_PRINT(string AppPath, DataSet reportData, m_plantillas CUR_TEMPLATE, string curPrinter, string referenceName)
        {

            curTemplate = CUR_TEMPLATE;

            CUR_PRINTER = curPrinter;

            //Inicializando variables usadas en la impresion
            m_streams = new Dictionary<m_plantillas, IList<Stream>>();

            m_currentPageIndex = new Dictionary<m_plantillas, int>();

            //Report File exists
            string reportPath = curTemplate.Header;


            if (!File.Exists(AppPath + "\\" + reportPath))
                throw new Exception("Report file does not exists.");

            try
            {
                //Rendering Report
                localReport = new LocalReport();
                localReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));

                localReport.EnableExternalImages = true;

                try
                {
                    localReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence);
                    localReport.AddTrustedCodeModuleInCurrentAppDomain("Barcode, Version=1.0.5.40001, Culture=neutral, PublicKeyToken=6dc438ab78a525b3");
                    localReport.AddTrustedCodeModuleInCurrentAppDomain("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                }
                catch { }

                localReport.ReportPath = reportPath;

                for (int i = 0; i < reportData.Tables.Count; i++)
                    localReport.DataSources.Add(new ReportDataSource("DataSet" + (i + 1).ToString(), reportData.Tables[i]));

            }
            catch (Exception ex)
            {
                throw;
            }

            //Print Report
            m_streams.Add(curTemplate, new List<Stream>());

            m_currentPageIndex.Add(curTemplate, 0);

            Export(localReport, curTemplate, "IMAGE");  //1 - Document, 2 -  Label


            m_currentPageIndex[curTemplate] = 0;



            //CUR_PRINTER = "Microsoft XPS Document Writer";


            //Ejecutar la impresion global en un Hilo            
            Thread th = new Thread(new ParameterizedThreadStart(Print));

            th.Start(new object[] { CUR_PRINTER, 1 });

            //Print(CUR_PRINTER);

        }

        #region Print Batch Methods

        private static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            try
            {
                Stream stream = new FileStream(Path.Combine(AppPath, Constantes.PRINT_GENERATE_DIR + "\\" + UtilTool.ReturnGUID() + name + "." + fileNameExtension), FileMode.Create);

                m_streams[curTemplate].Add(stream);

                return stream;
            }
            catch
            {
                return null;
            }
        }


        private static void Export(LocalReport report, m_plantillas template, string renderFormat)
        {
            //renderFormat = "IMAGE", "PDF", 

            string deviceInfo = template.Body;

            Warning[] warnings;

            //m_streams = new List<Stream>();


            try
            {
                report.Render(renderFormat, deviceInfo, CreateStream, out warnings);

                foreach (Stream stream in m_streams[curTemplate])
                    stream.Position = 0;

            }
            catch (Exception ex)
            {
                string exMessage = UtilTool.GetTechMessage(ex);

                throw new Exception(exMessage);
            }


        }


        private static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage = new Metafile(m_streams[curTemplate][m_currentPageIndex[curTemplate]]);
                ev.Graphics.DrawImage(pageImage, ev.PageBounds);

                m_currentPageIndex[curTemplate]++;
                ev.HasMorePages = (m_currentPageIndex[curTemplate] < m_streams[curTemplate].Count);
            }
            catch { }
        }


        private static void Print(object printerData)
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;


                string printerName = ((object[])printerData)[0].ToString();
                short NumCopies = short.Parse(((object[])printerData)[1].ToString());

                PrintDocument printDoc = new PrintDocument();

                printDoc.PrinterSettings.PrinterName = printerName.ToString();
                printDoc.PrinterSettings.Copies = NumCopies;
                printDoc.PrinterSettings.DefaultPageSettings.Landscape = true;
                if (!printDoc.PrinterSettings.IsValid)
                    throw new Exception("Can't found printer " + printerName.ToString());

                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                // printDoc.DefaultPageSettings.Landscape = true;
                printDoc.Print();

                /*
                //  Borrar archivos temporales de impresion ?
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(AppPath, Constantes.PRINT_GENERATE_DIR));

            
                foreach (FileInfo file in dir.GetFiles().Where(f => f.Name.Contains("emf") || f.Name.Contains("prn") || f.Name.Contains("tif")))
                {
                    if (file.CreationTime.AddDays(+8) < DateTime.Now)
                    {
                        try { file.Delete(); }
                        catch { }
                    }
                }
                */
            }
            catch { }
        }


        #endregion

    }
}



