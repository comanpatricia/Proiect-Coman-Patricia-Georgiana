using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class Designer
    {
        public int ID { get; set; }
        public string DesignerName { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
