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

        private static readonly TaskCompletionSource<bool> allBotsConnected =
            new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("=== WEREWOLF SERVER STRESS TEST ===");
            Console.WriteLine($"Số lượng bot: {BotCount}");
            Console.WriteLine();

            var botTasks = new List<Task>();

            // Tạo 7 bot chạy đồng thời
            for (int i = 1; i <= BotCount; i++)
            {
                botTasks.Add(RunBotAsync(i));
            }

            // Chờ tất cả bot hoàn thành
            await Task.WhenAll(botTasks);

            Console.WriteLine();
            Console.WriteLine("=== KẾT QUẢ STRESS TEST ===");
            Console.WriteLine(
                $"Kết nối thành công: {connectedCount}/{BotCount}");
            Console.WriteLine(
                $"Gửi CHAT thành công: {sentCount}/{BotCount}");
            Console.WriteLine(
                $"Tổng broadcast nhận: {receivedCount}");
            Console.WriteLine(
                $"Số bot thất bại: {failedCount}");

            bool passed =
                connectedCount == BotCount &&
                sentCount == BotCount &&
                receivedCount == BotCount * BotCount &&
                failedCount == 0;

            Console.WriteLine();

            if (passed)
            {
                Console.WriteLine("STRESS TEST: PASSED");
            }
            else
            {
                Console.WriteLine("STRESS TEST: FAILED");
            }

            // Chạy thủ công: chờ Enter.
            // Chạy automation với --ci: tự thoát.
            if (!Array.Exists(args, arg => arg == "--ci"))
            {
                Console.WriteLine();
                Console.WriteLine("Nhấn phím Enter để thoát...");
                Console.ReadLine();
            }

            // CI dùng exit code để biết PASS / FAIL
            return passed ? 0 : 1;
        }

        static async Task RunBotAsync(int botId)
        {
            string serverIp = "127.0.0.1";
            int serverPort = 8080;

            try
            {
                using TcpClient client = new TcpClient();

                Console.WriteLine(
                    $"[Bot {botId}] Đang kết nối tới server...");

                await client.ConnectAsync(serverIp, serverPort);

                Console.WriteLine(
                    $"[Bot {botId}] Kết nối thành công!");

                int currentConnectedCount =
                    Interlocked.Increment(ref connectedCount);

                // Khi bot thứ 7 kết nối thì cho tất cả bắt đầu
                if (currentConnectedCount == BotCount)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "[SYSTEM] Tất cả bot đã kết nối thành công. " +
                        "Bắt đầu gửi dữ liệu...");
                    Console.WriteLine();

                    allBotsConnected.TrySetResult(true);
                }

                // Bot kết nối sớm chờ các bot còn lại
                await allBotsConnected.Task
                    .WaitAsync(TimeSpan.FromSeconds(10));

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

                var packet = new GamePacket
                {
                    Action = "CHAT",
                    SenderId = $"Bot {botId}",
                    Payload = $"Hello from Bot {botId}!"
                };

                string json = GamePacket.Serialize(packet);

                await writer.WriteLineAsync(json);

                Interlocked.Increment(ref sentCount);

                Console.WriteLine(
                    $"[Bot {botId}] Đã gửi tin nhắn tới server: " +
                    packet.Payload);

                // 7 bot gửi 7 CHAT.
                // Mỗi bot phải nhận đủ 7 broadcast.
                for (int i = 0; i < BotCount; i++)
                {
                    string? responseJson = await reader
                        .ReadLineAsync()
                        .WaitAsync(TimeSpan.FromSeconds(5));

                    if (responseJson == null)
                    {
                        throw new Exception(
                            "Server đã đóng kết nối.");
                    }

                    // Xử lý BOM do Server có thể gửi kèm
                    responseJson =
                        responseJson.TrimStart('\uFEFF');

                    GamePacket response =
                        GamePacket.Deserialize(responseJson);

                    Interlocked.Increment(ref receivedCount);

                    Console.WriteLine(
                        $"[Bot {botId}] Nhận: " +
                        $"Payload={response.Payload} " +
                        $"SenderId={response.SenderId}");
                }

                Console.WriteLine(
                    $"[Bot {botId}] Hoàn thành.");
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref failedCount);

                Console.WriteLine(
                    $"[Bot {botId}] THẤT BẠI: {ex.Message}");
            }
        }
    }
}
