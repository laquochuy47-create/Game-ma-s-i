using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;

namespace WerewolfClient
{
    public class ClientNetworkService
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cts;
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        public event Action<GamePacket> OnPacketReceived;
        public event Action OnDisconnected;

        public bool IsConnected => _client != null && _client.Connected;

        public async Task ConnectAsync(string ip, int port, int timeoutMs = 5000)
        {
            try
            {
                _client = new TcpClient();

                using var connectCts = new CancellationTokenSource(timeoutMs);
                await _client.ConnectAsync(ip, port, connectCts.Token);

                var stream = _client.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                _cts = new CancellationTokenSource();
                _ = ReceiveLoopAsync(_cts.Token);
            }
            catch (Exception ex)
            {
                Disconnect();
                Console.WriteLine($"[Network] Lỗi kết nối: {ex.Message}");
                throw;
            }
        }

        public async Task SendActionAsync(string actionStr, string payload = "")
        {
            if (!IsConnected || _writer == null) return;

            await _sendLock.WaitAsync();
            try
            {
                var packet = new GamePacket { Action = actionStr, Payload = payload };
                string json = GamePacket.Serialize(packet);
                await _writer.WriteLineAsync(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Network] Lỗi khi gửi: {ex.Message}");
                Disconnect();
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public async Task SendChatAsync(string message)
        {
            await SendActionAsync("CHAT", message);
        }

        public void Disconnect()
        {
            if (_client == null) return;

            try
            {
                _cts?.Cancel();
                _reader?.Dispose();
                _writer?.Dispose();
                _client?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Network] Lỗi khi đóng socket: {ex.Message}");
            }
            finally
            {
                _client = null;
                _reader = null;
                _writer = null;
                OnDisconnected?.Invoke();
            }
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && IsConnected)
                {
                    string json = await _reader.ReadLineAsync();

                    if (json == null) break;

                    if (string.IsNullOrWhiteSpace(json)) continue;

                    var packet = GamePacket.Deserialize(json);
                    if (packet != null)
                    {
                        OnPacketReceived?.Invoke(packet);
                    }
                }
            }
            catch (Exception ex) when (ex is IOException or ObjectDisposedException or OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Network] Lỗi nhận dữ liệu: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
