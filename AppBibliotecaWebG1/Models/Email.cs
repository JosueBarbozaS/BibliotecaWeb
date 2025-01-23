using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AppBibliotecaWebG1.Models
{
    public class Email
    {
        //Método encargado de enviar la clave temporal al usuario
        public void Enviar(Usuario usuario)
        {
            try
            {
                //Se crea la instancia del objeto Email
                MailMessage email = new MailMessage();

                //Se indica el asunto del email
                email.Subject = "Datos de registro en plataforma web biblioteca CR";

                //Se agrega al email el correo del destinatario
                email.To.Add(new MailAddress(usuario.Email) );

                //Se copia al administrador de la cuenta
                email.Bcc.Add(new MailAddress("ejemplo@Outlook.com"));

                //Se indica el Emisor
                email.From = new MailAddress("ejemplo@Outlook.com");

                //se construye el html para el body del email
                string html = "Bienvenidos a biblioteca web CR gracias por formar parte de nuestra plataforma";

                html += "<br>A continuación detallamos los datos registrados en nuestra plataforma web:";

                html += "<br><b>Email:</b> " + usuario.Email;

                html += "<br><b>Nombre completo:</b>" + usuario.NombreCompleto;

                html += "<br><b>Su contraseña temporal es: </b>" + usuario.Password;

                html += "<br><b>No responda este correo porque fue generado de forma automatica.";

                html += "<br>Plataforma web biblioteca CR..</b>";

                //se indica que el contenido es en HTML
                email.IsBodyHtml = true;

                //Se indica la prioridad, recomendación utilizar prioridad normal
                email.Priority = MailPriority.Normal;

                //Se intancia la vista del html para el cuerpo del email
                AlternateView view = AlternateView.CreateAlternateViewFromString(html,Encoding.UTF8,
                    MediaTypeNames.Text.Html);

                //seagregala view al email
                email.AlternateViews.Add(view);

                //se configura el protocolo de comunicación smtp
                SmtpClient smtp = new SmtpClient();

                //Se indica el nombre del servidor smtp a sincronizar la cuenta
                smtp.Host = "smtp-mail.outlook.com";

                //se indica el número de puerto comunicación
                smtp.Port = 587;

                //se indica si utiliza seguridad tipo SSL
                smtp.EnableSsl = true;

                //se indican la credenciales por default para el buzón de correo
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("ejemplo@Outlook.com", "ejemplo123*");

                //se envia el email
                smtp.Send(email);

                //se liberan los recursos
                email.Dispose();
                smtp.Dispose();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    } //cierre class
} //cierre namespaces
