using AppBibliotecaWebG1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppBibliotecaWebG1.Controllers
{
    public class PersonaController : Controller
    {
        private  GoMetaService _personaService;

        public PersonaController()
        {
            _personaService = new GoMetaService();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            // Vista vacía para ingresar la cédula
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                ModelState.AddModelError("", "Por favor, ingrese un número de cédula.");
                return View();
            }

            // Llamar al método ExtraerPersona para obtener los datos
            var persona = await ExtraerPersona(cedula);

            if (persona != null)
            {
                // Pasamos los datos de la persona a la vista
                return View(persona);
            }
            else
            {
                ModelState.AddModelError("", "No se encontró la persona con la cédula proporcionada.");
                return View();
            }
        }


        [HttpGet]
        public async Task<Persona> ExtraerPersona(string cedula)
        {
            try
            {
                // Se obtiene el cliente HTTP desde el servicio
                var client = _personaService.Iniciar();

                // Se realiza la solicitud a la API
                var response = await client.GetAsync($"cedulas/{cedula}");

                if (response.IsSuccessStatusCode)
                {
                    // Se lee la respuesta obtenida
                    var result = await response.Content.ReadAsStringAsync();

                    // Deserializar solo la raíz del JSON
                    var jsonResult = JsonConvert.DeserializeObject<dynamic>(result);

                    // Verificamos que el array "results" contenga al menos un elemento
                    if (jsonResult.results.Count > 0)
                    {
                        var personaData = jsonResult.results[0];

                        // Mapeamos los valores del JSON al modelo Persona
                        var persona = new Persona
                        {
                            Cedula = personaData.cedula,
                            Nombre = personaData.fullname,
                            PrimerNombre = personaData.firstname1,
                            SegundoNombre = personaData.firstname2,
                            PrimerApellido = personaData.lastname1,
                            SegundoApellido = personaData.lastname2
                        };

                        return persona;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
