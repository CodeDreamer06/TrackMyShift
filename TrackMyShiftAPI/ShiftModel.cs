using Microsoft.EntityFrameworkCore;

namespace TrackMyShiftAPI;

public class Shift
{
    public int Id { get; set; }

    [Precision(10, 2)]
    public decimal Payment { get; set; }

    [Precision(3)]
    public DateTime Start { get; set; }

    [Precision(3)]
    public DateTime End { get; set; }

    [Precision(10, 2)]
    public decimal Duration { get; set; }
    public string Location { get; set; }
}
