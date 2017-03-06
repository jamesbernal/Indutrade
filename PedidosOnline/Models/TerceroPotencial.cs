
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace PedidosOnline.Models
{

using System;
    using System.Collections.Generic;
    
public partial class TerceroPotencial
{

    public int RowID { get; set; }

    public string Identificacion { get; set; }

    public string TipoIdentificacion { get; set; }

    public string RazonSocial { get; set; }

    public string NombreComercial { get; set; }

    public Nullable<short> Vendedor { get; set; }

    public Nullable<short> Cliente { get; set; }

    public Nullable<short> Proveedor { get; set; }

    public Nullable<short> Accionista { get; set; }

    public Nullable<int> ContactoID { get; set; }

    public Nullable<int> OrigenID { get; set; }

    public string Otro { get; set; }

    public string Descripcion { get; set; }

    public Nullable<int> EstadoID { get; set; }

    public Nullable<bool> Activo { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public string UsuarioCreacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }

    public string UsuarioModificacion { get; set; }



    public virtual Contacto Contacto { get; set; }

    public virtual Opcion Opcion { get; set; }

    public virtual Opcion Opcion1 { get; set; }

}

}
