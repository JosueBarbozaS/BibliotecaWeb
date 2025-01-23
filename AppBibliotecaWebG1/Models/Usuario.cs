using System.ComponentModel.DataAnnotations;

namespace AppBibliotecaWebG1.Models
{
    public class Usuario
    {
        [Key]
        [Required(ErrorMessage ="No se permite el email en blanco")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Debe ingresar el nombre completo")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage ="No se permite la contraseña en blanco")]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Password { get; set; }

        public char Restablecer { get; set; }

        public DateTime FechaRegistro { get; set; }

        public char Estado { get; set; }
    }
}
