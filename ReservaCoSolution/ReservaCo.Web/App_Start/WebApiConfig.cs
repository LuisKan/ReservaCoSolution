// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase configura el comportamiento general de la Web API de ASP.NET.
// - Habilita CORS para permitir peticiones desde el frontend (por ejemplo, React en localhost:3000).
// - Define las rutas para los controladores de la API, incluyendo mapeo por atributos y ruta por defecto.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors; // Habilita el soporte para CORS en Web API.

namespace ReservaCo.Web
{
    // Clase estática que configura la API al iniciar la aplicación.
    public static class WebApiConfig
    {
        // Método que registra la configuración global de la Web API.
        public static void Register(HttpConfiguration config)
        {
            // Habilita CORS para permitir solicitudes desde http://localhost:3000 (cliente React u otro frontend).
            // El segundo y tercer parámetro "*" permiten todos los encabezados y métodos HTTP.
            var cors = new EnableCorsAttribute("http://localhost:3000", "*", "*");
            config.EnableCors(cors);

            // Habilita el mapeo de rutas mediante atributos [Route] en los controladores.
            config.MapHttpAttributeRoutes();

            // Define la ruta por defecto para los controladores sin atributos explícitos.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
