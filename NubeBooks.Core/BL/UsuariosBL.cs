using NubeBooks.Core.DTO;
using NubeBooks.Data;
using NubeBooks.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//required for sql function access
using System.Data.Entity.Core.Objects.DataClasses;

namespace NubeBooks.Core.BL
{
    public class UsuariosBL : Base
    {
        public List<UsuarioDTO> getUsuariosEnEmpresa(int idEmpresa, int IdRol)
        {
            using (var context = getContext())
            {
                //where getRoleKeys(IdRol).Contains(r.IdRol) && r.IdUsuario != 1 && r.IdEmpresa == idEmpresa
                var result = from r in context.Usuario.AsEnumerable()
                             where getRoleKeys(IdRol).Contains(r.IdRol) && r.IdEmpresa == idEmpresa
                             select new UsuarioDTO
                             {
                                 IdUsuario = r.IdUsuario,
                                 Nombre = r.Nombre,
                                 Email = r.Email,
                                 Cuenta = r.Cuenta,
                                 Active = r.Estado,
                                 IdRol = r.IdRol, //?? 0
                                 NombreRol = r.Rol.Nombre,
                                 IdCargo = r.IdCargo,
                                 IdEmpresa = r.IdEmpresa,
                                 nombreEmpresa = r.Empresa.Nombre ?? "N/A",
                                 Token = r.Token
                             };
                return result.ToList<UsuarioDTO>();
            }
        }
        public List<UsuarioDTO> getUsuariosActivosEnEmpresa(int idEmpresa, int IdRol)
        {
            using (var context = getContext())
            {
                //where getRoleKeys(IdRol).Contains(r.IdRol) && r.IdUsuario != 1 && r.IdEmpresa == idEmpresa
                var result = from r in context.Usuario.AsEnumerable()
                             where getRoleKeys(IdRol).Contains(r.IdRol) && r.IdEmpresa == idEmpresa && r.Estado
                             select new UsuarioDTO
                             {
                                 IdUsuario = r.IdUsuario,
                                 Nombre = r.Nombre,
                                 Email = r.Email,
                                 Cuenta = r.Cuenta,
                                 Active = r.Estado,
                                 IdRol = r.IdRol, //?? 0
                                 NombreRol = r.Rol.Nombre,
                                 IdCargo = r.IdCargo,
                                 IdEmpresa = r.IdEmpresa,
                                 nombreEmpresa = r.Empresa.Nombre ?? "N/A",
                                 Token = r.Token
                             };
                return result.ToList<UsuarioDTO>();
            }
        }
        public IList<UsuarioDTO> getUsuarios()
        {
            using (var context = getContext())
            {
                var result = from r in context.Usuario
                             where r.Estado == true && r.IdRol != CONSTANTES.SUPER_ADMIN_ROL
                             select new UsuarioDTO
                             {
                                 IdUsuario = r.IdUsuario,
                                 Nombre = r.Nombre,
                                 Email = r.Email,
                                 Cuenta = r.Cuenta,
                                 Pass = r.Pass,
                                 Active = r.Estado,
                                 IdRol = r.IdRol, //?? 0
                                 IdCargo = r.IdCargo,
                                 IdEmpresa = r.IdEmpresa,
                                 Token = r.Token
                             };
                return result.AsEnumerable<UsuarioDTO>().OrderByDescending(x => x.IdUsuario).ToList<UsuarioDTO>();
            }
        }
        public IList<UsuarioDTO> getUsuarios(int IdRol)
        {
            using (var context = getContext())
            {
                var result = from r in context.Usuario.AsEnumerable()
                             //where getRoleKeys(IdRol).Contains(r.IdRol)//& r.IdRol != CONSTANTES.SUPER_ADMIN_ROL & r.Estado == true
                             where getRoleKeys(IdRol).Contains(r.IdRol) && r.IdUsuario != 1
                             select new UsuarioDTO
                             {
                                 IdUsuario = r.IdUsuario,
                                 Nombre = r.Nombre,
                                 Email = r.Email,
                                 Cuenta = r.Cuenta,
                                 Active = r.Estado,
                                 IdRol = r.IdRol, //?? 0
                                 NombreRol = r.Rol.Nombre,
                                 IdCargo = r.IdCargo,
                                 IdEmpresa = r.IdEmpresa,
                                 Token = r.Token
                             };
                return result.ToList<UsuarioDTO>();//.AsEnumerable<UsuarioDTO>().OrderByDescending(x => x.Nombre).ToList<UsuarioDTO>();
            }
        }

        public int[] getRoleKeys(int IdRol)
        {
            var roles = new int[1];
            if (IdRol == CONSTANTES.SUPER_ADMIN_ROL) roles = new int[] { CONSTANTES.SUPER_ADMIN_ROL, CONSTANTES.ROL_ADMIN, CONSTANTES.ROL_USUARIO_INT, CONSTANTES.ROL_USUARIO_EXT };
            if (IdRol == CONSTANTES.ROL_ADMIN) roles = new int[] { CONSTANTES.ROL_ADMIN, CONSTANTES.ROL_USUARIO_INT, CONSTANTES.ROL_USUARIO_EXT };
            if (IdRol == CONSTANTES.ROL_USUARIO_INT) roles = new int[] { CONSTANTES.ROL_USUARIO_INT, CONSTANTES.ROL_USUARIO_EXT };
            if (IdRol == CONSTANTES.ROL_USUARIO_EXT) roles = new int[] { CONSTANTES.ROL_USUARIO_EXT };
            return roles;
        }

        public IList<UsuarioDTO> getUsuarios(int IdRol, int[] usuariosIds)
        {
            using (var context = getContext())
            {
                var result = from r in context.Usuario
                             where r.Estado == true & r.IdRol != CONSTANTES.SUPER_ADMIN_ROL & r.IdRol == IdRol & usuariosIds.Contains(r.IdUsuario)
                             select new UsuarioDTO
                             {
                                 IdUsuario = r.IdUsuario,
                                 Nombre = r.Nombre,
                                 Email = r.Email,
                                 Cuenta = r.Cuenta,
                                 Active = r.Estado,
                                 IdRol = r.IdRol, //?? 0
                                 IdCargo = r.IdCargo,
                                 IdEmpresa = r.IdEmpresa,
                                 Token = r.Token
                             };
                return result.AsEnumerable<UsuarioDTO>().OrderByDescending(x => x.Nombre).ToList<UsuarioDTO>();
            }
        }

        public System.Collections.IList getUsuarios2(int IdRol)
        {
            using (var context = getContext())
            {
                var result = from r in context.Usuario
                             where r.Estado == true & r.IdRol != CONSTANTES.SUPER_ADMIN_ROL & r.IdRol == IdRol
                             select new
                             {
                                 name = r.Nombre,
                                 id = r.IdUsuario,
                                 //tareas = context.Tarea.Where(x => x.IdResponsable == r.IdUsuario).Select(y => new { IdTarea = y.IdTarea, Nombre = y.Nombre, FechaInicio = y.FechaInicio, FechaFin = y.FechaFin })
                             };
                return result.ToList();//.AsEnumerable().OrderByDescending(x => x.name).ToList();
            }
        }

        public bool add(UsuarioDTO user)
        {
            using (var context = getContext())
            {
                try
                {
                    Usuario usuario = new Usuario();
                    usuario.Nombre = user.Nombre;
                    usuario.Email = user.Email;
                    usuario.Cuenta = user.Cuenta;
                    usuario.Pass = Encrypt.GetCrypt(user.Pass);
                    usuario.IdRol = user.IdRol;//>= 2 ? user.IdRol : 3;
                    //usuario.IdCargo = user.IdCargo;
                    usuario.Estado = true;
                    usuario.FechaRegistro = DateTime.Now;
                    usuario.IdCargo = user.IdCargo;
                    usuario.IdEmpresa = user.IdEmpresa;
                    context.Usuario.Add(usuario);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool validateUsuario(UsuarioDTO user)
        {
            using (var context = getContext())
            {
                if (user.Cuenta != null && user.Email != null)
                {
                    var result = from r in context.Usuario
                                 where (r.Cuenta == user.Cuenta || r.Email == user.Email) && r.IdEmpresa == user.IdEmpresa
                                 select r;
                    if (result.FirstOrDefault<Usuario>() == null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool validateUsuarioNoDuplicado(UsuarioDTO user)
        {
            using (var context = getContext())
            {
                if (user.IdUsuario > 0 && user.Cuenta != null && user.Email != null)
                {
                    var element = context.Usuario.Where(x => x.IdUsuario == user.IdUsuario);
                    var list = context.Usuario.Where(x => (x.Cuenta == user.Cuenta || x.Email == user.Email) && x.IdEmpresa == user.IdEmpresa).Except(element).ToList<Usuario>();

                    if (list.Count == 0)
                    { return true; }

                    return false;
                }
                return false;
            }
        }
        public UsuarioDTO getUserByAcountOrEmail(UsuarioDTO user)
        {
            using (var context = getContext())
            {
                if (user.Cuenta != null || user.Email != null)
                {
                    var result = context.Usuario.Where(x => (x.Cuenta == user.Cuenta || x.Email == user.Cuenta) && x.Empresa.Codigo == user.codigoEmpresa).Select(x => new UsuarioDTO
                    {
                        IdUsuario = x.IdUsuario,
                        Nombre = x.Nombre,
                        Email = x.Email,
                        Cuenta = x.Cuenta,
                        Token = x.Token,
                        IdRol = x.IdRol,
                        IdEmpresa = x.IdEmpresa,
                        codigoEmpresa = x.Empresa.Codigo
                    }).FirstOrDefault();
                    if (result == null)
                    {
                        return null;
                    }
                    return result;
                }
                return null;
            }
        }

        public bool updateToken(UsuarioDTO usuario)
        {
            using (var context = getContext())
            {
                try
                {
                    var row = context.Usuario.Where(x => x.IdUsuario == usuario.IdUsuario).SingleOrDefault();
                    row.Token = usuario.Token;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public List<RolDTO> getRoles()
        {
            using (var context = getContext())
            {
                var result = context.Rol.Where(x => x.IdRol != CONSTANTES.SUPER_ADMIN_ROL).Select(r => new RolDTO
                {
                    IdRol = r.IdRol,
                    Nombre = r.Nombre
                }).ToList();

                return result;
            }
        }

        public List<SelectDTO> getSelectRoles()
        {
            using (var context = getContext())
            {
                var result = context.Rol.Where(x => x.IdRol != CONSTANTES.SUPER_ADMIN_ROL).Select(r => new SelectDTO
                    {
                        SelectItemId = r.IdRol,
                        SelectItemName = r.Nombre
                    }).ToList();
                return result;
            }
        }

        public List<SelectDTO> getSelectAllRoles()
        {
            using (var context = getContext())
            {
                var result = context.Rol.Select(r => new SelectDTO
                {
                    SelectItemId = r.IdRol,
                    SelectItemName = r.Nombre
                }).ToList();
                return result;
            }
        }

        public IList<RolDTO> getRolesDown(int idRol)
        {
            using (var context = getContext())
            {
                var result = from r in context.Rol.AsEnumerable()
                             where getRoleKeys(idRol).Contains(r.IdRol)
                             select new RolDTO
                             {
                                 IdRol = r.IdRol,
                                 Nombre = r.Nombre
                             };
                return result.ToList<RolDTO>();
            }
        }

        public IList<SelectDTO> getRolesViewBag(bool AsSelectList = false)
        {
            if (!AsSelectList)
            {
                return getSelectRoles();
            }
            else
            {
                var lista = getSelectRoles();
                lista.Insert(0, new SelectDTO() { SelectItemId = 0, SelectItemName = "Seleccione el Rol del usuario." });
                return lista;
            }
        }

        public IList<SelectDTO> getAllRolesViewBag(bool AsSelectList = false)
        {
            if (!AsSelectList)
            {
                return getSelectAllRoles();
            }
            else
            {
                var lista = getSelectAllRoles();
                lista.Insert(0, new SelectDTO() { SelectItemId = 0, SelectItemName = "Seleccione el Rol del usuario." });
                return lista;
            }
        }

        public UsuarioDTO getUsuarioByCuenta(UsuarioDTO user)
        {
            using (var context = getContext())
            {
                var result = from r in context.Usuario
                             where r.Estado == true && r.Cuenta == user.Cuenta && r.Empresa.Codigo == user.codigoEmpresa
                             select new UsuarioDTO
                             {
                                 IdUsuario = r.IdUsuario,
                                 Nombre = r.Nombre,
                                 IdRol = r.IdRol,// ?? 0,
                                 Active = r.Estado,
                                 Email = r.Email,
                                 Pass = r.Pass,
                                 Cuenta = r.Cuenta,
                                 IdCargo = r.IdCargo,
                                 IdEmpresa = r.IdEmpresa,
                                 nombreEmpresa = r.Empresa.Nombre ?? "N/A",
                                 //empresa = context.Empresa.Where(x => x.IdEmpresa == r.IdEmpresa).Select(y => new EmpresaDTO { IdEmpresa = y.IdEmpresa, Nombre = y.Nombre ?? "N/A", Estado = y.Estado, Descripcion = y.Descripcion, TipoCambio = y.TipoCambio }).FirstOrDefault()

                             };
                return result.SingleOrDefault<UsuarioDTO>();
            }
        }

        public bool isValidUser(UsuarioDTO user)
        {
            if (user.Cuenta == null || user.Pass == null || user.codigoEmpresa == null)
                return false;

            using (var context = getContext())
            {
                /*var result = from r in context.Usuario
                             where r.Estado == true && r.Cuenta == user.Cuenta && r.Empresa.Codigo == user.codigoEmpresa
                             select r;*/
                var usuario = context.Usuario.Where(x => x.Estado == true && x.Cuenta == user.Cuenta && x.Empresa.Codigo == user.codigoEmpresa).SingleOrDefault<Usuario>();

                //Usuario usuario = result.SingleOrDefault<Usuario>();
                if (usuario != null)
                {
                    if (Encrypt.comparetoCrypt(user.Pass, usuario.Pass))
                        return true;
                }
            }
            return false;
        }

        public UsuarioDTO getUsuarioEnEmpresa(int idEmpresa, int id)
        {
            using (var context = getContext())
            {
                var result = context.Usuario.Where(x => x.IdUsuario == id && x.IdEmpresa == idEmpresa).Select(r => new UsuarioDTO
                {
                    IdUsuario = r.IdUsuario,
                    Nombre = r.Nombre,
                    Email = r.Email,
                    Cuenta = r.Cuenta,
                    Pass = r.Pass,
                    Active = r.Estado,
                    IdRol = r.IdRol,
                    IdCargo = r.IdCargo,
                    NombreRol = r.Rol.Nombre,
                    IdEmpresa = r.IdEmpresa,
                    nombreEmpresa = r.Empresa.Nombre ?? "N/A"
                    //empresa = context.Empresa.Where(x => x.IdEmpresa == r.IdEmpresa).Select(y => new EmpresaDTO { IdEmpresa = y.IdEmpresa, Nombre = y.Nombre ?? "N/A", Estado = y.Estado, Descripcion = y.Descripcion, TipoCambio = y.TipoCambio }).FirstOrDefault()
                }).SingleOrDefault();
                return result;
            }
        }
        public UsuarioDTO getUsuario(int id)
        {
            using (var context = getContext())
            {
                var result = context.Usuario.Where(x => x.IdUsuario == id).Select(r => new UsuarioDTO
                    {
                        IdUsuario = r.IdUsuario,
                        Nombre = r.Nombre,
                        Email = r.Email,
                        Cuenta = r.Cuenta,
                        Pass = r.Pass,
                        Active = r.Estado,
                        IdRol = r.IdRol,
                        IdCargo = r.IdCargo,
                        NombreRol = r.Rol.Nombre,
                        IdEmpresa = r.IdEmpresa,
                        nombreEmpresa = r.Empresa.Nombre ?? "N/A"
                        //empresa = context.Empresa.Where(x => x.IdEmpresa == r.IdEmpresa).Select(y => new EmpresaDTO { IdEmpresa = y.IdEmpresa, Nombre = y.Nombre ?? "N/A", Estado = y.Estado, Descripcion = y.Descripcion, TipoCambio = y.TipoCambio }).FirstOrDefault()
                    }).SingleOrDefault();
                return result;
            }
        }

        public bool update(UsuarioDTO user, string passUser, string passChange, UsuarioDTO currentUser)
        {
            using (var context = getContext())
            {
                try
                {
                    Usuario usuario = context.Usuario.Where(x => x.IdUsuario == user.IdUsuario).SingleOrDefault();
                    if (usuario != null)
                    {
                        //No se podra actualizar el nombre
                        //usuario.Nombre = user.Nombre;
                        usuario.Email = user.Email;
                        usuario.IdRol = user.IdRol;// >= 2 ? user.IdRol : 3;
                        usuario.Cuenta = user.Cuenta;
                        usuario.Estado = user.Active;
                        usuario.IdCargo = user.IdCargo;
                        usuario.IdEmpresa = user.IdEmpresa;
                        if (!String.IsNullOrWhiteSpace(passUser) && !String.IsNullOrWhiteSpace(passChange))
                        {
                            if ((currentUser.IdRol <= 2 || currentUser.IdUsuario == user.IdUsuario)
                                && Encrypt.comparetoCrypt(passUser, currentUser.Pass))
                            {
                                usuario.Pass = Encrypt.GetCrypt(passChange);
                            }
                            else return false;
                        }
                        context.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    //throw e;
                    return false;
                }
                return false;
            }
        }

        public bool recoverPasswordNew(UsuarioDTO user)
        {
            if (user != null)
            {
                string newPassword = Functions.GenerateRandomPassword(10);
                updatePassword(user, newPassword);
                SendMailPassRecovery(user, newPassword);
                return true;
            }
            return false;
        }
        public UsuarioDTO generateTokenRecoverPassword(UsuarioDTO user)
        {
            if(user != null)
            {
                user.Token = updateTokenUser(user.IdUsuario);
                //SendMailResetPassword(user);
                return user;
            }
            return user;
        }
        public bool recoverPassword(string CuentaOEmail)
        {
            using (var context = getContext())
            {
                var result = (from r in context.Usuario
                              where r.Cuenta.Equals(CuentaOEmail) || r.Email.Equals(CuentaOEmail) && r.Estado == true
                              select new UsuarioDTO
                              {
                                  IdUsuario = r.IdUsuario,
                                  Nombre = r.Nombre,
                                  Email = r.Email
                              }).FirstOrDefault();
                if (result != null)
                {
                    string newPassword = Functions.GeneratePassword();
                    updatePassword(result, newPassword);
                    SendMailPassRecovery(result, newPassword);
                    return true;
                }
                else
                    return false;
            }
        }
        public bool updatePassword(UsuarioDTO user, string passChange)
        {
            using (var context = getContext())
            {
                try
                {
                    Usuario usuario = context.Usuario.Where(x => x.IdUsuario == user.IdUsuario).SingleOrDefault();
                    usuario.Pass = Encrypt.GetCrypt(passChange);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    //throw e;
                    return false;
                }
            }
        }

        public bool resetPasswordByTokenAndEmp(UsuarioDTO user)
        {
            using (var context = getContext())
            {
                try
                {
                    var usuario = context.Usuario.Where(x => x.Empresa.Codigo == user.codigoEmpresa && x.Token == user.Token).SingleOrDefault();
                    if(usuario != null)
                    { 
                        usuario.Pass = Encrypt.GetCrypt(user.Pass);
                        usuario.Token = null;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public string updateTokenUser(int id, bool nullToken = false)
        {
            using(var context = getContext())
            {
                try
                {
                    Usuario usuario = context.Usuario.Where(x => x.IdUsuario == id).SingleOrDefault();
                    string tempPassword = usuario.Cuenta + Functions.GenerateRandomPassword(7);
                    //string token = Encrypt.GetCrypt(usuario.Cuenta);
                    string token = Encrypt.GetCrypt(tempPassword);
                    usuario.Token = (nullToken == true) ? null : token;
                    context.SaveChanges();
                    return token;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
        public void SendMailPassRecovery(UsuarioDTO user, string passChange)
        {
            string to = user.Email;
            string subject = "Recuperación de Contraseña";
            string body = "Sr(a). " + user.Nombre + " su contraseña es : " + passChange;
            MailHandler.Send(to, "", subject, body); 
        }
        public void SendMailResetPassword(UsuarioDTO user, string link)
        {
            string to = user.Email;
            string subject = "Resetear contraseña";
            //url.Action("ResetPassword", "Admin", new { rt = token })
            //string resetLink = MailHandler.ResetLink(user.Token, user.codigoEmpresa);

            string body = "Sr(a). " + user.Nombre + ". Si desea resetear su contraseña acceda al link: " + link;
            MailHandler.Send(to, "", subject, body);
        }

        public bool actualizarEmpresaSuperAdmin(int id, int idEmpresa)
        {
            using (var context = getContext())
            {
                try
                {
                    Usuario usuario = context.Usuario.Where(x => x.IdUsuario == id).SingleOrDefault();
                    usuario.IdEmpresa = idEmpresa;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        #region
        public IList<UsuarioDTO> searchResponsables(string busqueda)
        {
            using (var context = getContext())
            {
                return (from r in context.Usuario
                        where r.IdRol != CONSTANTES.SUPER_ADMIN_ROL
                        & r.Nombre.Contains(busqueda)
                        & r.Estado == true
                        select new UsuarioDTO
                        {
                            IdUsuario = r.IdUsuario,
                            Nombre = r.Nombre,
                            Email = r.Email
                        }).ToList();
            }
        }
        #endregion
    }
}
