// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Se configura el punto de entrada de la aplicación web API.
// - Se inicializa la configuración del enrutamiento de servicios REST.
// - Se deja constancia de que CORS se configura en otro archivo.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ReservaCo.Web
{
    // Clase principal que representa la aplicación web
    public class WebApiApplication : System.Web.HttpApplication
    {
        // Método que se ejecuta al iniciar la aplicación
        protected void Application_Start()
        {
            // Configura el enrutamiento y otras configuraciones de Web API
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Nota: La configuración CORS (Cross-Origin Resource Sharing)
            // se realiza dentro del método WebApiConfig.Register
        }
    }
}
