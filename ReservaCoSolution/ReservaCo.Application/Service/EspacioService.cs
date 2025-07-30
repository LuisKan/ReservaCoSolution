using System;
using System.Linq;
using ReservaCo.Domain.Entities;
using ReservaCo.Domain.Business;
using ReservaCo.Infrastructure.Context;
using System.Collections.Generic;

namespace ReservaCo.Application.Service
{
    public class EspacioService
    {

        private readonly ReservaCoDbContext _context;

        public EspacioService(ReservaCoDbContext context)
        {
            _context = context;
        }

        public List<Espacio> ObtenerEspacios()
        {
            return _context.Espacios.ToList();
        }


        public void GuardarEspacio(Espacio espacio)
        {
            var existentes = _context.Espacios
        .Where(e => e.Nombre == espacio.Nombre && e.Id != espacio.Id) // excluye el mismo
        .ToList();


            EspacioRules.Validar(espacio);
            EspacioRules.FormatearNombre(espacio);
            EspacioRules.ValidarCapacidad(espacio);
            EspacioRules.ValidarNombreUnico(espacio, existentes);

            if (espacio.Id == 0)
            {
                if (espacio.FechaCreacion == DateTime.MinValue)
                    espacio.FechaCreacion = DateTime.Now;

                _context.Espacios.Add(espacio);
            }
            else
            {
                _context.Entry(espacio).State = System.Data.Entity.EntityState.Modified;
            }

            _context.SaveChanges();
        }



        public void EliminarEspacio(int id)
        {
            var espacio = _context.Espacios.Find(id);
            if (espacio != null)
            {
                _context.Espacios.Remove(espacio);
                _context.SaveChanges();
            }
        }

    }
}
