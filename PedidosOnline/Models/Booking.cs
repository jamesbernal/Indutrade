
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
    
public partial class Booking
{

    public int RowID { get; set; }

    public Nullable<int> ProformaID { get; set; }

    public Nullable<int> TipoLlenadoID { get; set; }

    public string Nota { get; set; }



    public virtual Proforma Proforma { get; set; }

    public virtual Opcion Opcion { get; set; }

}

}
