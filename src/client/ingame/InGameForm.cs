using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public class InGameForm : Form
    {
        private NetworkService _networkService;
        private string _username;
        private string _myRole = "VILLAGER";

        private ListBox lstPlayers;
        private RichTextBox rtbChat;
        private TextBox txtChatInput;
        private Button btnSend;
        private Label lblTimer;
        private Button btnVote;
        private Button btnSkill;

        public InGameForm(NetworkService networkService, string username)
        {
            _networkService = networkService;
            _username = username;

            this.Text = $"Game Ma Sói - Đang chơi ({_username})";
            this.Size = new Size(820, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(42, 42, 46);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Color accentColor = Color.DarkOrange;
            Color panelBg = Color.FromArgb(30, 30, 34);
            Font titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font contentFont = new Font("Segoe UI", 11);

            // Cột trái: Danh sách người chơi
            Label lblPlayers = new Label() { Text = "DANH SÁCH SỐNG/CHẾT", Location = new Point(20, 20), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            lstPlayers = new ListBox() { Location = new Point(20, 50), Size = new Size(200, 430), BackColor = panelBg, ForeColor = Color.White, Font = contentFont, BorderStyle = BorderStyle.FixedSingle };

            // Cột giữa: Lịch sử Chat và Nhập liệu
            Label lblChat = new Label() { Text = "LỊCH SỬ CHAT", Location = new Point(240, 20), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            rtbChat = new RichTextBox() { Location = new Point(240, 50), Size = new Size(350, 390), BackColor = panelBg, ForeColor = Color.White, ReadOnly = true, Font = contentFont, BorderStyle = BorderStyle.FixedSingle };

            txtChatInput = new TextBox() { Location = new Point(240, 455), Size = new Size(260, 30), Font = contentFont };
            btnSend = new Button() { Text = "GỬI", Location = new Point(510, 453), Size = new Size(80, 28), BackColor = accentColor, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
            
            // Gắn sự kiện gửi tin nhắn (bấm nút hoặc ấn Enter)
            btnSend.Click += BtnSend_Click;
            txtChatInput.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSend_Click(null, EventArgs.Empty); } };

            // Cột phải: Đồng hồ và Nút Hành động
            Label lblTimeTitle = new Label() { Text = "THỜI GIAN", Location = new Point(620, 20), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            lblTimer = new Label() { Text = "00:00", Location = new Point(620, 50), AutoSize = true, ForeColor = Color.Red, Font = new Font("Segoe UI", 26, FontStyle.Bold) };

            Label lblAction = new Label() { Text = "HÀNH ĐỘNG", Location = new Point(620, 120), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            btnVote = new Button() { Text = "VOTE TREO CỔ", Location = new Point(620, 150), Size = new Size(160, 40), BackColor = Color.Firebrick, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
            btnVote.Click += BtnVote_Click;

            btnSkill = new Button() { Text = "DÙNG KỸ NĂNG", Location = new Point(620, 200), Size = new Size(160, 40), BackColor = Color.DarkSlateBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
            btnSkill.Click += BtnSkill_Click;

            this.FormClosed += (sender, e) => Application.Exit();
            this.Controls.AddRange(new Control[] { lblPlayers, lstPlayers, lblChat, rtbChat, txtChatInput, btnSend, lblTimeTitle, lblTimer, lblAction, btnVote, btnSkill });

            // Lắng nghe dữ liệu từ Server
            _networkService.OnMessageReceived += HandleGamePacket;
            _networkService.OnDisconnected += () => {
                if (IsHandleCreated) Invoke(new Action(() => MessageBox.Show("Mất kết nối với máy chủ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning)));
            };
        }

        private void HandleGamePacket(NetworkPacket packet)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleGamePacket(packet)));
                return;
            }

            switch (packet.Action.ToLower())
            {
                case "chat":
                    AppendChatMessage($"[{packet.Sender}]: {packet.Message}", Color.LightSkyBlue);
                    break;
                case "system":
                    AppendChatMessage($"[HỆ THỐNG]: {packet.Message}", Color.Yellow);
                    break;
                case "timer":
                    UpdateTimerDisplay(packet.Message);
                    break;
                case "update_players":
                    lstPlayers.Items.Clear();
                    var players = packet.Message.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in players) lstPlayers.Items.Add(p.Trim());
                    break;
                case "role_assign":
                    _myRole = packet.Message.ToUpper();
                    AppendChatMessage($"[HỆ THỐNG]: Vai trò của bạn là {_myRole}", Color.Cyan);
                    break;
                case "phase_day":
                    ToggleActionButtons(canVote: true, canUseSkill: false);
                    AppendChatMessage("☀️ Trời đã sáng! Thảo luận và bỏ phiếu.", Color.Gold);
                    break;
                case "phase_night":
                    ToggleActionButtons(canVote: false, canUseSkill: _myRole != "VILLAGER");
                    AppendChatMessage("🌙 Trời đã tối! Các chức năng thức giấc.", Color.MediumPurple);
                    break;
            }
        }

        public void AppendChatMessage(string message, Color textColor)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = textColor;
            rtbChat.AppendText(message + "\n");
            rtbChat.ScrollToCaret();
        }

        public void UpdateTimerDisplay(string timeString) => lblTimer.Text = timeString;

        public void ToggleActionButtons(bool canVote, bool canUseSkill)
        {
            btnVote.Enabled = canVote;
            btnVote.BackColor = canVote ? Color.Firebrick : Color.Gray;
            btnSkill.Enabled = canUseSkill;
            btnSkill.BackColor = canUseSkill ? Color.DarkSlateBlue : Color.Gray;
        }

        private async void BtnSend_Click(object? sender, EventArgs e)
        {
            string message = txtChatInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(message)) return;

            // In tin nhắn lên màn hình của chính bạn
            AppendChatMessage($"[Bạn]: {message}", Color.LightSkyBlue);
            
            // Bắn tin nhắn lên Server
            await _networkService.SendChatAsync(message);
            
            // Xóa trắng ô nhập liệu
            txtChatInput.Clear();
        }

        private async void BtnVote_Click(object? sender, EventArgs e)
        {
            if (lstPlayers.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn người chơi trong danh sách để Vote!");
                return;
            }
            string target = lstPlayers.SelectedItem?.ToString() ?? "";
            await _networkService.SendVoteAsync(target);
        }

        private async void BtnSkill_Click(object? sender, EventArgs e)
        {
            if (lstPlayers.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn người chơi trong danh sách để dùng kỹ năng!");
                return;
            }
            string target = lstPlayers.SelectedItem?.ToString() ?? "";
            await _networkService.SendSkillAsync(target);
        }
    }
}