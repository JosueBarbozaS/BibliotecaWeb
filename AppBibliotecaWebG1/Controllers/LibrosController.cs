using Microsoft.AspNetCore.Mvc;
//Se referencia el  ORM
using AppBibliotecaWebG1.Models;
using Microsoft.EntityFrameworkCore;

namespace AppBibliotecaWebG1.Controllers
{
    public class LibrosController : Controller
    {
        //Variable que permite manejar la referencia del contexto
        private readonly DbContextBiblioteca _context = null;

        /// <summary>
        /// Constructor con parámetros recibe la referencia del ORM
        /// </summary>
        /// <param name="context"></param>
        public LibrosController(DbContextBiblioteca context)
        {
            _context = context; //se asigna la referencia del contexto 
        }


        public async Task<IActionResult> Index()
        {
            //se declara la variable lista
            //por medio del ORM se lee la información de todos los libros en la db
            var listado = await _context.Libros.ToListAsync();

            return View(listado);//Se envia el listado al front-end
        }

        //Métodos encargado para almacenar un libro
        [HttpGet]
        public IActionResult Create() //Método encargado de mostrar el front-end para crear un libro
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files,[Bind]Libro libro)
        {
            if(libro == null) //se valida que el objeto libro tenga datos
            {
                return View(); //Como no hay datos dejamos al usuario dentro del formulario create
            }
            else
            {
                //se asigna cero para generar el ISBN automatico
                libro.ISBN = 0;
                
                //se pregunta si tiene la foto
                if(files.Count >0 )
                {
                    //lugar donde se almacena la foto
                    string filePath = @"wwwroot\css\img\";

                    //variable para construir el nombre de la foto
                    string fileName = "";

                    //se leen las fotos adjuntas al formulario
                    foreach (var formFile in files)
                    {
                        //se valida el tamaño para la foto
                        if (formFile.Length > 0 )
                        {
                            //se construye le nombre de la foto
                            fileName = libro.Titulo + "_" + formFile.FileName;

                            //se quitan los espacios en blanco dentro del nombre de la foto
                            fileName = fileName.Replace(" ", "_");
                            
                            fileName = fileName.Replace("#", "_");


                            //se indica la ruta fisica donde se almacena la foto
                            filePath += fileName;

                            //se copia  la foto dentro de la carpeta
                            using(var stream = new FileStream(filePath, FileMode.Create))
                            {
                                //se copia la foto
                                await formFile.CopyToAsync(stream);

                                //ahora se indica a nuestra base de datos donde se ubica la foto
                                libro.Foto = @"/css/img/" + fileName;
                            }//cierre using
                        }//cierre if
                    }//cierre foreach
                }
                else
                {
                    libro.Foto = "ND";
                }


                //si hay datos almacenamos el libro
                _context.Libros.Add(libro);

                //se aplican los cambios en la db
                await _context.SaveChangesAsync();

                //ubicamos al usuario dentro del listado libros
                return RedirectToAction("Index");
                
            }
        }


        //Métodos para el proceso de eliminar un libro
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //buscar e libro  a eliminar por medio del  ORM
            var temp  = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);

            //se envian los datos del libro al front-end
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Delete(int? id)
        {
            //se busca por el ISBN el libro  a eliminar
            var temp = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);

            if(temp != null) //se valida si tiene datos
            {
                _context.Libros.Remove(temp); //se elimina el libro
                await _context.SaveChangesAsync();  //se aplican los cambios
                return RedirectToAction("Index");//se muestra la lista de libros
            }
            else
            {
                return NotFound(); //Error 404 recurso no disponible
            }
        }


        //Método encargado de mostrar los datos para un libro especifico
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //se busca por ISBN el libro a mostrar
            var temp = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);

            //se envian los datos al front-end
            return View(temp);
        }


        //Métodos encargados de realizar el proceso de editar los datos a un libro
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FirstOrDefaultAsync(r => r.ISBN == id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Titulo,Editorial,PrecioVenta,Foto,FechaPublicacion,Estado")] Libro libroActualizado, List<IFormFile> files)
        {
            var libroExistente = await _context.Libros.FindAsync(id);

            if (libroExistente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mantener el ISBN original
                    libroExistente.Titulo = libroActualizado.Titulo;
                    libroExistente.Editorial = libroActualizado.Editorial;
                    libroExistente.PrecioVenta = libroActualizado.PrecioVenta;
                    libroExistente.FechaPublicacion = libroActualizado.FechaPublicacion;
                    libroExistente.Estado = libroActualizado.Estado;

                    // Handle file upload if new file is provided
                    if (files != null && files.Count > 0)
                    {
                        string filePath = @"wwwroot\css\img\";
                        foreach (var formFile in files)
                        {
                            if (formFile.Length > 0)
                            {
                                string fileName = libroActualizado.Titulo + "_" + formFile.FileName;
                                fileName = fileName.Replace(" ", "_").Replace("#", "_");
                                string fullPath = Path.Combine(filePath, fileName);

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    await formFile.CopyToAsync(stream);
                                    libroExistente.Foto = @"/css/img/" + fileName;
                                }
                            }
                        }
                    }

                    _context.Update(libroExistente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(libroExistente);
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.ISBN == id);
        }



    } //cierre controller
} //cierre namepaces
