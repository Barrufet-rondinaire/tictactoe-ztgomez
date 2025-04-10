using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace tictactoe;

class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        var participants = await GetParticipants();
        var desqualificats = new HashSet<string>();
        var victòries = new Dictionary<string, int>();

        for (int i = 1; i <= 1000; i++)
        {
            var partida = await GetPartida(i);
            if (partida == null) continue;

            if (desqualificats.Contains(partida.Jugador1) || desqualificats.Contains(partida.Jugador2))
                continue;

            string guanyador = DeterminarGuanyador(partida.Tauler);
            if (guanyador == "jugador1") guanyador = partida.Jugador1;
            else if (guanyador == "jugador2") guanyador = partida.Jugador2;
            else continue;

            if (!victòries.ContainsKey(guanyador))
                victòries[guanyador] = 0;
            victòries[guanyador]++;
        }

        string guanyadorFinal = null;
        int maxVictòries = 0;
        foreach (var nose in victòries)
        {
            if (nose.Value > maxVictòries)
            {
                maxVictòries = nose.Value;
                guanyadorFinal = nose.Key;
            }
        }

        Console.WriteLine("Participants:");
        foreach (var nose in participants)
        {
            Console.WriteLine($"- {nose.Key} ({nose.Value})");
        }

        if (guanyadorFinal != null)
        {
            string pais = participants.ContainsKey(guanyadorFinal) ? participants[guanyadorFinal] : "país desconegut";
            Console.WriteLine($"\nGuanyador: {guanyadorFinal} ({pais}) amb {maxVictòries} victòries.");
        }
        else
        {
            Console.WriteLine("\nNo hi ha cap guanyador.");
        }


    }

    static async Task<Dictionary<string, string>> GetParticipants()
    {
        var resultat = new Dictionary<string, string>();
        var text = await client.GetStringAsync("http://localhost:8080/jugadors");

        string pattern = @"participant\s+([A-Z][a-zA-ZÀ-ÿ']+\s+[A-Z][a-zA-ZÀ-ÿ']+).*?(?:representant de|representa a)\s+([A-Z][a-zA-ZÀ-ÿ']+)";
        var matches = Regex.Matches(text, pattern);

        foreach (Match m in matches)
        {
            string nom = m.Groups[1].Value.Trim();
            string pais = m.Groups[2].Value.Trim();
            if (!resultat.ContainsKey(nom))
                resultat[nom] = pais;
        }

        return resultat;
    }

    static async Task<Partida> GetPartida(int numero)
    {
        try
        {
            var resposta = await client.GetStringAsync($"http://localhost:8080/partida/{numero}");
            var partida = JsonSerializer.Deserialize<Partida>(resposta);
            if (partida?.Tauler == null || partida.Tauler.Count < 3)
                return null;
            return partida;
        }
        catch
        {
            return null;
        }
    }

    static string DeterminarGuanyador(List<string> tauler)
    {
        var files = tauler;
        var columnes = new List<string> {
            $"{tauler[0][0]}{tauler[1][0]}{tauler[2][0]}",
            $"{tauler[0][1]}{tauler[1][1]}{tauler[2][1]}",
            $"{tauler[0][2]}{tauler[1][2]}{tauler[2][2]}"
        };
        var diagonals = new List<string> {
            $"{tauler[0][0]}{tauler[1][1]}{tauler[2][2]}",
            $"{tauler[0][2]}{tauler[1][1]}{tauler[2][0]}"
        };

        foreach (var linia in files.Concat(columnes).Concat(diagonals))
        {
            if (linia == "XXX") return "jugador2";
            if (linia == "000") return "jugador1";
        }

        return null;
    }
}
