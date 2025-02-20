namespace MoneyMap.Models
    {
    public class User
        {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty; // For security

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
