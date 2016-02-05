using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NubeBooks.Helpers;

namespace NubeBooks.Core.BL
{
    public class Base
    {
        protected LibrosDBEntities getContext()
        {
            return new LibrosDBEntities();
        }
    }
}
