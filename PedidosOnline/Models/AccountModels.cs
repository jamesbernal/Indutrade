using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PedidosOnline.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]

        public string Password { get; set; }

        //  [Required]
        //  [Display(Name = "Compañia")]
        //  public int Compania { get; set; }

        [Required]
        [Display(Name = "EsMobil")]
        public int EsMobil { get; set; }

        [Display(Name = "Mantener la sesión iniciada")]
        public bool RememberMe { get; set; }
        public bool p { get; set; }

    }

    //public static class DatosCliente
    //{
    //    public static List<Menu> MenuUsuario = new List<Menu>();
    //    public static List<Modulo> ModulosMenu = new List<Modulo>();
    //    public static Usuario UsuarioLogeado = new Usuario();
    //}
}
