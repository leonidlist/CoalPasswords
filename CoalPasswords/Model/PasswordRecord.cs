namespace CoalPasswords
{
    /// <summary>
    /// Реализует интерфейс IRecord. Хранит в себе данные о записи пароля
    /// </summary>
    class PasswordRecord : IRecord
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Website { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public Color CardColor { get; set; }
    }
}
