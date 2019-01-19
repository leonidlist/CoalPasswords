namespace CoalPasswords
{
    interface IRecord
    {
        string Title { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Website { get; set; }
    }
}
