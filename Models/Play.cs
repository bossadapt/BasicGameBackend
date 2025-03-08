
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

[method: SetsRequiredMembers]
[Index(nameof(PlayLength), nameof(PlayerId), AllDescending = false)]
public class Play(string playerId,string mapId, double playLength)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string PlayerId { get; set;} = playerId;
    public required string MapId { get; set; } = mapId;
    public required double PlayLength { get; set; } = playLength;
    public required DateTime TimeSubmitted { get; set; } = DateTime.Now;
}