using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AppBibliotecaWebG1.Models;

namespace AppBibliotecaWebG1.Models
{
    public class GoMetaService
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
