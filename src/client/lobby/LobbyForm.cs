using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Client
{
    public class LobbyForm : Form
    {
        private ListView lvPlayers;
        private Button btnReady;
        private Label lblTitle, lblPlayerCount;

        public LobbyForm()
        {
            // 1. Cài đặt Form
            this.Text = "Game Ma Sói - Phòng chờ";
            this.Size = new Size(450, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 24, 30); // Nền rừng đêm tăm tối
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 2. Bảng màu đồng bộ từ Login
            Color moonGlowColor = Color.FromArgb(160, 245, 235);
            Color btnGlassColor = Color.FromArgb(120, 20, 50, 60);

            Font boldFont = new Font("Segoe UI", 14, FontStyle.Bold);
            Font mainFont = new Font("Segoe UI", 10);

            // 3. Tiêu đề
            lblTitle = new Label() { Text = "🏰 PHÒNG CHỜ", Location = new Point(0, 20), Width = 450, TextAlign = ContentAlignment.MiddleCenter, Font = boldFont, ForeColor = moonGlowColor };
            lblPlayerCount = new Label() { Text = "Số lượng: 1/10", Location = new Point(40, 70), AutoSize = true, Font = mainFont, ForeColor = Color.LightGray };

            // 4. Danh sách người chơi (Thiết kế phẳng, không viền)
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
            
            // Dữ liệu giả lập ban đầu
            lvPlayers.Items.Add(new ListViewItem("🐺 Bạn (Host)"));
            lvPlayers.Items.Add(new ListViewItem("⏳ Đang chờ người chơi khác..."));

            // 5. Nút Sẵn sàng (Kính mờ, bo góc)
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

            // Đảm bảo tắt form này là tắt luôn toàn bộ Game
            this.FormClosed += (sender, e) => Application.Exit();

            this.Controls.AddRange(new Control[] { lblTitle, lblPlayerCount, lvPlayers, btnReady });
        }
        public void UpdatePlayerList(string[] players)
        {
            if (lvPlayers.InvokeRequired) { lvPlayers.Invoke(new Action(() => UpdatePlayerList(players))); return; }
            lvPlayers.Items.Clear();
            foreach(var p in players) lvPlayers.Items.Add(new ListViewItem(p));
        }
    }
}