
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

[Index(nameof(PlayLength), nameof(PlayerId))]
public class Play
{

    public string Id { get; } = Guid.NewGuid().ToString();
    public string PlayerId { get; set; }
    public string PlayerUserName { get; set; }
    public required string MapId { get; set; }
    public required double PlayLength { get; set; }
    public required DateTime TimeSubmitted { get; set; }
    public Play()
    {
        //USED BY DOTNET
    }

    [method: SetsRequiredMembers]
    public Play(string playerId, string mapId, string username, double playLength)
    {
        Id = Guid.NewGuid().ToString();
        PlayerId = playerId;
        PlayerUserName = username;
        MapId = mapId;
        PlayLength = playLength;
        TimeSubmitted = DateTime.Now;
    }
}