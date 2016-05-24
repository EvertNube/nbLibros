using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NubeBooks.Models
{
    [Serializable]
    public class InfoSItem
    {
        public int total_count { get; set; }
        public List<sItem> items { get; set; }
    }
}