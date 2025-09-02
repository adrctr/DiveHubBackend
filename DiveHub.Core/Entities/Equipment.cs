using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveHub.Core.Entities
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = null!;

        public ICollection<Dive> Dives { get; set; } = [];
    }
}
