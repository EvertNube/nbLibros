using NubeBooks.Core.Logistics.DTO;
using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class ItemBL : Base
    {
        public List<ItemDTO> getItemsEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Item.Where(x => x.IdEmpresa == idEmpresa).Select(x => new ItemDTO
                {
                    IdItem = x.IdItem,
                    IdCategoriaItm = x.IdCategoriaItm,
                    IdMoneda = x.IdMoneda,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    UnidadMedida = x.UnidadMedida,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    nMoneda = x.Moneda.Nombre,
                    simboloMoneda = x.Moneda.Simbolo,
                    Precio = x.Precio,
                    nCategoriaItem = x.CategoriaItm.Nombre
                }).ToList();
                return result;
            }
        }
        public List<ItemDTO> getItemsActivosEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Item.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new ItemDTO
                {
                    IdItem = x.IdItem,
                    IdCategoriaItm = x.IdCategoriaItm,
                    IdMoneda = x.IdMoneda,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    UnidadMedida = x.UnidadMedida,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    nMoneda = x.Moneda.Nombre,
                    simboloMoneda = x.Moneda.Simbolo,
                    Precio = x.Precio
                }).ToList();
                return result;
            }
        }
        public List<ItemDTO> getItemsEnEmpresaViewBag(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.Item.Where(x => x.Estado && x.IdEmpresa == idEmpresa).Select(x => new ItemDTO
                {
                    IdItem = x.IdItem,
                    IdCategoriaItm = x.IdCategoriaItm,
                    IdMoneda = x.IdMoneda,
                    Codigo = x.Codigo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    UnidadMedida = x.UnidadMedida,
                    Estado = x.Estado,
                    IdEmpresa = x.IdEmpresa,
                    simboloMoneda = x.Moneda.Simbolo
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
        public ItemDTO getItemEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Item.Where(x => x.IdEmpresa == idEmpresa && x.IdItem == id)
                    .Select(x => new ItemDTO
                    {
                        IdItem = x.IdItem,
                        IdCategoriaItm = x.IdCategoriaItm,
                        IdMoneda = x.IdMoneda,
                        Codigo = x.Codigo,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripcion,
                        UnidadMedida = x.UnidadMedida,
                        Estado = x.Estado,
                        IdEmpresa = x.IdEmpresa,
                        Precio = x.Precio,
                        simboloMoneda = x.Moneda.Simbolo
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(ItemDTO Item)
        {
            using (var context = getContext())
            {
                try
                {
                    Item nuevo = new Item();
                    nuevo.IdCategoriaItm = Item.IdCategoriaItm;
                    nuevo.IdMoneda = Item.IdMoneda;
                    nuevo.Codigo = Item.Codigo;
                    nuevo.Nombre = Item.Nombre;
                    nuevo.Precio = Item.Precio;
                    nuevo.Descripcion = Item.Descripcion;
                    nuevo.UnidadMedida = Item.UnidadMedida;
                    nuevo.Estado = true;
                    nuevo.IdEmpresa = Item.IdEmpresa;
                    context.Item.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(ItemDTO Item)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Item.Where(x => x.IdItem == Item.IdItem).SingleOrDefault();
                    row.IdCategoriaItm = Item.IdCategoriaItm;
                    row.IdMoneda = Item.IdMoneda;
                    row.Codigo = Item.Codigo;
                    row.Nombre = Item.Nombre;
                    row.Precio = Item.Precio;
                    row.Descripcion = Item.Descripcion;
                    row.UnidadMedida = Item.UnidadMedida;
                    row.Estado = Item.Estado;
                    row.IdEmpresa = Item.IdEmpresa;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public List<CategoriaItmDTO> getCategoriasEnEmpresa(int idEmpresa)
        {
            using (var context = getContext())
            {
                var result = context.CategoriaItm.Where(x => x.IdEmpresa == idEmpresa && x.Estado).Select(x => new CategoriaItmDTO
                {
                    IdCategoriaItm = x.IdCategoriaItm,
                    Nombre = x.Nombre,
                    Estado = x.Estado
                }).OrderBy(x => x.Nombre).ToList();
                return result;
            }
        }
    }
}
