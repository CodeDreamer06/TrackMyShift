using System.Runtime.Serialization;

namespace TrackMyShiftClient;

public class Shift
{
    [DataMember(Name = "id")]
    public int Id { get; set; }

    [DataMember(Name = "payment")]
    public decimal? Payment { get; set; }

    [DataMember(Name = "start")]
    public DateTime? Start { get; set; }

    [DataMember(Name = "end")]
    public DateTime? End { get; set; }

    [DataMember(Name = "duration")]
    public decimal? Duration { get; set; }

    [DataMember(Name = "location")]
    public string? Location { get; set; }

    public override string ToString() => $"{Id} {Payment} {Start} {End} {Duration} {Location}";
}
