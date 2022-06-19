using RestSharp;
using Utf8Json;

namespace TrackMyShiftClient;

internal class ShiftsService
{
    private RestClient _client = new("https://localhost:7176/api/Shifts/");

    public ShiftsService()
    {
    }

    internal Shift ReplaceEmptyFields(Shift shift)
    {
        var request = new RestRequest(shift.Id != 0 ? shift.Id.ToString() : null);
        var oldLog = JsonSerializer.Deserialize<Shift>(_client.Execute(request).Content);

        foreach (var property in from property in shift.GetType().GetProperties()
                                 where property.GetValue(shift) is null
                                 select property)
        {
            property.SetValue(shift, property.GetValue(oldLog));
        }

        return shift;
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
