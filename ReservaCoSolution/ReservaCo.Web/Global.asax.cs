// ************************************************************************
// Proyecto 02 
// Aguilar Ver�nica, Guerrero Luis
// Fecha de realizaci�n: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Se configura el punto de entrada de la aplicaci�n web API.
// - Se inicializa la configuraci�n del enrutamiento de servicios REST.
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
    // Clase principal que representa la aplicaci�n web
    public class WebApiApplication : System.Web.HttpApplication
    {
        // M�todo que se ejecuta al iniciar la aplicaci�n
        protected void Application_Start()
        {
            // Configura el enrutamiento y otras configuraciones de Web API
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Nota: La configuraci�n CORS (Cross-Origin Resource Sharing)
            // se realiza dentro del m�todo WebApiConfig.Register
        }
    }
}
