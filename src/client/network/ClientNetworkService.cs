using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client
{
    public class NetworkPacket
    {
        public string Action { get; set; } = "";
        public string Target { get; set; } = "";
        public string Message { get; set; } = "";
        public string Sender { get; set; } = "";
    }

    public class NetworkService
    {
        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private bool _isConnected;

        public event Action<NetworkPacket>? OnMessageReceived;
        public event Action<string>? OnStatusChanged;
        public event Action? OnDisconnected;

        public bool IsConnected => _isConnected && _client != null && _client.Connected;

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
                Disconnect();
                OnStatusChanged?.Invoke($"Lỗi kết nối: {ex.Message}");
                throw;
            }
        }

        private async Task ListenForServerDataAsync()
        {
            try
            {
                while (_isConnected && _reader != null)
                {
                    string? jsonLine = await _reader.ReadLineAsync();
                    if (jsonLine == null) break;
                    if (string.IsNullOrWhiteSpace(jsonLine)) continue;

                    NetworkPacket? packet = JsonSerializer.Deserialize<NetworkPacket>(jsonLine);
                    if (packet != null)
                    {
                        OnMessageReceived?.Invoke(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Mất kết nối: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        public async Task SendPacketAsync(string action, string target = "", string message = "")
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

        public async Task SendChatAsync(string message) => await SendPacketAsync("chat", message: message);
        public async Task SendVoteAsync(string targetPlayer) => await SendPacketAsync("vote", target: targetPlayer);
        public async Task SendSkillAsync(string targetPlayer) => await SendPacketAsync("skill", target: targetPlayer);

        public void Disconnect()
        {
            if (!_isConnected) return;
            _isConnected = false;
            _reader?.Close();
            _writer?.Close();
            _client?.Close();
            _client = null;
            OnDisconnected?.Invoke();
        }
    }
}