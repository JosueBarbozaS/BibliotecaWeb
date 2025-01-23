using System.ComponentModel.DataAnnotations;

namespace AppBibliotecaWebG1.Models
{
    public class SeguridadRestablecer
    {
        public string Email { get; set; }

        [Required(ErrorMessage ="Debe ingresar la clave temporal enviada por email")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Debe ingresar el nuevo password")]
        [DataType (DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="Es importante la confirmación del nuevo password")]
        [DataType (DataType.Password)]  
        public string Confirmar { get; set; }
    }
}
