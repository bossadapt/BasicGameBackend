
using System.Diagnostics.CodeAnalysis;

[method: SetsRequiredMembers]
public class StrippedPlayer(string username)
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public required string Username { get; set; } = username;
    public DateTime AccountCreated { get; set; } = DateTime.Now;
    public DateTime LastLoggedIn { get; set; } = DateTime.Now;
}