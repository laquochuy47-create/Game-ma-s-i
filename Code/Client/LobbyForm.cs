using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public class LobbyForm : Form
    {
        private ListView lvPlayers;
        private ProgressBar pbPlayers;
        private Label lblPlayerCount;
        private Label lblStatus;
        private Button btnReady;

        public LobbyForm()
        {
            this.Text = "Game Ma Sói - Phòng chờ";
            this.Size = new Size(450, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(42, 42, 46);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Font mainFont = new Font("Segoe UI", 11);
            Font boldFont = new Font("Segoe UI", 12, FontStyle.Bold);

            // Tiêu đề
            Label lblTitle = new Label() { Text = "🎮 DANH SÁCH NGƯỜI CHƠI", Location = new Point(30, 20), AutoSize = true, Font = boldFont, ForeColor = Color.DarkOrange };

            // Tiến độ (ProgressBar)
            pbPlayers = new ProgressBar() { Location = new Point(30, 60), Width = 370, Height = 10, Maximum = 10, Value = 3, Style = ProgressBarStyle.Continuous };
            lblPlayerCount = new Label() { Text = "Số lượng: 3/10", Location = new Point(30, 75), AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Color.LightGray };

            // Danh sách người chơi (Sử dụng ListView để đẹp hơn ListBox)
            lvPlayers = new ListView() { Location = new Point(30, 110), Size = new Size(370, 260), BackColor = Color.FromArgb(30, 30, 34), ForeColor = Color.White, Font = mainFont, BorderStyle = BorderStyle.FixedSingle, View = View.Details, HeaderStyle = ColumnHeaderStyle.None, FullRowSelect = true };
            lvPlayers.Columns.Add("Name", 340);
            
           // Dữ liệu mẫu: 5 người chơi
            lvPlayers.Items.Add(new ListViewItem("👤 nguoi_choi_1 (Bạn) - Host"));
            lvPlayers.Items.Add(new ListViewItem("👤 nguoi_choi_2"));
            lvPlayers.Items.Add(new ListViewItem("👤 nguoi_choi_3"));
            lvPlayers.Items.Add(new ListViewItem("👤 nguoi_choi_4"));
            lvPlayers.Items.Add(new ListViewItem("👤 nguoi_choi_5"));

            // Cập nhật thanh tiến độ thành 5 người
            pbPlayers.Value = 5;
            lblPlayerCount.Text = "Số lượng: 5/10";
            // Nút Sẵn sàng
            btnReady = new Button() { Text = "SẴN SÀNG", Location = new Point(30, 390), Width = 370, Height = 40, Font = boldFont, BackColor = Color.SeaGreen, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnReady.FlatAppearance.BorderSize = 0;

            // Label trạng thái 
            lblStatus = new Label() { Text = "⏳ Đang chờ Quản trò bắt đầu ván đấu...", Location = new Point(30, 440), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Italic), ForeColor = Color.Gold };

            this.Controls.AddRange(new Control[] { lblTitle, pbPlayers, lblPlayerCount, lvPlayers, btnReady, lblStatus });
            this.FormClosed += (sender, e) => Application.Exit();
        }
    }
}