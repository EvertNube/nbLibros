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
    public class ContactoBL : Base
    {
        public List<ContactoDTO> getContactos()
        {
            using (var context = getContext())
            {
                var result = context.Contacto.Select(x => new ContactoDTO
                {
                    IdContacto = x.IdContacto,
                    Nombre = x.Nombre,
                    Telefono = x.Telefono,
                    Celular = x.Celular,
                    Email = x.Email,
                    Cargo = x.Cargo,
                    Estado = x.Estado
                }).ToList();
                return result;
            }
        }
        public ContactoDTO getContacto(int id)
        {
            using (var context = getContext())
            {
                var result = context.Contacto.Where(x => x.IdContacto == id)
                    .Select(x => new ContactoDTO
                    {
                        IdContacto = x.IdContacto,
                        IdEntidadResponsable = x.IdEntidadResponsable,
                        Nombre = x.Nombre,
                        Telefono = x.Telefono,
                        Celular = x.Celular,
                        Email = x.Email,
                        Cargo = x.Cargo,
                        Estado = x.Estado,
                    }).SingleOrDefault();
                return result;
            }
        }
        public bool add(ContactoDTO Contacto)
        {
            using (var context = getContext())
            {
                try
                {
                    Contacto nuevo = new Contacto();
                    nuevo.Nombre = Contacto.Nombre;
                    nuevo.IdEntidadResponsable = Contacto.IdEntidadResponsable;
                    nuevo.Estado = true;
                    nuevo.Telefono = Contacto.Telefono;
                    nuevo.Celular = Contacto.Celular;
                    nuevo.Email = Contacto.Email;
                    nuevo.Cargo = Contacto.Cargo;
                    context.Contacto.Add(nuevo);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool update(ContactoDTO Contacto)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Contacto.Where(x => x.IdContacto == Contacto.IdContacto).SingleOrDefault();
                    row.IdEntidadResponsable = Contacto.IdEntidadResponsable;
                    row.Nombre = Contacto.Nombre;
                    row.Telefono = Contacto.Telefono;
                    row.Celular = Contacto.Celular;
                    row.Email = Contacto.Email;
                    row.Estado = Contacto.Estado;
                    row.Cargo = Contacto.Cargo;
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
