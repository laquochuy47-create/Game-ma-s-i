using System;
using System.Drawing;
using System.Windows.Forms;
using GameClient.Network;

namespace GameClient
{
    public class MainForm : Form
    {
        private NetworkService _networkService;

        private RichTextBox txtChatLog;
        private TextBox txtInputChat;
        private TextBox txtTargetPlayer;
        private Button btnConnect;
        private Button btnSendChat;
        private Button btnVote;
        private Button btnSoi;
        private Button btnNiu;
        private Label lblStatus;

        public MainForm()
        {
            InitializeComponentLayout();
            _networkService = new NetworkService();

            _networkService.OnStatusChanged += UpdateStatus;
            _networkService.OnMessageReceived += HandleServerPacket;
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateStatus(message)));
                return;
            }
            lblStatus.Text = $"Trạng thái: {message}";
        }

        private void HandleServerPacket(NetworkPacket packet)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleServerPacket(packet)));
                return;
            }

            switch (packet.Action)
            {
                case "chat":
                    txtChatLog.AppendText($"[{packet.Sender}]: {packet.Message}\r\n");
                    break;

                case "system_announce":
                    txtChatLog.AppendText($"[HỆ THỐNG]: {packet.Message}\r\n");
                    break;

                case "soi_result":
                    MessageBox.Show($"Kết quả soi [{packet.Target}]: {packet.Message}", "Kết quả Tiên Tri");
                    break;

                case "update_state":
                    txtChatLog.AppendText($"[TRẠNG THÁI GAME]: {packet.Message}\r\n");
                    break;
            }
        }

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            await _networkService.ConnectAsync("127.0.0.1", 8888);
        }

        private async void BtnSendChat_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtInputChat.Text))
            {
                await _networkService.SendChatAsync(txtInputChat.Text);
                txtInputChat.Clear();
            }
        }

        private async void BtnVote_Click(object sender, EventArgs e)
        {
            string target = txtTargetPlayer.Text.Trim();
            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show("Vui lòng nhập tên người chơi muốn Vote!");
                return;
            }
            await _networkService.SendVoteAsync(target);
        }

        private async void BtnSoi_Click(object sender, EventArgs e)
        {
            string target = txtTargetPlayer.Text.Trim();
            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show("Vui lòng nhập tên người chơi muốn Soi!");
                return;
            }
            await _networkService.SendSoiAsync(target);
        }

        private async void BtnNiu_Click(object sender, EventArgs e)
        {
            string target = txtTargetPlayer.Text.Trim();
            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show("Vui lòng nhập tên người chơi muốn Níu!");
                return;
            }
            await _networkService.SendNiuAsync(target);
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Ma Sói Client - Network UI";
            this.Size = new Size(500, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblStatus = new Label { Location = new Point(15, 10), Size = new Size(450, 20), Text = "Trạng thái: Chưa kết nối" };
            btnConnect = new Button { Location = new Point(15, 35), Size = new Size(450, 30), Text = "Kết nối Máy Chủ" };

            txtChatLog = new RichTextBox { Location = new Point(15, 75), Size = new Size(450, 220), ReadOnly = true };
            
            txtInputChat = new TextBox { Location = new Point(15, 305), Size = new Size(340, 25) };
            btnSendChat = new Button { Location = new Point(365, 303), Size = new Size(100, 27), Text = "Gửi Chat" };

            Label lblTarget = new Label { Location = new Point(15, 350), Size = new Size(120, 20), Text = "Mục tiêu (Tên Player):" };
            txtTargetPlayer = new TextBox { Location = new Point(140, 347), Size = new Size(325, 25) };

            btnVote = new Button { Location = new Point(15, 390), Size = new Size(130, 35), Text = "VOTE" };
            btnSoi = new Button { Location = new Point(175, 390), Size = new Size(130, 35), Text = "SOI" };
            btnNiu = new Button { Location = new Point(335, 390), Size = new Size(130, 35), Text = "NÍU" };

            btnConnect.Click += BtnConnect_Click;
            btnSendChat.Click += BtnSendChat_Click;
            btnVote.Click += BtnVote_Click;
            btnSoi.Click += BtnSoi_Click;
            btnNiu.Click += BtnNiu_Click;

            this.Controls.Add(lblStatus);
            this.Controls.Add(btnConnect);
            this.Controls.Add(txtChatLog);
            this.Controls.Add(txtInputChat);
            this.Controls.Add(btnSendChat);
            this.Controls.Add(lblTarget);
            this.Controls.Add(txtTargetPlayer);
            this.Controls.Add(btnVote);
            this.Controls.Add(btnSoi);
            this.Controls.Add(btnNiu);
        }
    }
}