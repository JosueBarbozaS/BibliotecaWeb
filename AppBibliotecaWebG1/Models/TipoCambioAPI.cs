namespace AppBibliotecaWebG1.Models
{
    public class TipoCambioAPI
    {

        public HttpClient Iniciar()
        {
            //Variable para manejar el Object client
            var client = new HttpClient();

            //Se indica el nombre del dominio donde está publicada la API
            client.BaseAddress = new Uri("https://apis.gometa.org");

            //se retorna el objeto cliente
            return client;
        }

    }
}
