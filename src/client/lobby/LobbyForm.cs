using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Client
{
    public class LobbyForm : Form
    {
        private NetworkService _networkService;
        private string _username;
        private ListView lvPlayers;
        private Button btnReady;
        private Label lblTitle, lblPlayerCount;

        public LobbyForm(NetworkService networkService, string username)
        {
            _networkService = networkService;
            _username = username;

            this.Text = "Game Ma Sói - Phòng chờ";
            this.Size = new Size(450, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 24, 30);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Color moonGlowColor = Color.FromArgb(160, 245, 235);
            Color btnGlassColor = Color.FromArgb(120, 20, 50, 60);
            Font boldFont = new Font("Segoe UI", 14, FontStyle.Bold);
            Font mainFont = new Font("Segoe UI", 10);

            lblTitle = new Label() { Text = "🏰 PHÒNG CHỜ", Location = new Point(0, 20), Width = 450, TextAlign = ContentAlignment.MiddleCenter, Font = boldFont, ForeColor = moonGlowColor };
            lblPlayerCount = new Label() { Text = "Số lượng: 1/10", Location = new Point(40, 70), AutoSize = true, Font = mainFont, ForeColor = Color.LightGray };

            lvPlayers = new ListView() { 
                Location = new Point(40, 100), 
                Size = new Size(350, 350), 
                BackColor = Color.FromArgb(15, 25, 30), 
                ForeColor = Color.White, 
                Font = mainFont, 
                BorderStyle = BorderStyle.None, 
                View = View.Details, 
                HeaderStyle = ColumnHeaderStyle.None, 
                FullRowSelect = true 
            };
            lvPlayers.Columns.Add("Name", 320);
            lvPlayers.Items.Add(new ListViewItem($"🐺 {_username} (Bạn)"));

            btnReady = new Button() { Text = "SẴN SÀNG", Location = new Point(40, 480), Width = 350, Height = 45, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnReady.BackColor = btnGlassColor;
            btnReady.ForeColor = moonGlowColor;
            btnReady.FlatStyle = FlatStyle.Flat;
            btnReady.FlatAppearance.BorderSize = 0;
            btnReady.Cursor = Cursors.Hand;
            
            btnReady.Paint += (sender, e) => {
                GraphicsPath path = new GraphicsPath();
                int r = 15;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(btnReady.Width - r, 0, r, r, 270, 90);
                path.AddArc(btnReady.Width - r, btnReady.Height - r, r, r, 0, 90);
                path.AddArc(0, btnReady.Height - r, r, r, 90, 90);
                btnReady.Region = new Region(path);
            };

            btnReady.Click += async (s, e) => {
                btnReady.Enabled = false;
                await _networkService.SendPacketAsync("ready", message: _username);
            };

            this.FormClosed += (sender, e) => Application.Exit();
            this.Controls.AddRange(new Control[] { lblTitle, lblPlayerCount, lvPlayers, btnReady });

            _networkService.OnMessageReceived += HandleLobbyPacket;
        }

        private void HandleLobbyPacket(NetworkPacket packet)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleLobbyPacket(packet)));
                return;
            }

            switch (packet.Action.ToLower())
            {
                case "update_players":
                    var players = packet.Message.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    UpdatePlayerList(players);
                    break;

                case "start_game":
                    _networkService.OnMessageReceived -= HandleLobbyPacket;
                    InGameForm inGame = new InGameForm(_networkService, _username);
                    inGame.Show();
                    this.Hide();
                    break;
            }
        }

        public void UpdatePlayerList(string[] players)
        {
            lvPlayers.Items.Clear();
            foreach(var p in players) lvPlayers.Items.Add(new ListViewItem(p.Trim()));
            lblPlayerCount.Text = $"Số lượng: {lvPlayers.Items.Count}/10";
        }
    }
}