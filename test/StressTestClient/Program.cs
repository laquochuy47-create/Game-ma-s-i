using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;

namespace StressTestClient
{
    class Program
    {
        private const int BotCount = 7;

        private static int connectedCount = 0;
        private static int sentCount = 0;
        private static int receivedCount = 0;
        private static int failedCount = 0;

        // Chờ đủ 7 bot kết nối trước khi gửi dữ liệu
        private static readonly TaskCompletionSource<bool> allBotsConnected =
            new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== WEREWOLF SERVER STRESS TEST ===");
            Console.WriteLine($"Số lượng bot: {BotCount}");
            Console.WriteLine();

            var botTasks = new List<Task>();

            // Tạo 7 bot
            for (int i = 1; i <= BotCount; i++)
            {
                botTasks.Add(RunBotAsync(i));
            }

            // Chờ tất cả bot kết nối
            await Task.WhenAll(botTasks);

            Console.WriteLine();
            Console.WriteLine("=== KẾT QUẢ STRESS TEST ===");
            Console.WriteLine("Kết nối thành công: " + connectedCount + "/" + BotCount);
            Console.WriteLine("Gửi CHAT thành công: " + sentCount + "/" + BotCount);
            Console.WriteLine("Tổng broadcast nhận: " + receivedCount);
            Console.WriteLine("Số bot thất bại: " + failedCount);

            if (connectedCount == BotCount && failedCount == 0)
            {
                Console.WriteLine();
                Console.WriteLine("STRESS TEST: PASSED");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("STRESS TEST: FAILED");
            }

            Console.WriteLine();
            Console.WriteLine("Nhấn phím Enter để thoát...");
            Console.ReadLine();
        }

        static async Task RunBotAsync(int botId)
        {
            string serverIp = "127.0.0.1";
            int serverPort = 8080;

            try
            {
                using TcpClient client = new TcpClient();

                Console.WriteLine($"[Bot {botId}] Đang kết nối tới server...");

                await client.ConnectAsync(serverIp, serverPort);

                Console.WriteLine($"[Bot {botId}] Kết nối thành công!");

                // Tăng số lượng bot kết nối thành công
                int currentConnectedCount = Interlocked.Increment(ref connectedCount);

                // Nếu đây là bot cuối cùng kết nối thì cho tất cả các bot bắt đầu gửi dữ liệu
                if (currentConnectedCount == BotCount)
                {
                    Console.WriteLine();
                    Console.WriteLine("[SYSTEM] Tất cả bot đã kết nối thành công. Bắt đầu gửi dữ liệu...");
                    Console.WriteLine();

                    allBotsConnected.TrySetResult(true);
                }

                // Bot nào connect sớm phải chờ các bot còn lại
                await allBotsConnected.Task;

                NetworkStream stream = client.GetStream();

                using StreamWriter writer = new StreamWriter(
                    stream,
                    new UTF8Encoding(false),
                    leaveOpen: true)
                {
                    AutoFlush = true
                };

                using StreamReader reader = new StreamReader(
                    stream,
                    Encoding.UTF8,
                    leaveOpen: true);

                // Tạo packet riên cho từng bot
                var packet = new GamePacket
                {
                    Action = "CHAT",
                    SenderId = $"Bot {botId}",
                    Payload = $"Hello from Bot {botId}!"
                };

                string json = GamePacket.Serialize(packet);

                // Gửi packet tới server
                await writer.WriteLineAsync(json);

                Interlocked.Increment(ref sentCount);

                Console.WriteLine($"[Bot {botId}] Đã gửi tin nhắn tới server: " + packet.Payload);

                /*
                 * Mỗi bot gửi 1 CHAT 
                 * Server sẽ broadcast lại cho tất cả bot
                 * Vì vậy mỗi bot sẽ nhận được 7 broadcast (bao gồm cả tin nhắn của chính nó)
                 */
                for (int i = 0; i < BotCount; i++)
                {
                    // Timeout 5 giây để nhận phản hồi từ server
                    string? responseJson = await reader
                        .ReadLineAsync()
                        .WaitAsync(TimeSpan.FromSeconds(5));
                    if (responseJson == null)
                    {
                        throw new Exception("Server đã đóng kết nối.");
                    }

                    // Loại bỏ UTF-8 BOM nếu có
                    responseJson = responseJson.TrimStart('\uFEFF');

                    GamePacket response = GamePacket.Deserialize(responseJson);

                    Interlocked.Increment(ref receivedCount);

                    Console.WriteLine(
                        $"[Bot {botId}] Nhận: " +
                        $"Payload={response.Payload}" +
                        $"SenderId={response.SenderId}, ");
                }

                Console.WriteLine($"[Bot {botId}] Hoàn thành.");
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref failedCount);

                Console.WriteLine($"[Bot {botId}] Kết nối thất bại: " + ex.Message);
            }
        }
    }
}

