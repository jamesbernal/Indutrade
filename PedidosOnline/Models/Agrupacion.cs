
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
    
public partial class Agrupacion
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Agrupacion()
    {

        this.Opcion = new HashSet<Opcion>();

    }


    public int RowID { get; set; }

    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public bool Activo { get; set; }

    public string UsuarioCreacion { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Opcion> Opcion { get; set; }

}

}
