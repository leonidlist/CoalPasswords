namespace CoalPasswords
{
    /// <summary>
    /// Интерфейс для записи пароля.
    /// </summary>
    interface IRecord
    {
        string Title { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Website { get; set; }
        int Category { get; set; }
        string ImageUrl { get; set; }
        int UniqueId { get; set; }
        Color CardColor { get; set; }
    }
}
