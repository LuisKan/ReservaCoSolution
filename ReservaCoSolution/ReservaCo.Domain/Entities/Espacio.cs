// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Esta clase representa la entidad de dominio 'Espacio', utilizada para modelar aulas, laboratorios u otros lugares reservables.
// - Contiene propiedades como nombre, tipo, ubicación, capacidad y fecha de creación del espacio.
// - Incluye la relación con las reservas que se han hecho para este espacio.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    // Clase que representa un espacio físico dentro del sistema (ej. Aula, Laboratorio).
    public class Espacio
    {
        // Identificador único del espacio.
        public int Id { get; set; }

        // Nombre asignado al espacio (ej. Aula B101, Lab Redes).
        public string Nombre { get; set; }

        // Tipo de espacio (ej. Aula, Laboratorio, Auditorio).
        public string Tipo { get; set; }

        // Ubicación física del espacio dentro del campus o edificio.
        public string Ubicacion { get; set; }

        // Capacidad máxima de personas que admite el espacio.
        public int Capacidad { get; set; }

        // Fecha en la que se registró este espacio en el sistema.
        public DateTime FechaCreacion { get; set; }

        // Colección de reservas asociadas a este espacio (relación uno a muchos).
        public ICollection<Reserva> Reservas { get; set; }
    }
}
