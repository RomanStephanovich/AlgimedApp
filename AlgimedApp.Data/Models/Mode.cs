using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Data.Models
{
    public class Mode
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxBottleNumber { get; set; }
        public int MaxUsedTips { get; set; }

        public List<Step> Steps { get; set; } = new();
    }
}
