using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class MonedaBL : Base
    {
        public List<MonedaDTO> getListaMonedas()
        {
            using (var context = getContext())
            {
                var result = context.Moneda.Select(x => new MonedaDTO
                {
                    IdMoneda = x.IdMoneda,
                    Nombre = x.Nombre,
                    Simbolo = x.Simbolo
                }).Take(2).ToList();
                return result;
            }
        }
    }
}
