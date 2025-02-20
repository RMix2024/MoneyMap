using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMap.Models
{
    public class LogEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Action { get; set; } = string.Empty; // e.g., "Added Transaction", "Deleted Category"

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
