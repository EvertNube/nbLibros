using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Core.DTO
{
    [Serializable]
    public class DateStartEndDTO
    {
        public DateTime today { get; set; }
        public DateTime firstDayYear { get; set; }
        public DateTime lastDayYear { get; set; }
        /*public DateTime firstDayMonth { get; set; }
        public DateTime lastDayMonth { get; set; }
        public DateTime firstDayMonthYA { get; set; }
        public DateTime lastDayMonthYA { get; set; }*/
    }
}
