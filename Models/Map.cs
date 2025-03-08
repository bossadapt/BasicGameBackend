
using System.Diagnostics.CodeAnalysis;
public class Map
{
    public required string Id { get; set; }
    public double AuthorTime { get; set; }
    public double SPlusTime { get; set; }
    public double STime { get; set; }
    public double ATime { get; set; }
    public double BTime { get; set; }
    public ICollection<Play> Leaderboard { get; set; } = [];

    public Map() { }  // Required by EF Core
[method: SetsRequiredMembers]
    public Map(string mapId, double authorTime, double sPlusTime, double sTime, double aTime, double bTime)
    {
        Id = mapId;
        AuthorTime = authorTime;
        SPlusTime = sPlusTime;
        STime = sTime;
        ATime = aTime;
        BTime = bTime;
    }
}


class MapsComparer : EqualityComparer<Map>
{
    public override bool Equals(Map? x, Map? y)
    {
        if(x == null || y == null){
            return false;
        }
        return x.Id == y.Id;
    }

    public override int GetHashCode([DisallowNull] Map obj)
    {
        return obj.Id.GetHashCode();
    }
}