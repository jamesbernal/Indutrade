﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace PedidosOnline.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;
using System.Linq;


public partial class PedidosOnlineEntities : DbContext
{
    public PedidosOnlineEntities()
        : base("name=PedidosOnlineEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Actividad> Actividad { get; set; }

    public virtual DbSet<Contacto> Contacto { get; set; }

    public virtual DbSet<TerceroPotencial> TerceroPotencial { get; set; }

    public virtual DbSet<AgenteNaviera> AgenteNaviera { get; set; }

    public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

    public virtual DbSet<Ciudad> Ciudad { get; set; }

    public virtual DbSet<Compañia> Compañia { get; set; }

    public virtual DbSet<CondicionPago> CondicionPago { get; set; }

    public virtual DbSet<ContactoERP> ContactoERP { get; set; }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<Empresa> Empresa { get; set; }

    public virtual DbSet<Item> Item { get; set; }

    public virtual DbSet<ListaPrecio> ListaPrecio { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<Precio> Precio { get; set; }

    public virtual DbSet<Sucursal> Sucursal { get; set; }

    public virtual DbSet<Tercero> Tercero { get; set; }

    public virtual DbSet<Booking> Booking { get; set; }

    public virtual DbSet<Calculadora> Calculadora { get; set; }

    public virtual DbSet<CalculadoraItems> CalculadoraItems { get; set; }

    public virtual DbSet<Contrato> Contrato { get; set; }

    public virtual DbSet<ContratoAdjunto> ContratoAdjunto { get; set; }

    public virtual DbSet<OrdenCompra> OrdenCompra { get; set; }

    public virtual DbSet<OrdenCompraContrato> OrdenCompraContrato { get; set; }

    public virtual DbSet<OrdenCompraItem> OrdenCompraItem { get; set; }

    public virtual DbSet<Proforma> Proforma { get; set; }

    public virtual DbSet<ProformaItemCalculadora> ProformaItemCalculadora { get; set; }

    public virtual DbSet<Agrupacion> Agrupacion { get; set; }

    public virtual DbSet<Costo> Costo { get; set; }

    public virtual DbSet<MotoNave> MotoNave { get; set; }

    public virtual DbSet<Opcion> Opcion { get; set; }

    public virtual DbSet<Planta> Planta { get; set; }

    public virtual DbSet<Puerto> Puerto { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<Modulo> Modulo { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<RolMenu> RolMenu { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<m_plantillas> m_plantillas { get; set; }

    public virtual DbSet<PuntoEnvio> PuntoEnvio { get; set; }

    public virtual DbSet<ItemImpuesto> ItemImpuesto { get; set; }

    public virtual DbSet<AutorizacionCargue> AutorizacionCargue { get; set; }

    public virtual DbSet<AutorizacionCargueVehiculo> AutorizacionCargueVehiculo { get; set; }

    public virtual DbSet<SolicitudTransporte> SolicitudTransporte { get; set; }

    public virtual DbSet<TipoVehiculo> TipoVehiculo { get; set; }

    public virtual DbSet<EnvioMail> EnvioMail { get; set; }

    public virtual DbSet<Estado> Estado { get; set; }

    public virtual DbSet<Firma> Firma { get; set; }

    public virtual DbSet<Parametro> Parametro { get; set; }

    public virtual DbSet<Vehiculo> Vehiculo { get; set; }

    public virtual DbSet<DetalleRegistroLlenado> DetalleRegistroLlenado { get; set; }

    public virtual DbSet<EncabezadoRegistroLlenado> EncabezadoRegistroLlenado { get; set; }


    public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        var versionParameter = version.HasValue ?
            new ObjectParameter("version", version) :
            new ObjectParameter("version", typeof(int));


        var definitionParameter = definition != null ?
            new ObjectParameter("definition", definition) :
            new ObjectParameter("definition", typeof(byte[]));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
    }


    public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        var versionParameter = version.HasValue ?
            new ObjectParameter("version", version) :
            new ObjectParameter("version", typeof(int));


        var definitionParameter = definition != null ?
            new ObjectParameter("definition", definition) :
            new ObjectParameter("definition", typeof(byte[]));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
    }


    public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
    }


    public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
    }


    public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
    }


    public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
    {

        var diagramnameParameter = diagramname != null ?
            new ObjectParameter("diagramname", diagramname) :
            new ObjectParameter("diagramname", typeof(string));


        var owner_idParameter = owner_id.HasValue ?
            new ObjectParameter("owner_id", owner_id) :
            new ObjectParameter("owner_id", typeof(int));


        var new_diagramnameParameter = new_diagramname != null ?
            new ObjectParameter("new_diagramname", new_diagramname) :
            new ObjectParameter("new_diagramname", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
    }


    public virtual int sp_upgraddiagrams()
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
    }


    public virtual ObjectResult<spPROFORMA_EXPORTAR_PDF_Result> spPROFORMA_EXPORTAR_PDF(Nullable<int> rowid_proforma)
    {

        var rowid_proformaParameter = rowid_proforma.HasValue ?
            new ObjectParameter("rowid_proforma", rowid_proforma) :
            new ObjectParameter("rowid_proforma", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spPROFORMA_EXPORTAR_PDF_Result>("spPROFORMA_EXPORTAR_PDF", rowid_proformaParameter);
    }


    public virtual int contratos(Nullable<int> rowid_contrato)
    {

        var rowid_contratoParameter = rowid_contrato.HasValue ?
            new ObjectParameter("rowid_contrato", rowid_contrato) :
            new ObjectParameter("rowid_contrato", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("contratos", rowid_contratoParameter);
    }


    public virtual ObjectResult<spPROFORMA_EXPORTAR_PDF1_Result> spPROFORMA_EXPORTAR_PDF1(Nullable<int> rowid_proforma)
    {

        var rowid_proformaParameter = rowid_proforma.HasValue ?
            new ObjectParameter("rowid_proforma", rowid_proforma) :
            new ObjectParameter("rowid_proforma", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spPROFORMA_EXPORTAR_PDF1_Result>("spPROFORMA_EXPORTAR_PDF1", rowid_proformaParameter);
    }


    public virtual ObjectResult<spSOLICITUD_TRASNPORTE_EXPORTAR_PDF_Result> spSOLICITUD_TRASNPORTE_EXPORTAR_PDF()
    {

        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSOLICITUD_TRASNPORTE_EXPORTAR_PDF_Result>("spSOLICITUD_TRASNPORTE_EXPORTAR_PDF");
    }


    public virtual ObjectResult<ProformaExport_Result> ProformaExport(Nullable<int> rowID)
    {

        var rowIDParameter = rowID.HasValue ?
            new ObjectParameter("RowID", rowID) :
            new ObjectParameter("RowID", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ProformaExport_Result>("ProformaExport", rowIDParameter);
    }


    public virtual ObjectResult<SolicitudTransportePDF_Result> SolicitudTransportePDF(Nullable<int> rowID)
    {

        var rowIDParameter = rowID.HasValue ?
            new ObjectParameter("RowID", rowID) :
            new ObjectParameter("RowID", typeof(int));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SolicitudTransportePDF_Result>("SolicitudTransportePDF", rowIDParameter);
    }

}

}

