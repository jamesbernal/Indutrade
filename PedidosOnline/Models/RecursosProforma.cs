
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
    
public partial class RecursosProforma
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public RecursosProforma()
    {

        this.RecursosContrato = new HashSet<RecursosContrato>();

    }


    public int RowID { get; set; }

    public string Codigo { get; set; }

    public string Archivo { get; set; }

    public int ProformaID { get; set; }



    public virtual Proforma Proforma { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RecursosContrato> RecursosContrato { get; set; }

}

}