using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class SearchModel : PageModel
{
    public required List<string> Results { get; set; }

    public async Task OnGetAsync(string query)
    {
        if (!string.IsNullOrEmpty(query))
        {
            // API URL para buscar equipos en TheSportsDB
            string apiKey = "3"; // Reemplaza con tu API Key
            string apiUrl = $"https://www.thesportsdb.com/api/v1/json/{apiKey}/searchteams.php?t={query}";

            using var client = new HttpClient();
            var response = await client.GetStringAsync(apiUrl);

            // Procesar la respuesta de la API
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jsonResponse = JsonSerializer.Deserialize<ApiResponse>(response, options);

            // Transformar los resultados en una lista de nombres de equipos
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Results = jsonResponse.Teams.Select(team => team.StrTeam).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }

    private class ApiResponse
    {
        public required List<Team> Teams { get; set; }
    }

    private class Team
    {
        public required string StrTeam { get; set; }
    }
}
