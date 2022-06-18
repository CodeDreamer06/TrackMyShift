namespace TrackMyShiftClient;

public class Shift
{
    public int Id { get; set; }
    public decimal Payment { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal Duration { get; set; }
    public string Location { get; set; }

    public override string ToString() => $"{Id} {Payment} {Start} {End} {Duration} {Location}";
}
