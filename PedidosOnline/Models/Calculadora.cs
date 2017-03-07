
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
    
public partial class Calculadora
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Calculadora()
    {

        this.CalculadoraItems = new HashSet<CalculadoraItems>();

        this.Contrato = new HashSet<Contrato>();

    }


    public int RowID { get; set; }

    public Nullable<int> TerceroID { get; set; }

    public Nullable<int> AgenteNavieraID { get; set; }

    public Nullable<int> DestinoID { get; set; }

    public System.DateTime Fecha { get; set; }

    public string observacion { get; set; }

    public Nullable<int> IncotermID { get; set; }

    public string UsuarioCreacion { get; set; }

    public System.DateTime FechaCreacion { get; set; }

    public string UsuarioModificacion { get; set; }

    public Nullable<System.DateTime> FechaModificacion { get; set; }

    public bool Terminada { get; set; }

    public Nullable<int> BrokerID { get; set; }



    public virtual AgenteNaviera AgenteNaviera { get; set; }

    public virtual AgenteNaviera AgenteNaviera1 { get; set; }

    public virtual Ciudad Ciudad { get; set; }

    public virtual Tercero Tercero { get; set; }

    public virtual Tercero Tercero1 { get; set; }

    public virtual Tercero Tercero2 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CalculadoraItems> CalculadoraItems { get; set; }

    public virtual Opcion Opcion { get; set; }

    public virtual Opcion Opcion1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Contrato> Contrato { get; set; }

}

}
