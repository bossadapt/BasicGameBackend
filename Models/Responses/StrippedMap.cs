
using System.Diagnostics.CodeAnalysis;
public class StrippedMap
{
    public required string Id { get; set; }
    public double AuthorTime { get; set; }
    public double SPlusTime { get; set; }
    public double STime { get; set; }
    public double ATime { get; set; }
    public double BTime { get; set; }

    public StrippedMap() { }  // Required by EF Core
[method: SetsRequiredMembers]
    public StrippedMap(string mapId, double authorTime, double sPlusTime, double sTime, double aTime, double bTime)
    {
        Id = mapId;
        AuthorTime = authorTime;
        SPlusTime = sPlusTime;
        STime = sTime;
        ATime = aTime;
        BTime = bTime;
    }
}


