
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
    using System.Collections.Generic;
    
public partial class PuntoEnvio
{

    public int RowID { get; set; }

    public Nullable<int> RowIDERP { get; set; }

    public Nullable<int> SucursalID { get; set; }

    public Nullable<int> VendedorID { get; set; }

    public string Codigo { get; set; }

    public string Nombre { get; set; }

    public Nullable<bool> Activo { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public string UsuarioCreacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }

    public string UsuarioModificacion { get; set; }



    public virtual Sucursal Sucursal { get; set; }

    public virtual Tercero Tercero { get; set; }

}

}
