
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
    
public partial class AutorizacionCargue
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AutorizacionCargue()
    {

        this.AutorizacionCargueVehiculo = new HashSet<AutorizacionCargueVehiculo>();

    }


    public int RowID { get; set; }

    public Nullable<int> SolicitudTransporteID { get; set; }

    public Nullable<int> EstadoID { get; set; }

    public string UsuarioCreacion { get; set; }

    public Nullable<System.DateTime> FechaCreacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }

    public string BKK { get; set; }

    public string Empaque { get; set; }



    public virtual Estado Estado { get; set; }

    public virtual SolicitudTransporte SolicitudTransporte { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AutorizacionCargueVehiculo> AutorizacionCargueVehiculo { get; set; }

}

}
