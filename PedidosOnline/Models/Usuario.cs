
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
    
public partial class Usuario
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Usuario()
    {

        this.Contrato = new HashSet<Contrato>();

        this.Contrato1 = new HashSet<Contrato>();

        this.Proforma = new HashSet<Proforma>();

        this.Proforma1 = new HashSet<Proforma>();

        this.ProformaItemCalculadora = new HashSet<ProformaItemCalculadora>();

        this.ProformaItemCalculadora1 = new HashSet<ProformaItemCalculadora>();

    }


    public int RowID { get; set; }

    public string NombreUsuario { get; set; }

    public string Contraseña { get; set; }

    public Nullable<int> TerceroID { get; set; }

    public Nullable<int> RolID { get; set; }

    public Nullable<bool> Activo { get; set; }

    public string UsuarioCreacion { get; set; }

    public Nullable<System.DateTime> FechaCreacion { get; set; }

    public string UsuarioActualizacion { get; set; }

    public Nullable<System.DateTime> FechaActualizacion { get; set; }



    public virtual Tercero Tercero { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Contrato> Contrato { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Contrato> Contrato1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Proforma> Proforma { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Proforma> Proforma1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ProformaItemCalculadora> ProformaItemCalculadora { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ProformaItemCalculadora> ProformaItemCalculadora1 { get; set; }

    public virtual Rol Rol { get; set; }

}

}
