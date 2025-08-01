// ************************************************************************
// Proyecto 02 
// Aguilar Verónica, Guerrero Luis
// Fecha de realización: 21/07/2025 
// Fecha de entrega: 03/08/2025  
// Resultados:
// - Este archivo define el enumerador 'EstadoReserva', que representa los posibles estados en los que puede encontrarse una reserva.
// - Se utiliza para el control de flujo y visualización del estado de aprobación de una solicitud de reserva.
// - Los estados posibles son: Pendiente, Aprobada y Rechazada.
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaCo.Domain.Entities
{
    // Enumeración que representa los estados posibles de una reserva en el sistema.
    public enum EstadoReserva
    {
        // Reserva creada pero aún no ha sido aprobada ni rechazada.
        Pendiente,

        // Reserva que ha sido aprobada por el administrador o coordinador.
        Aprobada,

        // Reserva que ha sido rechazada por algún motivo.
        Rechazada
    }
}
