
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
    
public partial class FEP
{

    public int RowID { get; set; }

    public Nullable<int> DCDID { get; set; }

    public string Dex { get; set; }

    public string NCertificadoCP { get; set; }



    public virtual DCD DCD { get; set; }

}

}
