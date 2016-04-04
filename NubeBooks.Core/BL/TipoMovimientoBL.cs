using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
//using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class TipoMovimientoBL : Base
    {
        public List<TipoMovimientoDTO> getTiposMovimientos()
        {
            using (var context = getContext())
            {
                var result = context.TipoMovimiento.Select(x => new TipoMovimientoDTO
                {
                    IdTipoMovimiento = x.IdTipoMovimiento,
                    Nombre = x.Nombre,
                    Estado = x.Estado,
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }

        public TipoMovimientoDTO getTipoMovimiento(int id)
        {
            using (var context = getContext())
            {
                var result = context.TipoMovimiento.Where(x => x.IdTipoMovimiento == id)
                    .Select(r => new TipoMovimientoDTO
                    {
                        IdTipoMovimiento = r.IdTipoMovimiento,
                        Nombre = r.Nombre,
                        Estado = r.Estado,
                    }).SingleOrDefault();
                return result;
            }
        }

        public bool add(TipoMovimientoDTO TipoMovimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    TipoMovimiento nuevo = new TipoMovimiento();
                    nuevo.Nombre = TipoMovimiento.Nombre;
                    nuevo.Estado = true;
                    context.TipoMovimiento.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool update(TipoMovimientoDTO TipoMovimiento)
        {
            using (var context = getContext())
            {
                try
                {
                    var datoRow = context.TipoMovimiento.Where(x => x.IdTipoMovimiento == TipoMovimiento.IdTipoMovimiento).SingleOrDefault();
                    datoRow.Nombre = TipoMovimiento.Nombre;
                    datoRow.Estado = TipoMovimiento.Estado;
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
