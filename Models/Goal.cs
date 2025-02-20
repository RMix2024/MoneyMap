using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMap.Models
{
    public class Goal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; } = 0;

        public DateTime TargetDate { get; set; }
    }

}
