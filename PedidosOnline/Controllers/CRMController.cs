using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PedidosOnline.Models;
using PedidosOnline.Utilidades;
using System.IO;
namespace PedidosOnline.Controllers
{
    public class CRMController : Controller
    {
        PedidosOnlineEntities db = new PedidosOnlineEntities();

        #region :::::METODOS GENERALES:::::
        private FormCollection DeSerialize(FormCollection FormData)
        {
            FormCollection collection = new FormCollection();
            //un-encode, and add spaces back in
            string querystring = Uri.UnescapeDataString(FormData["FormData"]).Replace("+", " ");
            var split = querystring.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> items = new Dictionary<string, string>();
            foreach (string s in split)
            {
                string text = s.Substring(0, s.IndexOf("="));
                string value = s.Substring(s.IndexOf("=") + 1);

                if (items.Keys.Contains(text))
                    items[text] = items[text] + "," + value;
                else
                    items.Add(text, value);
            }
            foreach (var i in items)
            {
                collection.Add(i.Key, i.Value);
            }
            return collection;
        }
        public JsonResult Get_Ruta_File()
        {

            try
            {
                HttpPostedFileBase file = Request.Files[0];

                var ruta = "";
                if (file != null && file.ContentLength > 0)
                {
                    string Guid_Img = Guid.NewGuid().ToString();
                    Guid_Img = Guid_Img.Substring(1, 7);
                    var nombreArchivo = Guid_Img.Trim() + "_" + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Recurso/ImagenPerfil"), nombreArchivo);

                    ruta = "/Recurso/ImagenPerfil/" + nombreArchivo;
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Recurso/ImagenPerfil"));
                    }
                    catch (Exception ex) { }
                    file.SaveAs(path);
                }

                if (string.IsNullOrEmpty(ruta))
                {
                    Response.StatusCode = 500;
                    return Json("No hay archivo adjunto");
                }
                return Json(ruta);
            }
            catch { }

            Response.StatusCode = 500;
            return Json("No hay archivo adjunto");
        }

        public string SelectPaises()
        {
            string result = "<option value=''>-Seleccionar-</option>";
            foreach (var item in db.Pais.ToList())
            {
                result += "<option value='" + item.RowID + "'>" + item.Nombre + "</option>";
            }
            return result;
        }
        public string SelectDepartamento(int? rowid)
        {
            string result = "<option value=''>-Seleccionar-</option>";
            foreach (var item in db.Departamento.Where(f => f.PaisID == rowid).ToList())
            {
                result += "<option value='" + item.RowID + "'>" + item.Nombre + "</option>";
            }
            return result;
        }
        public string SelectCiudades(int? rowid)
        {
            string result = "<option value=''>-Seleccionar-</option>";
            foreach (var item in db.Ciudad.Where(f => f.DepartamentoID == rowid).ToList())
            {
                result += "<option value='" + item.RowID + "'>" + item.Nombre + "</option>";
            }
            return result;
        }
        #endregion

        #region :::::ERP TERCERO:::::
        [CheckSessionOut]
        public ActionResult ListadoTercerosSucursal()
        {
            ViewBag.listercero = db.Tercero.ToList();
            return View();
        }
        [CheckSessionOut]
        public ActionResult Tercero(int rowid_tercero)
        {
            //todos los contactos
            ViewBag.ListadoContactos = db.Contacto.ToList();


            //los combos
            //ViewBag.Tipos_Industria = _db.m_metaclasses.Where(f => f.class_code == "TERCERO.TIPO.INDUSTRIA").ToList();
            ViewBag.Tipos_Industria = db.Opcion.Where(f => f.Agrupacion.Nombre == "TERCERO.TIPO.INDUSTRIA").ToList();
            //ViewBag.Clasificacion = _db.m_metaclasses.Where(f => f.class_code == "TERCERO.CLASIFICACION").ToList();
            //ViewBag.Clasificacion = db.Opcion.Where(f => f.Agrupacion.Nombre == "TERCERO.CLASIFICACION").ToList();

            //usuario actual
            //var usuario = ((usuario)Session["NombreUsuario"]).nombre_usuario;
            var usuario = ((Usuario)Session["curUser"]).NombreUsuario;

            //el tercero a observar
            //var tercero = _db.erp_terceros.Where(f => f.rowid == rowid_tercero).FirstOrDefault();
            var tercero = db.Tercero.Where(f => f.RowID == rowid_tercero).FirstOrDefault();

            //actividades del tercero.
            ViewBag.ListaActividades = db.Actividad.Where(f => f.TerceroERPID == tercero.RowID).ToList();
            //ViewBag.ListaActividades = _db.crm_actividades.Where(f => f.rowid_relacion == tercero.rowid).OrderByDescending(f => f.fecha_creacion).ToList();

            //puntos de envio del tercero
            ViewBag.ListadoPuntoEnv = db.PuntoEnvio.Where(f => f.VendedorID == tercero.RowID).ToList();

            ContactoERP cp;
            //sucursales del tercero
            ViewBag.ListadoSucursales = db.Sucursal.Where(f => f.TerceroID == rowid_tercero).ToList();


            try
            {
                cp = db.ContactoERP.Where(f => f.RowID == tercero.ContactoERPID).First();
            }
            catch (Exception e)
            { cp = new ContactoERP(); }


            TerceroCompletInfo tcrm = new TerceroCompletInfo();
            tcrm.tercero = db.Tercero.Where(f => f.RowID == rowid_tercero).FirstOrDefault();
            tcrm.contacto_principal = cp;


            //tcrm.contacto_principal = cp;
            #region Pedidos - Diarios
            //List<t_pedido> ListaPedidos_ValorGrafica = new List<t_pedido>();


            //if (UtilTool.ObtenerParametro("USAR_DATOS_ERP_QUEMADOS") == "T")
            //{
            //    ListaPedidos_ValorGrafica = _db.t_pedido
            //            .Where(f => f.fecha_pedido >= DateTime.Today.AddDays(-30))
            //            .OrderBy(f => f.fecha_pedido).ToList();
            //}
            //else
            //{
            //    ListaPedidos_ValorGrafica = _db.t_pedido
            //            .Where(f => f.numpedido_erp != null && f.numpedido_erp != "" && f.erp_terceros_sucursale.rowid_tercero == rowid_tercero)
            //            .Where(f => f.fecha_pedido >= DateTime.Today.AddDays(-30))
            //            .OrderBy(f => f.fecha_pedido).ToList();
            //}

            //List<DatoValorGrafica> listaDias = ListaPedidos_ValorGrafica.GroupBy(f => f.fecha_pedido).Select(group => new DatoValorGrafica
            //{
            //    Dato = "", 
            //    Valor = Math.Round(group.Sum(f => f.valor_total), 0),
            //    DatoFecha = group.Key
            //}).ToList();

            List<DatoValorGrafica> listaDiasFinal = new List<DatoValorGrafica>();

            //int z = 1;
            //for (int i = -30; i <= 0; i++)
            //{
            //    DateTime curFecha = DateTime.Today.AddDays(i);
            //    DatoValorGrafica curDato = listaDias.Where(f => f.DatoFecha == curFecha).FirstOrDefault();

            //    if (curDato == null)
            //        curDato = new DatoValorGrafica
            //        {
            //            Dato = (curFecha.Day == 1 || i == -30) ? curFecha.ToString("MMM/dd") : curFecha.ToString("dd"),
            //            Valor = 0,
            //            DatoFecha = curFecha,
            //            Seq = z++,
            //        };
            //    else
            //    {
            //        curDato.Dato = (curFecha.Day == 1 || i == -30) ? curFecha.ToString("MMM/dd") : curFecha.ToString("dd");
            //        curDato.Seq = z++;
            //    }

            //    listaDiasFinal.Add(curDato);
            //}


            ViewBag.listaPedidos = listaDiasFinal;

            #endregion

            #region Pedidos - Mensuales
            //List<t_pedido> ListaPedidos_ValorGrafica_M = new List<t_pedido>();
            //if (UtilTool.ObtenerParametro("USAR_DATOS_ERP_QUEMADOS") == "T")
            //{
            //    ListaPedidos_ValorGrafica_M = _db.t_pedido
            //            .Where(f => f.fecha_pedido >= DateTime.Today.AddMonths(-11).AddDays(-1 * DateTime.Today.Day + 1))
            //            .OrderBy(f => f.fecha_pedido).ToList();
            //}
            //else
            //{
            //    ListaPedidos_ValorGrafica_M = _db.t_pedido
            //            .Where(f => f.fecha_pedido >= DateTime.Today.AddMonths(-11).AddDays(-1 * DateTime.Today.Day + 1))
            //            .Where(f => f.numpedido_erp != null && f.numpedido_erp != "" && f.erp_terceros_sucursale.rowid_tercero== rowid_tercero)
            //            .OrderBy(f => f.fecha_pedido).ToList();
            //}
            //List<DatoValorGrafica> listaMes = ListaPedidos_ValorGrafica_M.GroupBy(f => f.fecha_pedido.Month)
            //            .Select(group => new DatoValorGrafica
            //            {
            //                Dato = "",
            //                Valor = Math.Round(group.Sum(f => f.valor_total), 0),
            //                DatoFecha = group.Min(f => f.fecha_pedido)
            //            }).ToList();

            List<DatoValorGrafica> listaMesFinal = new List<DatoValorGrafica>();

            //for (int i = -11; i <= 0; i++)
            //{
            //    DateTime curFecha = DateTime.Today.AddMonths(i);
            //    curFecha = curFecha.AddDays(-1 * curFecha.Day + 1);

            //    DatoValorGrafica curDato = listaMes.Where(f => f.DatoFecha.Value.ToString("MMM/yy") == curFecha.ToString("MMM/yy")).FirstOrDefault();

            //    if (curDato == null)
            //        curDato = new DatoValorGrafica
            //        {
            //            Dato = curFecha.ToString("MMM/yy"),
            //            Valor = 0,
            //            DatoFecha = curFecha
            //        };
            //    else
            //        curDato.Dato = curFecha.ToString("MMM/yy");

            //    listaMesFinal.Add(curDato);
            //}


            ViewBag.listaPedidosAnual = listaMesFinal;

            #endregion
            return View(tcrm);
        }
        #endregion

        #region :::::TERCEROS POTENCIALES:::::

        public JsonResult InformacionContacto(int? rowid)
        {
            var data = (from reg in db.Contacto.Where(f => f.RowID == rowid)
                        select new
                        {
                            ciudad=reg.Ciudad.Nombre,
                            departamento=reg.Ciudad.Departamento.Nombre,
                            pais=reg.Ciudad.Departamento.Pais.Nombre,
                            telefono1=reg.Telefono1,
                            direccion=reg.Direccion1,
                            telefono2=reg.Telefono2,
                        });

            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Modal_SeleccionContacto()
        {
            return View(db.Contacto.ToList());
        }

        [HttpPost]
        public ActionResult Cliente_Nuevo(TerceroPotencial model)
        {
            try
            {
                TerceroPotencial tercero_potencial = db.TerceroPotencial.Where(f => f.RowID == model.RowID).FirstOrDefault();

                if (tercero_potencial == null)
                {
                    tercero_potencial = new TerceroPotencial();
                    tercero_potencial.Activo = true;
                    tercero_potencial.FechaCreacion = UtilTool.GetDateTime();
                    tercero_potencial.UsuarioCreacion = ((Usuario)Session["curUser"]).NombreUsuario;
                }
                else
                {
                    tercero_potencial.FechaModificacion = UtilTool.GetDateTime();
                    tercero_potencial.UsuarioModificacion = ((Usuario)Session["curUser"]).NombreUsuario;
                }


                tercero_potencial.Descripcion = model.Descripcion;
                tercero_potencial.EstadoID = model.EstadoID;
                tercero_potencial.OrigenID = model.OrigenID;
                if (model.OrigenID == 124)
                {
                    tercero_potencial.Otro = model.Otro;
                }
                else
                {
                    tercero_potencial.Otro = null;
                }

                try { tercero_potencial.Identificacion = model.Identificacion; }
                catch { }
                try { tercero_potencial.TipoIdentificacion = model.TipoIdentificacion; }
                catch { }
                tercero_potencial.RazonSocial = model.RazonSocial.ToUpper();
                tercero_potencial.TipoIdentificacion = model.TipoIdentificacion;
                tercero_potencial.Cliente = model.Cliente;
                tercero_potencial.Proveedor = model.Proveedor;
                tercero_potencial.Vendedor = model.Vendedor;
                tercero_potencial.ContactoID = model.ContactoID;

                if (tercero_potencial.RowID <= 0)
                    db.TerceroPotencial.Add(tercero_potencial);

                db.SaveChanges();
                switch (Request.Params["accion"])
                {
                    case "1":
                        return RedirectToAction("Cliente_Nuevo", "CRM", new { rowid = tercero_potencial.RowID, @rih = Request.Params["rih"] });

                    default:
                        return RedirectToAction("ListadoPotenciales", "CRM", new { @rih = Request.Params["rih"] });
                }
            }

            catch (Exception ex)
            {
                return RedirectToAction("Cliente_Nuevo", "CRM", new { rowid = model.RowID, @rih = Request.Params["rih"] });
                //return RedirectToAction("Cliente_Nuevo", "CRM", new { Msj = ex.Message });
            }
        }

        [CheckSessionOut]
        public ActionResult ListadoPotenciales()
        {
            return View(db.TerceroPotencial.ToList());
        }
        [CheckSessionOut]
        public ActionResult Cliente_Nuevo(int? rowid)
        {
            ViewBag.ListaEstado = db.Opcion.Where(f => f.Agrupacion.Nombre == "POTENCIAL.ESTADO").ToList();
            ViewBag.ListaOrigen = db.Opcion.Where(f => f.Agrupacion.Nombre == "POTENCIAL.ORIGEN" ).ToList();
            
            TerceroPotencial tercero = new TerceroPotencial();
            if (rowid > 0)
            {
                tercero = db.TerceroPotencial.Where(f => f.RowID == rowid).FirstOrDefault();
            }
            else
            {
                tercero.Contacto = new Contacto();
                tercero.Contacto.Ciudad = new Ciudad();
                tercero.Contacto.Ciudad.Departamento = new Departamento();
                tercero.Contacto.Ciudad.Departamento.Pais = new Pais();
                tercero.Opcion = new Opcion();
                tercero.Opcion1 = new Opcion();
            }
            ViewBag.Actividades = db.Actividad.Where(f => f.TerceroERPID==tercero.RowID).ToList();
            if (tercero.Contacto==null)
            {
                tercero.Contacto = new Contacto();
            }
            if (tercero.Contacto.Ciudad==null)
            {
                tercero.Contacto.Ciudad = new Ciudad();
                tercero.Contacto.Ciudad.Departamento = new Departamento();
                tercero.Contacto.Ciudad.Departamento.Pais = new Pais();
            }
            return View(tercero);
        }

        #endregion

        #region :::::CONTACTOS:::::
        [CheckSessionOut]
        public ActionResult ListadoContactos()
        {
            return View(db.Contacto.ToList());//Crear tabla de contactos
        }
        [CheckSessionOut]
        public ActionResult Contacto(int? rowid)
        {

            ViewBag.error = "";
            ViewBag.Sucursal = "";
            //ViewBag.cargos = _db.m_metaclasses.Where(f => f.class_code == "CONTACTO.CARGO").ToList();
            ViewBag.cargos = db.Opcion.Where(f => f.Agrupacion.Nombre == "CONTACTO.CARGO").ToList();

            //ViewBag.ListaPaises = _db.m_localizacions.Where(f => f.tipo_localizacion == "PAIS").ToList();


            //ViewBag.Actividades = _db.crm_actividades.Where(f => f.tipo_relacion == Constantes.ACTIVIDAD_TIPO_RELACION_contacto && f.rowid_relacion == rowid).ToList();
            ViewBag.Actividades = db.Actividad.Where(f => f.ContactoID == rowid).ToList();

            Contacto contacto = db.Contacto.Where(f => f.RowID == rowid).FirstOrDefault();

            if (contacto == null)
                contacto = new Contacto();
            else
            {
                //Sucursal Sucur = contacto;
                //if (Sucur != null)
                //    ViewBag.Sucursal = 0;
            }
            if (contacto.Ciudad==null)
            {
                contacto.Ciudad = new Ciudad();
                contacto.Ciudad.Departamento=new Departamento ();
                contacto.Ciudad.Departamento.Pais = new Pais();
            }
            return View(contacto);
        }
        [HttpPost]
        public JsonResult Contacto(FormCollection form)
        {
            form = DeSerialize(form);

            ViewBag.error = "";
            int rowid_contacto = 0;
            try { rowid_contacto = int.Parse(form["rowid"]); }
            catch { }

            try
            {
                Contacto contacto = db.Contacto.Where(f => f.RowID == rowid_contacto).FirstOrDefault();

                if (contacto == null)//insertar
                {
                    contacto = new Contacto();
                    contacto.UsuarioCreacion = ((Usuario)Session["curUser"]).NombreUsuario;
                    contacto.FechaCreacion = UtilTool.GetDateTime();
                }
                else//modificar
                {
                    contacto.UsuarioModificacion = ((Usuario)Session["curUser"]).NombreUsuario;
                    contacto.FechaModificacion = UtilTool.GetDateTime();

                }

                contacto.Nombre = form["nombres"];
                contacto.Apellido = form["apellidos"];
                contacto.Identificacion = form["identificacion"];
                try
                {
                    contacto.CiudadID = int.Parse(form["Ciudad"]);
                }
                catch (Exception)
                {
                }
                try { contacto.SucursalID = int.Parse(form["rowid_sucursal"]); }
                catch { contacto.SucursalID = null; }
                contacto.Telefono1 = form["telefono"];
                contacto.Area = form["area"];
                contacto.Descripcion = form["descripcion"];
                contacto.Skype = form["skype"];
                contacto.ruta_imagen = form["ruta_imagen"];
                contacto.Cargo = form["cargo"];
                contacto.Celular = form["celular"];
                contacto.Email = form["email"];

                if (rowid_contacto == 0)
                    db.Contacto.Add(contacto);

                db.SaveChanges();
                rowid_contacto = contacto.RowID;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = e.Message.ToString();
                return Json("Se presento un problema al momento de almacenar la información del contacto");
            }

            return Json(rowid_contacto);
        }
        #endregion

        #region :::::ACTIVIDADES:::::
        [CheckSessionOut]
        public ActionResult Modal_Referencia(string tipo_referencia)
        {
            int TIPO = int.Parse(tipo_referencia);
            Opcion data = db.Opcion.Where(f => f.RowID == TIPO).FirstOrDefault();

            ViewBag.titulo = "";
            ViewBag.columna1 = "";
            ViewBag.columna2 = "";
            ViewBag.columna3 = "";
            ViewBag.columna4 = "";
            ViewBag.columna5 = "";

            List<ModalReferencias> List = new List<ModalReferencias>();
            switch (data.Nombre)
            {
                //case Constantes.ACTIVIDAD_TIPO_RELACION_oportunidad://cliente erp
                //    ViewBag.titulo = "Oportunidades";
                //    ViewBag.columna1 = "Oportunidad";
                //    ViewBag.columna2 = "Vendedor";
                //    ViewBag.columna3 = "Fecha Solicitud";
                //    ViewBag.columna4 = "Estado";

                //    List = (from reg in db.t_cotizaciones.Where(f => f.ind_oportunidad == true && f.id_estado != Constantes.OPORTUNIDAD_Pedida).OrderByDescending(f => f.nombre).ToList()
                //            join usuario in db.s_usuarios.Where(f => f.erp_codigo != null && f.erp_codigo != "").ToList() on reg.id_vendedor equals usuario.rowid.ToString()
                //            select new ModalReferencias()
                //                {
                //                    rowid = reg.rowid,
                //                    valor = reg.nombre + " => " + usuario.nombre_completo,
                //                    columna1 = reg.nombre,
                //                    columna2 = usuario.nombre_completo,
                //                    columna3 = reg.fecha_solicitud.ToString(),
                //                    columna4 = reg.m_estado.nombre_estado,
                //                }).ToList();

                //    break;
                case Constantes.ACTIVIDAD_TIPO_RELACION_cliente://cliente erp
                    ViewBag.titulo = "Clientes ERP";
                    ViewBag.columna1 = "ID";
                    ViewBag.columna2 = "Razon Social";
                    ViewBag.columna3 = "contacto";
                    ViewBag.columna4 = "Telefono";

                    List = (from reg in db.Tercero.ToList()
                            select new ModalReferencias()
                            {
                                rowid = reg.RowID,
                                valor = reg.Identificacion + " - " + reg.RazonSocial,
                                columna1 = reg.Identificacion,
                                columna2 = reg.RazonSocial,
                                columna3 = reg.ContactoERP==null?"":reg.ContactoERP.Nombre,
                                columna4 = reg.ContactoERP == null ? "" : reg.ContactoERP.Telefono1,
                            }).ToList();

                    break;
                //case Constantes.ACTIVIDAD_TIPO_RELACION_potencial://cliente potencial                   

                //    ViewBag.titulo = "Clientes Potenciales";
                //    ViewBag.columna1 = "ID";
                //    ViewBag.columna2 = "Nombre";
                //    ViewBag.columna3 = "Tipo Relacion";
                //    ViewBag.columna4 = "Estado";

                //    List = (from reg in db.crm_terceros_potenciales.OrderByDescending(f => f.ind_activo).ToList()
                //            select new ModalReferencias()
                //            {
                //                rowid = reg.rowid,
                //                valor = reg.identificacion + " - " + reg.razonsocial,
                //                columna1 = reg.identificacion,
                //                columna2 = reg.razonsocial,
                //                columna3 = GetTipoRelacion(reg.es_cliente, reg.es_proveedor, reg.es_vendedor),
                //                columna4 = (reg.ind_activo == 1) ? "Activo" : "Inactivo",
                //            }).ToList();
                //    break;
                case Constantes.ACTIVIDAD_TIPO_RELACION_contacto://contacto                    
                    ViewBag.titulo = "Contactos";
                    ViewBag.columna1 = "ID";
                    ViewBag.columna2 = "Nombre";
                    ViewBag.columna3 = "Sucursal";
                    ViewBag.columna4 = "Cargo";

                    List = (from reg in db.Contacto.ToList()
                            select new ModalReferencias()
                            {
                                rowid = reg.RowID,
                                valor = reg.Identificacion + " - " + reg.Nombre + " " + reg.Apellido,
                                columna1 = reg.Identificacion,
                                columna2 = reg.Nombre + " " + reg.Apellido,
                                columna3 = "",
                                columna4 = reg.Cargo,
                            }).ToList();
                    break;
            }


            return View(List.ToList());
        }
        public string GetTipoRelacion(int? cliente, int? proveedor, int? vendedor)
        {
            string relacion = "";
            if (cliente == 1)
                relacion += "|Cliente| ";
            if (proveedor == 1)
                relacion += "|Proveedor| ";
            if (vendedor == 1)
                relacion += "|Vendedor| ";

            return relacion;
        }
        [CheckSessionOut]
        public ActionResult Actividad(int? rowid)
        {
            ViewBag.error = "";
            ViewBag.fecha_final = "";
            ViewBag.fecha_inicial = "";
            // Actividad actividad = db.Actividad.Where(f => f.RowID== rowid).FirstOrDefault();


            Actividad actividad = db.Actividad.Where(f => f.RowID == rowid).OrderByDescending(f => f.FechaCreacion).FirstOrDefault();
            if (actividad == null)
                actividad = new Actividad();
            else
            {
                ViewBag.fecha_final = formatFecha(actividad.FechaFinal);
                ViewBag.fecha_inicial = formatFecha(actividad.FechaInicial);
            }


            if (actividad.TerceroERPID == null || actividad.TerceroERPID == 0)
            {
                actividad.Tercero = new Models.Tercero();
            }
            else
            {
                actividad.Tercero = db.Tercero.Where(f => f.RowID == actividad.TerceroERPID).FirstOrDefault();
            }
            if (actividad.ContactoID == null || actividad.ContactoID == 0)
            {
                actividad.Contacto = new Models.Contacto();
            }
            else
            {
                actividad.Contacto = db.Contacto.Where(f => f.RowID == actividad.ContactoID).FirstOrDefault();
            }




            ViewBag.Estados = db.Opcion.Where(F => F.Agrupacion.Nombre == "ESTADOS.ACTIVIDAD").ToList();
            ViewBag.Prioridades = db.Opcion.Where(F => F.Agrupacion.Nombre == "ACTIVIDAD.PRIORIDAD").ToList();

            ViewBag.tipos = db.Opcion.Where(F => F.Agrupacion.Nombre == "ACTIVIDAD.TIPO").ToList();
            ViewBag.tiposRelacion = db.Opcion.Where(F => F.Agrupacion.Nombre == "ACTIVIDAD.TIPO.RELACION").ToList();

            return View(actividad);
        }
        public string formatFecha(DateTime? fecha)
        {
            return (DateTime.Parse(fecha.ToString()).ToString("yyyy-MM-dd"));
        }

        public ActionResult Activdad_Calendario()
        {
            return View();
        }
        [CheckSessionOut]
        public ActionResult ListadoActividades()
        {
            return View(db.Actividad.ToList().OrderByDescending(f => f.RowID).ToList());
        }
        public JsonResult GetEvents()
        {

            //LISTADO DE EVENTOS PARA CARGAR EL CALENDARIO
            List<fullCalendar> calendario = (from evento in (db.Actividad.ToList().OrderByDescending(f => f.FechaCreacion).ToList())
                                             select new fullCalendar()
                                             {
                                                 title = evento.UsuarioCreacion.Trim() + ": " + evento.Tema.Trim() + " " + evento.Descripcion,
                                                 start = evento.FechaInicial.ToString("s"),
                                                 end = evento.FechaFinal.ToString("s"),
                                                 backgroundColor = ColorEvento(evento.TerceroERPID.ToString(), evento.ContactoID.ToString()),
                                                 borderColor = ColorEvento(evento.TerceroERPID.ToString(), evento.ContactoID.ToString())
                                             }).ToList();

            var rows = calendario.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public string ColorEvento(string tercero, string contacto)
        {
            if (tercero != 0.ToString())
            {
                return "#f39c12";//AMARILLO
            }
            else if (contacto != 0.ToString())
            {
                return "#0073b7";//AZUL
            }
            else
            {
                return "#0073b7";
                //    //   break;
            }


            //switch (tipo_evento)
            //{
            //    case "Cliente":
            //        return "#f39c12";//AMARILLO
            //    // break;
            //    case "Prospecto":
            //        return "#f56954";//ROJO
            //    //  break;
            //    case "Contacto":
            //        return "#0073b7";//AZUL
            //    // break;
            //    case "Pedido":
            //        return "#00a65a";//VERDE
            //    //  break;
            //    case "PQRS":
            //        return "#00c0ef";//AQUA
            //    //  break;
            //    default:
            //        return "#0073b7";
            //    //   break;
            //}
        }
        [HttpPost]
        public JsonResult Actividad(FormCollection FormData)
        {
            ViewBag.error = "";
            int rowid_actividad = 0;

            try
            {
                FormCollection form = DeSerialize(FormData);
                try
                {
                    rowid_actividad = int.Parse(form["rowid_actividad"]);
                }
                catch { }

                Usuario usuario = ((Usuario)Session["curUser"]);

                Actividad actividad = db.Actividad.Where(f => f.RowID == rowid_actividad).FirstOrDefault();

                if (actividad == null)//insertar
                {
                    actividad = new Actividad();
                    actividad.UsuarioCreacion = usuario.NombreUsuario;
                    actividad.FechaCreacion = UtilTool.GetDateTime();
                }
                else//modificar
                {
                    actividad.UsuarioModificacion = usuario.NombreUsuario;
                    actividad.FechaModificacion = UtilTool.GetDateTime();
                }

                actividad.EstadoID = int.Parse(form["rowid_estado"]);
                actividad.Tema = form["tema"];
                actividad.PrioridadID = int.Parse(form["ind_prioridad"]);
                actividad.FechaInicial = DateTime.Parse(form["fecha_inicial"]);
                actividad.FechaFinal = DateTime.Parse(form["fecha_final"]);
                actividad.TipoID = int.Parse(form["tipo"]);
                //actividad.TipoRelacionID = int.Parse(form["tipo_relacion"]);
                try
                {
                    int a = int.Parse(form["tipo_relacion"]);
                    Opcion data = db.Opcion.Where(f => f.RowID ==a).FirstOrDefault();
                    actividad.TipoRelacionID = data.RowID;
                    if (data.Nombre == "CLIENTE")
                    {
                        actividad.TerceroERPID = int.Parse(form["rowid_relacion"]);
                    }
                    else if (data.Nombre == " CONTACTO")
                    {
                        actividad.ContactoID = int.Parse(form["rowid_relacion"]);
                    }
                }
                catch { }

                actividad.Descripcion = form["descripcion"];


                if (rowid_actividad == 0)
                    db.Actividad.Add(actividad);


                db.SaveChanges();

                return Json(actividad.RowID);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = e.Message.ToString();
                return Json("Se presento un problema al momento de almacenar los datos, intente nuevamente");
            }
        }

        #endregion
    }
}