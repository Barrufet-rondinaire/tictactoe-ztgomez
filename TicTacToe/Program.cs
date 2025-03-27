namespace TicTacToe;
class Program
{
    static async Task Main()
    {
        using (var client = new HttpClient())
        {
            var r = await client.GetAsync(
                "http://localhost:8080/"
            );
            
        }

    }
}