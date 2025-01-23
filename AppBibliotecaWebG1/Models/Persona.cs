namespace AppBibliotecaWebG1.Models
{
    public class Persona
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }  // Este es el campo "fullname"
        public string PrimerNombre { get; set; }  // Este es el campo "firstname1"
        public string SegundoNombre { get; set; }  // Este es el campo "firstname2"
        public string PrimerApellido { get; set; }  // Este es el campo "lastname1"
        public string SegundoApellido { get; set; }  // Este es el campo "lastname2"

        public int Edad {  get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}
