using RestSharp;
using Utf8Json;

namespace TrackMyShiftClient;

internal class ShiftsService
{
    private RestClient _client = new("https://localhost:5001/api/Shifts/");

    public ShiftsService()
    {
    }

    internal void Get(int id)
    {
        var request = new RestRequest(id != 0 ? id.ToString() : null);
        var response = _client.Execute(request);
        
        if (response.IsSuccessful)
        {
            var shifts = JsonSerializer.Deserialize<List<Shift>>(response.Content);
            for (int i = 0; i < shifts.Count; i++) shifts[i].Id = i + 1;

            Helpers.DisplayTable(shifts, Helpers.NoResultsMessage);
        }

        else Console.WriteLine(response.ErrorMessage);

    }

    internal void Post(Shift shift)
    {
        var request = new RestRequest("", Method.Post)
            .AddJsonBody(JsonSerializer.ToJsonString(shift));
        _client.Execute(request);

        Console.WriteLine("Successfully logged your shift!");
    }

    internal void Put(Shift shift)
    {
        var request = new RestRequest(shift.Id.ToString(), Method.Put)
            .AddJsonBody(JsonSerializer.ToJsonString(shift));
        _client.Execute(request);

        Console.WriteLine("Successfully updated your shift!");
    }

    internal void Delete(int id)
    {
        var request = new RestRequest(id.ToString(), Method.Delete);
        _client.Execute(request);

        Console.WriteLine("Successfully removed your shift!");
    }
}
