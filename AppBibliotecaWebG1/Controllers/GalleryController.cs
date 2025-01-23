using AppBibliotecaWebG1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace AppBibliotecaWebG1.Controllers
{
    public class GalleryController : Controller
    {
        //Variable permite manejar la referencia del ORM
        private readonly DbContextBiblioteca _context = null;

        /// <summary>
        /// Construtor recibe la instancia del ORM
        /// </summary>
        /// <param name="pContext"></param>
        public GalleryController(DbContextBiblioteca pContext)
        {
                _context = pContext; //se asigna la referencia
        }

        [HttpGet]
        public IActionResult Gallery()
        {
            //se toma la lista de libros
            var listado = _context.Libros.ToList();

            //se envia la lista de libros al front-end
            return View(listado);
        }

    }
}
