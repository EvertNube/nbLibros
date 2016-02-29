using NubeBooks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.Logistics.BL
{
    public class Base
    {
        protected LibrosDBEntities getContext()
        {
            return new LibrosDBEntities();
        }
    }
}
