using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace WerewolfClient
{
    public class ClientNetworkService
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        
        // Event để bắn dữ liệu ra UI khi nhận được message
        public event Action<GamePacket> OnPacketReceived;

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            
            var stream = _client.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            
            // Chạy luồng lắng nghe tin nhắn từ Server
            _ = ReceiveLoopAsync();
        }

        public async Task SendChatAsync(string message)
        {
            var packet = new GamePacket { Action = "CHAT", Payload = message };
            string json = GamePacket.Serialize(packet);
            await _writer.WriteLineAsync(json);
        }

        private async Task ReceiveLoopAsync()
        {
            try
            {
                while (_client.Connected)
                {
                    string json = await _reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        var packet = GamePacket.Deserialize(json);
                        OnPacketReceived?.Invoke(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mất kết nối tới server.");
            }
        }
    }
}
