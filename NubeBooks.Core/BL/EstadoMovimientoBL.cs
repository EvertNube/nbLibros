using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data.Objects.SqlClient;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class EstadoMovimientoBL : Base
    {
        public List<EstadoMovimientoDTO> getEstadosMovimientos()
        {
            using (var context = getContext())
            {
                var result = context.EstadoMovimiento.Select(x => new EstadoMovimientoDTO
                {
                    IdEstadoMovimiento = x.IdEstadoMovimiento,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                }).OrderByDescending(x => x.Nombre).ToList();
                return result;
            }
        }

        public EstadoMovimientoDTO getEstadoMovimiento(int id)
        {
            using (var context = getContext())
            {
                var result = context.EstadoMovimiento.Where(x => x.IdEstadoMovimiento == id)
                    .Select(r => new EstadoMovimientoDTO
                    {
                        IdEstadoMovimiento = r.IdEstadoMovimiento,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                    }).SingleOrDefault();
                return result;
            }
        }

        public bool add(EstadoMovimientoDTO EstadoMovimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    EstadoMovimiento nuevo = new EstadoMovimiento();
                    nuevo.Nombre = EstadoMovimiento.Nombre;
                    nuevo.Estado = true;
                    context.EstadoMovimiento.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool update(EstadoMovimientoDTO EstadoMovimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    var datoRow = context.EstadoMovimiento.Where(x => x.IdEstadoMovimiento == EstadoMovimiento.IdEstadoMovimiento).SingleOrDefault();
                    datoRow.Nombre = EstadoMovimiento.Nombre;
                    datoRow.Estado = EstadoMovimiento.Estado;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
