using Microsoft.AspNetCore.Mvc;
using AppBibliotecaWebG1.Models;
using Microsoft.EntityFrameworkCore;

//librerias necesario para el proceso de autenticación
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace AppBibliotecaWebG1.Controllers
{
    public class UsuariosController : Controller
    {
        //variable para manejar la referencia del ORM
        private readonly DbContextBiblioteca _context = null;

        //variable para controlar el email a restablecer
        private static string EmailRestablecer = "";


        //Constructor con parámetros
        public UsuariosController(DbContextBiblioteca pContext)
        {
            _context = pContext;    //se asigna la referencia del  ORM
        }

        /// <summary>
        /// Método encargado de mostrar el formulario de autenticación
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind]Usuario user)
        {
            //se utiliza el método para validar los datos del usuario
            var temp = ValidarUsuario(user);

            //se verifica si hay datos
            if (temp != null)
            {
                bool restablecer = false; //variable control

                restablecer = VerificarRestablecer(temp);//se verifica si necesita restablecer

                //se verifica si debe realizar el proceso de restablecer
                if (restablecer)
                {
                    //se ubica al usuario dentro del formulario para restablecer
                    return RedirectToAction("Restablecer", "Usuarios", new { Email = temp.Email });
                }
                else  //ya realizó anteriormente el proceso de restablecer
                {
                    //se proceso de autenticación

                    //se crea la instancia para la entidad del usuario y el tipo de authenticación
                    var userClaim = new List<Claim>() { new Claim(ClaimTypes.Name, temp.Email) };
                    
                    //se crea el tipo de entidad
                    var grandIdentity = new ClaimsIdentity(userClaim,"User Identity");

                    //se instancia la entidad principal
                    var userPrincipal = new ClaimsPrincipal(new[] { grandIdentity });

                    //Se realiza la autenticación dentro del contexto Http se envia la entidad como parámetro
                    await HttpContext.SignInAsync(userPrincipal);

                    //se ubica al usuario en la página de inicio
                    return RedirectToAction("Index", "Home");
                }
            }
            else //si no hay datos
            {
                TempData["Mensaje"] = "Error el usuario o contraseña son incorrectos..";
                return View(temp);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Restablecer(string? Email)
        {
            //se toman los datos del usuario que debe restablecer la contraseña
            var temp = await _context.Usuarios.FirstOrDefaultAsync( u => u.Email == Email);

            //Se instancia el objeto restablecer
            SeguridadRestablecer restablecer = new SeguridadRestablecer();

            //se rellena los datos del usuario a restablecer
            restablecer.Email = temp.Email;
            restablecer.Password = temp.Password;

            //Se almacena el email del usuario a restablecer
            EmailRestablecer = temp.Email;

            //se envian los datos del usuario a restablecer el password
            return View(restablecer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restablecer([Bind]SeguridadRestablecer pRestablecer)
        {
            //se valida si hay datos
            if (pRestablecer != null)
            {
                //se identifica al usuario que vamos a restablecer la contraseña
                var temp = await _context.Usuarios.FirstOrDefaultAsync(
                    u => u.Email.Equals(EmailRestablecer));

                if (temp != null)
                {
                    //se realiza el proceso de verificación del password
                    if (temp.Password.Equals(pRestablecer.Password))
                    {
                        //se verifica la confirmación del nuevo password
                        if (pRestablecer.NewPassword.Equals(pRestablecer.Confirmar))
                        {
                            //se realiza el proceso de restablecer
                            //se asigna la nueva clave
                            temp.Password = pRestablecer.Confirmar;
                            
                            //se indica que ya realizó el proceso de restablecer
                            temp.Restablecer = 'N';

                            //se actualiza los datos
                            _context.Usuarios.Update(temp);

                            //se aplican los cambios
                            await _context.SaveChangesAsync();

                            //se ubica al usuario dentro del proceso login
                            return RedirectToAction("Login", "Usuarios");

                        }
                        else
                        {
                            //se muestra el siguiente mensaje de error
                            TempData["Mensaje"] = "La confirmación de la contraseña no es correcta..";

                            //se ubica al usuari dentro del formulario restablecer para corregir el error
                            return View(pRestablecer);
                        }


                    }
                    else
                    {
                        //se muestra el mensaje de error
                        TempData["Mensaje"] = "El password es incorrecto..";

                        //se muestra el formulari restablecer para corregir el error
                        return View(pRestablecer);  
                    }
                }
                else
                {
                    //se muestra el siguiente mensaje de error
                    TempData["Mensaje"] = "No existe el usuario a restablecer la contraseña...";

                    //se ubica al usuario dentro del formulario de restablecer
                    return View(pRestablecer);
                }
            }
            else
            {
                //se muestra el mensaje de error mediante un alert
                TempData["Mensaje"] = "Datos incorrectos...";

                //se ubica al usuario dentro del formulario restablecer
                return View(pRestablecer);
            }
        }


        private Usuario ValidarUsuario(Usuario temp)
        {
            Usuario autorizado = null;

            //se busca el email dentro del catalogo de usuarios
            var user = _context.Usuarios.FirstOrDefault( u => u.Email.Equals(temp.Email));

            if (user != null) //se verifica si existe datos
            {
                if (user.Password.Equals(temp.Password)) //se valida el password
                {
                    autorizado = user; //se toman los datos el usuario
                }
            }
            return autorizado;
        }

        private bool VerificarRestablecer(Usuario temp)
        {
            bool verificado = false;

            var user = _context.Usuarios.FirstOrDefault(u => u.Email.Equals(temp.Email));

            if (user != null) // se verifica si hay datos
            {
                if (user.Restablecer == 'S') //se verifica si debe restablecer
                {
                    verificado = true; //se marca que debe restablecer la clave temporal
                }

            }
            //se retorna el valor para la variable control
            return verificado;  
        }

        /// <summary>
        /// Método encargado de mostrar el front-end para crear una cuenta de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CrearCuenta() { 
            return View(); 
        }

        //Método encargado de crear  una cuenta de usuario
        public async Task<IActionResult> CrearCuenta([Bind]Usuario pUser) {
            //se valida si tiene datos
            if (pUser == null)
            {
                return NotFound(); //Error 404 recurso no disponible
            }
            else
            { //Sí hay datos
                pUser.FechaRegistro = DateTime.Now; //Se asigna la fecha registro
                pUser.Estado = 'A'; //se indica que su estado es Activo
                pUser.Restablecer = 'S'; //se indica que debe restabler la contraseña

                //Se genera la clave temporal
                pUser.Password = GenerarClave();

                //se agrega el objeto usuario al catálogo
                _context.Usuarios.Add(pUser);

                try //se intente aplicar los cambios y enviar el email
                {
                    //se aplican los cambios
                    await _context.SaveChangesAsync();

                    //se crea una instancia del objeto email
                    Email email = new Email();

                    //Se envia como parámetro los datos del usuario que recibe la clave temporal
                    email.Enviar(pUser);

                    //Si todo está bien, se logró enviar el email
                    //se ubica al usuario dentro del formulario Login
                    return RedirectToAction("Login", "Usuarios");

                }
                catch (Exception ex) //ojo la variable ex almacena la información de error
                {
                    TempData["Mensaje"] = "No se logró crear la cuenta..<br>" +
                        "Verifique el siguiente mensaje de error:<br>" + ex.Message;

                    return View();//Se ubica al usuario dentro del formulario crear cuenta
                }
                
            }

        }

        //Método encargado de generar una clave temporal
        public string GenerarClave()
        {
            //Variable para generar un valor aleatorio
            Random rnd = new Random();

            //Variable para almacenar la clave
            string clave= string.Empty;

            //Caracteres utilizados para generar la clave temporal
            clave = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789";
            
            //Se genera la clave aleatoria
            return new string(Enumerable.Repeat(clave, 12).Select(
                s => s[ rnd.Next(s.Length)]).ToArray());
        }

        public async Task<IActionResult> Logout() {
           
            //cierre de sesión
            await  HttpContext.SignOutAsync();

            //se ubica al usuario dentro del formulario inicial
            return RedirectToAction("Index","Home");

        }


    }//cierre controller
}//cierre namespaces
