using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MoneyMap.Data
{
    public class DatabaseConfig
    {
        public static string GetDatabasePath()
        {
            string fileName = "moneymap.db3";
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folderPath, fileName);
        }

    }
}
