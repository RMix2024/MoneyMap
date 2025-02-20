namespace MoneyMap.Data
    {
    internal class DatabaseConfig
        {
        public static string GetDatabasePath()
            {
            string fileName = "moneymap.db3";
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return System.IO.Path.Combine(folderPath, fileName);

            }

        }
    }
