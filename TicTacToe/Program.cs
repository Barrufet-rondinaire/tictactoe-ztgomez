using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TicTacToe;

class TicTacToeME
{
    private static readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:8080/") };
    private static Dictionary<string, int> _victories = new();
    private static HashSet<string> _eliminatedPlayers = new();

    static async Task Main()
    {
        Console.WriteLine("Recuperant la llista de participants...");
        var participants = await GetParticipants();
        
        Console.WriteLine("Filtrant participants desqualificats...");
        await ();
        
        Console.WriteLine("Analitzant resultats de les partides...");
        await ();
        
        Console.WriteLine("Determinant el guanyador del torneig...");
        string winner = ();
        Console.WriteLine($"El guanyador és: {winner}");
    }

    private static async Task<Dictionary<string, string>> GetParticipants()
    {
        var response = await _client.GetFromJsonAsync<List<Participant>>("/participants");
        var participants = new Dictionary<string, string>();

        foreach (var participant in response)
        {
            participants[participant.Name] = participant.Country;
        }
        return participants;
    }

    
}


