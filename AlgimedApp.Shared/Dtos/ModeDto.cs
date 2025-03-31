using AlgimedApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Shared.Dtos
{
    public class ModeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaxBottleNumber { get; set; }
        public int MaxUsedTips { get; set; }

        public List<Step> Steps { get; set; } = new();
    }
}