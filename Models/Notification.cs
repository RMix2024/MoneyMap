using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMap.Models
{
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }

}
