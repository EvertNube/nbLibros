using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.BL
{
    public class CategoriaBL : Base
    {
        public List<CategoriaDTO> getCategorias()
        {
            using (var context = getContext())
            {
                var result = context.Categoria.Select(x => new CategoriaDTO
                    {
                        IdCategoria = x.IdCategoria,
                        Nombre = x.Nombre,
                        Orden = x.Orden,
                        Estado = x.Estado,
                        IdCategoriaPadre = x.IdCategoriaPadre,
                        IdEmpresa = x.IdEmpresa
                    }).ToList();
                return result;
            }
        }

        public List<CategoriaDTO> getCategoriasTreeEnEmpresa(int idEmpresa, int? id = null)
        {
            using (var context = getContext())
            {
                var result = from r in context.Categoria
                             where ((id == null ? r.IdCategoriaPadre == null : r.IdCategoriaPadre == id) && r.IdEmpresa == idEmpresa)
                             select new CategoriaDTO
                             {
                                 IdCategoria = r.IdCategoria,
                                 Nombre = r.Nombre,
                                 Orden = r.Orden,
                                 Estado = r.Estado,
                                 IdCategoriaPadre = r.IdCategoriaPadre,
                                 IdEmpresa = r.IdEmpresa,
                                 Presupuesto = r.CategoriaPorPeriodo.Where(x => x.IdPeriodo == r.Empresa.IdPeriodo).FirstOrDefault().Monto
                             };
                List<CategoriaDTO> categoriasTree = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();

                foreach (CategoriaDTO obj in categoriasTree)
                {
                    obj.Hijos = getCategoriasTreeEnEmpresa(idEmpresa, obj.IdCategoria);
                }
                return categoriasTree;
            }
        }

        public List<CategoriaDTO> getCategoriasPorPeriodo_ArbolEnEmpresa(int idEmpresa, int idPeriodo, int? id = null)
        {
            using (var context = getContext())
            {
                var result = from cp in context.CategoriaPorPeriodo
                              join ct in context.Categoria on cp.IdCategoria equals ct.IdCategoria
                              where ((id == null ? ct.IdCategoriaPadre == null : ct.IdCategoriaPadre == id) && cp.IdPeriodo == idPeriodo && ct.IdEmpresa == idEmpresa)
                              select new CategoriaDTO
                              {
                                  IdCategoria = ct.IdCategoria,
                                  Nombre = ct.Nombre,
                                  Orden = ct.Orden,
                                  Estado = ct.Estado,
                                  IdCategoriaPadre = ct.IdCategoriaPadre,
                                  IdEmpresa = ct.IdEmpresa,
                                  Presupuesto = cp.Monto
                              };

                List<CategoriaDTO> categoriasTree = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();

                foreach (CategoriaDTO obj in categoriasTree)
                {
                    obj.Hijos = getCategoriasPorPeriodo_ArbolEnEmpresa(idEmpresa, idPeriodo, obj.IdCategoria);
                }
                return categoriasTree;
            }
        }

        public IList<CategoriaDTO> getCategoriasTree(int? id = null)
        {
            using (var context = getContext())
            {
                var result = from r in context.Categoria
                             where (id == null ? r.IdCategoriaPadre == null : r.IdCategoriaPadre == id)
                             select new CategoriaDTO
                             {
                                 IdCategoria = r.IdCategoria,
                                 Nombre = r.Nombre,
                                 Orden = r.Orden,
                                 Estado = r.Estado,
                                 IdCategoriaPadre = r.IdCategoriaPadre,
                                 IdEmpresa = r.IdEmpresa
                             };
                IList<CategoriaDTO> categoriasTree = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();

                foreach (CategoriaDTO obj in categoriasTree)
                {
                    obj.Hijos = getCategoriasTree(obj.IdCategoria);
                }
                return categoriasTree;
            }
        }

        public IList<CategoriaDTO> getCategoriasPadreEnEmpresa(int idEmpresa, bool AsSelectList = false)
        {
            if (!AsSelectList)
                return getCategoriasTreeEnEmpresa(idEmpresa);
            else
            {
                var lista = getCategoriasTreeEnEmpresa(idEmpresa);
                lista.Insert(0, new CategoriaDTO() { IdCategoria = 0, Nombre = "Seleccione la Categoría Padre" });
                return lista;
            }
        }
        public IList<CategoriaDTO> getCategoriasPadre(bool AsSelectList = false)
        {
            if (!AsSelectList)
                return getCategoriasTree();
            else
            {
                var lista = getCategoriasTree();
                lista.Insert(0, new CategoriaDTO() { IdCategoria = 0, Nombre = "Seleccione la Categoría Padre" });
                return lista;
            }
        }

        public CategoriaDTO getCategoria(int id)
        {
            using (var context = getContext())
            {
                var result = context.Categoria.Where(x => x.IdCategoria == id)
                    .Select(r => new CategoriaDTO
                    {
                        IdCategoria = r.IdCategoria,
                        Nombre = r.Nombre,
                        Orden = r.Orden,
                        Estado = r.Estado,
                        IdCategoriaPadre = r.IdCategoriaPadre,
                        IdEmpresa = r.IdEmpresa
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(CategoriaDTO Categoria)
        {
            using (var context = getContext())
            {
                try
                {
                    Categoria nuevo = new Categoria();
                    nuevo.Nombre = Categoria.Nombre;
                    if (Categoria.IdCategoriaPadre != 0 && Categoria.IdCategoriaPadre != null)
                        nuevo.Orden = getUltimoHijo(Categoria.IdCategoriaPadre.GetValueOrDefault());
                    else
                        nuevo.Orden = 1;
                    //nuevo.Orden = Categoria.Orden;
                    nuevo.Estado = true;
                    nuevo.IdCategoriaPadre = Categoria.IdCategoriaPadre;
                    nuevo.IdEmpresa = Categoria.IdEmpresa;
                    context.Categoria.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(CategoriaDTO Categoria, int idPeriodo)
        {
            using (var context = getContext())
            {
                try
                {
                    var datoRow = context.Categoria.Where(x => x.IdCategoria == Categoria.IdCategoria).SingleOrDefault();
                    datoRow.Nombre = Categoria.Nombre;
                    if (Categoria.IdCategoriaPadre != 0 && Categoria.IdCategoriaPadre != null)
                        datoRow.Orden = getUltimoHijo(Categoria.IdCategoriaPadre.GetValueOrDefault());
                    else
                        datoRow.Orden = 1;

                    int pPadreAnterior = datoRow.IdCategoriaPadre.GetValueOrDefault();

                    datoRow.Estado = Categoria.Estado;
                    datoRow.IdCategoriaPadre = Categoria.IdCategoriaPadre;
                    datoRow.IdEmpresa = Categoria.IdEmpresa;
                    context.SaveChanges();

                    //Actualizamos padres
                    if(idPeriodo != 0)
                    {
                        //Actualizamos padre antiguo
                        if (pPadreAnterior != 0) context.SP_ActualizarPresupuestoPadre(pPadreAnterior, idPeriodo);
                        //Actualizamos padre nuevo
                        if (Categoria.IdCategoriaPadre.GetValueOrDefault() != 0) context.SP_ActualizarPresupuestoPadre(Categoria.IdCategoriaPadre, idPeriodo);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool updatePresupuesto(CategoriaPorPeriodoDTO dto)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Categoria.Where(x => x.IdCategoria == dto.IdCategoria).SingleOrDefault();
                    if(row.CategoriaPorPeriodo.Where(x => x.IdPeriodo == dto.IdPeriodo).Count() == 0)
                    {
                        CategoriaPorPeriodo novo = new CategoriaPorPeriodo() { IdCategoria = dto.IdCategoria, IdPeriodo = dto.IdPeriodo, Monto = dto.Monto };
                        row.CategoriaPorPeriodo.Add(novo);
                    }
                    else
                    {
                        row.CategoriaPorPeriodo.Where(x => x.IdPeriodo == dto.IdPeriodo).Single().Monto = dto.Monto;
                    }
                    context.SaveChanges();
                    //Actualizacion de Padres
                    int pPadreCategoria = getCategoria(dto.IdCategoria).IdCategoriaPadre.GetValueOrDefault();
                    if(pPadreCategoria != 0)
                    {
                        context.SP_ActualizarPresupuestoPadre(pPadreCategoria, dto.IdPeriodo);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public int getUltimoHijo(int idPadre)
        {
            using (var context = getContext())
            {
                var result = from r in context.Categoria.Where(x => x.IdCategoriaPadre == idPadre)
                             select new CategoriaDTO
                             {
                                 IdCategoria = r.IdCategoria,
                                 Nombre = r.Nombre,
                                 Orden = r.Orden,
                                 Estado = r.Estado,
                                 IdCategoriaPadre = r.IdCategoriaPadre,
                                 IdEmpresa = r.IdEmpresa
                             };
                IList<CategoriaDTO> lstCategorias = result.AsEnumerable<CategoriaDTO>().OrderBy(x => x.Orden).ToList<CategoriaDTO>();
                if (lstCategorias.Count == 0)
                    return 1;
                return lstCategorias.Last().Orden + 1;
            }
        }

        public string getNombreCategoria(int id)
        {
            if (id != 0)
            {
                CategoriaBL oBL = new CategoriaBL();
                return oBL.getCategoria(id).Nombre;
            }
            return "Sin Categoría";
        }

        /*
        public IList<CategoriaR_DTO> getReporteCategorias(int? IdCuentaB, DateTime? FechaInicio, DateTime? FechaFin)
        {
            using (var context = getContext())
            {
                var result = context.SP_GetReporteResumenCategorias(IdCuentaB, FechaInicio, FechaFin).Select(r => new CategoriaR_DTO
                {
                    IdCategoria = r.IdCategoria,
                    Nombre = r.Nombre,
                    MontoTotal = r.MontoTotal.GetValueOrDefault(),
                    IdCategoriaPadre = r.IdCategoriaPadre
                }).ToList();
                return result;
            }
        }
        
        public CategoriaR_DTO obtenerPadreEntidadReporte(CategoriaR_DTO obj, List<CategoriaDTO> lstCategorias, int Nivel)
        {
            if (obj.IdCategoriaPadre != null)
            {
                CategoriaR_DTO nuevo = new CategoriaR_DTO();
                CategoriaDTO aux = lstCategorias.Find(x => x.IdCategoria == obj.IdCategoriaPadre);
                nuevo.IdCategoria = aux.IdCategoria;
                nuevo.Nombre = aux.Nombre;
                nuevo.IdCategoriaPadre = aux.IdCategoriaPadre;

                if (nuevo.IdCategoriaPadre != null)
                {
                    nuevo = obtenerPadreEntidadReporte(nuevo, lstCategorias, Nivel);
                    nuevo.Nivel = nuevo.Padre.Nivel + 1;
                }
                else
                { nuevo.Nivel = Nivel; }
                obj.Padre = nuevo;
                obj.Nivel = nuevo.Nivel + 1;
                
                if (CONSTANTES.NivelCat < obj.Nivel)
                    CONSTANTES.NivelCat = obj.Nivel;
            }
            return obj;
        }*/

        public List<PeriodoDTO> GetPeriodosEnEmpresaViewBag(int idEmpresa)
        {
            PeriodoBL objBL = new PeriodoBL();
            return objBL.getPeriodosEnEmpresaViewBag(idEmpresa);
        }
    }
}
