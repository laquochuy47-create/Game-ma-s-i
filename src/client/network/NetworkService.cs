using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameClient.Network
{
    public class NetworkPacket
    {
        public string Action { get; set; }  // "vote", "chat", "niu", "soi"
        public string Target { get; set; }  
        public string Message { get; set; } 
        public string Sender { get; set; }  
    }

    public class NetworkService
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private bool _isConnected;

        public event Action<NetworkPacket> OnMessageReceived;
        public event Action<string> OnStatusChanged;

        public async Task ConnectAsync(string ip, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                
                NetworkStream stream = _client.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                _isConnected = true;

                OnStatusChanged?.Invoke("Kết nối Server thành công!");

                _ = Task.Run(ListenForServerDataAsync);
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Lỗi kết nối: {ex.Message}");
            }
        }

        private async Task ListenForServerDataAsync()
        {
            try
            {
                string jsonLine;
                while (_isConnected && (jsonLine = await _reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(jsonLine)) continue;

                    NetworkPacket packet = JsonSerializer.Deserialize<NetworkPacket>(jsonLine);
                    if (packet != null)
                    {
                        OnMessageReceived?.Invoke(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Mất kết nối Server: {ex.Message}");
            }
        }

        private async Task SendPacketAsync(string action, string target = "", string message = "")
        {
            if (!_isConnected || _writer == null) return;

            var packet = new NetworkPacket
            {
                Action = action,
                Target = target,
                Message = message
            };

            string jsonString = JsonSerializer.Serialize(packet);
            await _writer.WriteLineAsync(jsonString);
        }

        public async Task SendVoteAsync(string targetPlayer)
        {
            await SendPacketAsync("vote", target: targetPlayer);
        }

        public async Task SendChatAsync(string message)
        {
            await SendPacketAsync("chat", message: message);
        }

        public async Task SendNiuAsync(string targetPlayer)
        {
            await SendPacketAsync("niu", target: targetPlayer);
        }

        public async Task SendSoiAsync(string targetPlayer)
        {
            await SendPacketAsync("soi", target: targetPlayer);
        }

        public void Disconnect()
        {
            _isConnected = false;
            _reader?.Close();
            _writer?.Close();
            _client?.Close();
        }
    }
}