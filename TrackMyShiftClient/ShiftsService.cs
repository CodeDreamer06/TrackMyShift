using RestSharp;
using Utf8Json;

namespace TrackMyShiftClient;

internal class ShiftsService
{
    private readonly RestClient _client = new("https://localhost:7176/api/Shifts/");

    public ShiftsService()
    {
    }

    internal Shift ReplaceEmptyFields(Shift shift)
    {
        var request = new RestRequest(shift.Id.ToString());
        var oldLog = JsonSerializer.Deserialize<Shift>(_client.Execute(request).Content);

        try
        {
            foreach (var property in from property in shift.GetType().GetProperties()
                                     where property.GetValue(shift) is null
                                     select property)
            {
                property.SetValue(shift, property.GetValue(oldLog));
            }
        }

        catch
        {
            Console.WriteLine("Failed to update shift");
            return new Shift();
        }

        return shift;
    }

    internal int GetAbsoluteId(int id)
    {
        if (id == 0) return 0;
        var request = new RestRequest();
        var response = _client.Execute(request);

        try
        {
            return response.IsSuccessful ?
            JsonSerializer.Deserialize<List<Shift>>(response.Content)[id - 1].Id : 0;
        }

        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("The requested log couldn't be found.");
            return -1;
        }
    }

    internal void Get(int id)
    {
        if (id == -1) return;
        var request = new RestRequest(id != 0 ? id.ToString() : null);
        var response = _client.Execute(request);

        if (response.IsSuccessful)
        {
            var shifts = id == 0 ? JsonSerializer.Deserialize<List<Shift>>(response.Content) : 
                   new List<Shift> { JsonSerializer.Deserialize<Shift>(response.Content) };

            for (int i = 0; i < shifts.Count; i++) shifts[i].Id = i + 1;

            Helpers.DisplayTable(shifts, Helpers.NoResultsMessage);
        }

        else Console.WriteLine(response.ErrorMessage);

    }

    internal void Post(Shift shift)
    {
        if (shift.Id == -1) return;
        var request = new RestRequest("", Method.Post)
            .AddJsonBody(JsonSerializer.ToJsonString(shift));
        _client.Execute(request);

        Console.WriteLine("Successfully logged your shift!");
    }

    internal void Put(Shift shift)
    {
        if (shift.Id == -1) return;

        var request = new RestRequest(shift.Id.ToString(), Method.Put)
            .AddJsonBody(JsonSerializer.ToJsonString(shift));
        _client.Execute(request);

        Console.WriteLine("Successfully updated your shift!");
    }

    internal void Delete(int id)
    {
        if (id == 0)
            Console.WriteLine("Please specify the id of the log you want to delete.");
        
        if (id <= 0) return;

        var request = new RestRequest(id.ToString(), Method.Delete);
        _client.Execute(request);

        Console.WriteLine("Successfully removed your shift!");
    }
}
