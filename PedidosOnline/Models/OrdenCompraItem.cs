
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
    
public partial class OrdenCompraItem
{

    public int RowID { get; set; }

    public Nullable<int> OrdenCompraID { get; set; }

    public Nullable<int> ItemID { get; set; }

    public Nullable<double> Cantidad { get; set; }

    public Nullable<double> ValorUnitario { get; set; }

    public string UnidadEmpaque { get; set; }

    public Nullable<double> ValorImpuesto { get; set; }

    public Nullable<double> ValorDescuento { get; set; }

    public Nullable<double> ValorBase { get; set; }

    public Nullable<double> ValorTotal { get; set; }

    public string UsuarioCreacion { get; set; }

    public Nullable<System.DateTime> FechaCreacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }

    public Nullable<double> Retencion { get; set; }



    public virtual Item Item { get; set; }

    public virtual Item Item1 { get; set; }

    public virtual OrdenCompra OrdenCompra { get; set; }

    public virtual OrdenCompra OrdenCompra1 { get; set; }

}

}
