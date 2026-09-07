using System;
using System.Text; 
using System.Threading.Tasks;

namespace WerewolfServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Bật bộ gõ UTF-8 cho cửa sổ Console
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== WEREWOLF SERVER ===");
            Server server = new Server();
            await server.StartAsync(8080);
        }
    }
}