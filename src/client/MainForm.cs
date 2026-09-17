using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedLibrary;

namespace WerewolfClient
{
    public class MainForm : Form
    {
        private ClientNetworkService _networkService;
        private string _myRole = "VILLAGER";

        private TextBox txtIP;
        private TextBox txtPort;
        private TextBox txtUsername;
        private Button btnConnect;
        private Label lblStatus;

        private Label lblRole;
        private Label lblTimer;

        private ListBox lstPlayers;
        private RichTextBox txtChatLog;
        private TextBox txtInputChat;
        private Button btnSendChat;

        private Button btnVote;
        private Button btnSoi;
        private Button btnCan;

        public MainForm()
        {
            InitializeComponentLayout();

            _networkService = new ClientNetworkService();
            _networkService.OnPacketReceived += HandleServerPacket;
            _networkService.OnDisconnected += HandleDisconnected;
        }

        private void UpdateStatus(string message, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(message, color)));
                return;
            }
            lblStatus.Text = $"Trạng thái: {message}";
            lblStatus.ForeColor = color;
        }

        private void HandleServerPacket(GamePacket packet)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleServerPacket(packet)));
                return;
            }

            switch (packet.Action.ToUpper())
            {
                case "CHAT":
                    txtChatLog.AppendText($"[Trò chuyện]: {packet.Payload}\r\n");
                    break;

                case "SYSTEM":
                case "SYSTEM_ANNOUNCE":
                    txtChatLog.AppendText($"[HỆ THỐNG]: {packet.Payload}\r\n");
                    break;

                case "TIMER":
                    lblTimer.Text = $"⏱ {packet.Payload}s";
                    break;

                case "UPDATE_PLAYERS":
                    lstPlayers.Items.Clear();
                    var players = packet.Payload.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in players)
                    {
                        lstPlayers.Items.Add(p.Trim());
                    }
                    break;

                case "ROLE_ASSIGN":
                    _myRole = packet.Payload.Trim().ToUpper();
                    lblRole.Text = $"Vai trò: {_myRole}";
                    break;

                case "PHASE_DAY":
                    txtChatLog.AppendText("[HỆ THỐNG]: ☀️ Trời sáng! Bắt đầu thảo luận và Vote treo cổ.\r\n");
                    btnVote.Enabled = true;
                    btnSoi.Enabled = false;
                    btnCan.Enabled = false;
                    break;

                case "PHASE_NIGHT":
                    txtChatLog.AppendText("[HỆ THỐNG]: 🌙 Trời tối! Mọi người đi ngủ, các chức năng thức giấc.\r\n");
                    btnVote.Enabled = false;
                    btnSoi.Enabled = (_myRole == "SEER");
                    btnCan.Enabled = (_myRole == "WEREWOLF");
                    break;

                case "SOI_RESULT":
                    MessageBox.Show(packet.Payload, "Kết quả Tiên Tri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    txtChatLog.AppendText($"[{packet.Action}]: {packet.Payload}\r\n");
                    break;
            }
            txtChatLog.ScrollToCaret();
        }

        private void HandleDisconnected()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(HandleDisconnected));
                return;
            }

            UpdateStatus("Đã ngắt kết nối máy chủ.", Color.Red);
            btnConnect.Enabled = true;
            btnSendChat.Enabled = false;
            btnVote.Enabled = false;
            btnSoi.Enabled = false;
            btnCan.Enabled = false;
        }

        private async void BtnConnect_Click(object? sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Cổng Port không hợp lệ!");
                return;
            }

            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên người chơi!");
                return;
            }

            btnConnect.Enabled = false;
            UpdateStatus("Đang kết nối...", Color.Orange);

            try
            {
                await _networkService.ConnectAsync(ip, port);
                UpdateStatus("Đã kết nối thành công!", Color.Green);

                await _networkService.SendActionAsync("JOIN", username);
                btnSendChat.Enabled = true;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Lỗi: {ex.Message}", Color.Red);
                btnConnect.Enabled = true;
            }
        }

        private async void BtnSendChat_Click(object? sender, EventArgs e)
        {
            string msg = txtInputChat.Text.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                await _networkService.SendChatAsync(msg);
                txtInputChat.Clear();
            }
        }

        private async void BtnVote_Click(object? sender, EventArgs e)
        {
            if (lstPlayers.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhấp chọn một người chơi trong danh sách để Vote!");
                return;
            }
            await _networkService.SendActionAsync("VOTE", lstPlayers.SelectedItem.ToString());
        }

        private async void BtnSoi_Click(object? sender, EventArgs e)
        {
            if (lstPlayers.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhấp chọn một người chơi trong danh sách để Soi!");
                return;
            }
            await _networkService.SendActionAsync("SOI", lstPlayers.SelectedItem.ToString());
        }

        private async void BtnCan_Click(object? sender, EventArgs e)
        {
            if (lstPlayers.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhấp chọn một người chơi trong danh sách để Cắn!");
                return;
            }
            await _networkService.SendActionAsync("CAN", lstPlayers.SelectedItem.ToString());
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Ma Sói Client - Bàn Chơi Tổng Hợp";
            this.Size = new Size(720, 560);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblIpTitle = new Label { Location = new Point(15, 15), Size = new Size(50, 20), Text = "Server:" };
            txtIP = new TextBox { Location = new Point(65, 12), Size = new Size(95, 23), Text = "127.0.0.1" };

            Label lblPortTitle = new Label { Location = new Point(165, 15), Size = new Size(35, 20), Text = "Port:" };
            txtPort = new TextBox { Location = new Point(200, 12), Size = new Size(50, 23), Text = "8888" };

            Label lblUserTitle = new Label { Location = new Point(255, 15), Size = new Size(35, 20), Text = "Tên:" };
            txtUsername = new TextBox { Location = new Point(290, 12), Size = new Size(80, 23), Text = "Player1" };

            btnConnect = new Button { Location = new Point(380, 10), Size = new Size(75, 26), Text = "Kết nối" };
            lblRole = new Label { Location = new Point(470, 15), Size = new Size(130, 20), Text = "Vai trò: Chưa có", Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            lblTimer = new Label { Location = new Point(610, 12), Size = new Size(80, 25), Text = "⏱ --s", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DarkRed };

            lblStatus = new Label { Location = new Point(15, 42), Size = new Size(670, 20), Text = "Trạng thái: Chưa kết nối", ForeColor = Color.DarkGray };

            Label lblListTitle = new Label { Location = new Point(15, 68), Size = new Size(180, 18), Text = "Người chơi trong phòng:" };
            lstPlayers = new ListBox { Location = new Point(15, 88), Size = new Size(180, 360) };

            txtChatLog = new RichTextBox { Location = new Point(205, 88), Size = new Size(485, 320), ReadOnly = true, BackColor = Color.White };

            txtInputChat = new TextBox { Location = new Point(205, 420), Size = new Size(395, 25) };
            btnSendChat = new Button { Location = new Point(605, 418), Size = new Size(85, 27), Text = "Gửi", Enabled = false };

            btnVote = new Button { Location = new Point(15, 465), Size = new Size(180, 38), Text = "VOTE TREO CỔ", Enabled = false };
            btnSoi = new Button { Location = new Point(230, 465), Size = new Size(210, 38), Text = "SOI (TIÊN TRI)", Enabled = false };
            btnCan = new Button { Location = new Point(480, 465), Size = new Size(210, 38), Text = "CẮN (MA SÓI)", Enabled = false };

            btnConnect.Click += BtnConnect_Click;
            btnSendChat.Click += BtnSendChat_Click;
            btnVote.Click += BtnVote_Click;
            btnSoi.Click += BtnSoi_Click;
            btnCan.Click += BtnCan_Click;

            txtInputChat.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnSendChat.PerformClick();
                }
            };

            this.Controls.AddRange(new Control[] {
                lblIpTitle, txtIP, lblPortTitle, txtPort, lblUserTitle, txtUsername,
                btnConnect, lblRole, lblTimer, lblStatus, lblListTitle, lstPlayers,
                txtChatLog, txtInputChat, btnSendChat, btnVote, btnSoi, btnCan
            });
        }
    }
}
