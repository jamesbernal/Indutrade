
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
    
public partial class Costo
{

    public int RowID { get; set; }

    public string Nombre { get; set; }

    public Nullable<int> MonedaID { get; set; }

    public Nullable<int> TipoID { get; set; }

    public Nullable<double> CostoContenedor { get; set; }

    public Nullable<double> CostoTonelada { get; set; }

    public string UsuarioCreacion { get; set; }

    public Nullable<System.DateTime> FechaCreacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }



    public virtual Opcion Opcion { get; set; }

    public virtual Opcion Opcion1 { get; set; }

}

}
