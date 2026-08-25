using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace WerewolfServer
{
    public class Server
    {
        private TcpListener _listener;
        // Quản lý session người chơi (T08 chuẩn bị cho Sprint 2)
        private ConcurrentDictionary<string, TcpClient> _clients = new ConcurrentDictionary<string, TcpClient>();

        public async Task StartAsync(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            Console.WriteLine($"[Server] Đang lắng nghe kết nối tại cổng {port}...");

            while (true)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                // Không await ở đây để không block việc nhận client khác (T03)
                _ = HandleClientAsync(tcpClient);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            string clientId = Guid.NewGuid().ToString().Substring(0, 8);
            _clients.TryAdd(clientId, client);
            Console.WriteLine($"[Kết nối mới] ID: {clientId}");

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);

            try
            {
                while (client.Connected)
                {
                    string json = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(json)) break;

                    var packet = GamePacket.Deserialize(json);
                    packet.SenderId = clientId;
                    Console.WriteLine($"[Nhận] Action: {packet.Action} từ {clientId}");

                    // T05: Xử lý luồng Chat cơ bản (Broadcast)
                    if (packet.Action == "CHAT")
                    {
                        await BroadcastAsync(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Lỗi] ID: {clientId} - {ex.Message}");
            }
            finally
            {
                _clients.TryRemove(clientId, out _);
                client.Close();
                Console.WriteLine($"[Ngắt kết nối] ID: {clientId}");
            }
        }

        private async Task BroadcastAsync(GamePacket packet)
        {
            string json = GamePacket.Serialize(packet);
            foreach (var client in _clients.Values)
            {
                try
                {
                    var writer = new StreamWriter(client.GetStream(), Encoding.UTF8) { AutoFlush = true };
                    await writer.WriteLineAsync(json);
                }
                catch { /* Bỏ qua client bị đứt kết nối (T22) */ }
            }
        }
    }
}
