
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
    
public partial class Usuario
{

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

    public virtual Rol Rol { get; set; }

}

}
