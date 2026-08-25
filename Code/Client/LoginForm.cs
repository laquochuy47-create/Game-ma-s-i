using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public class LoginForm : Form
    {
        private TextBox txtIP, txtPort, txtUsername;
        private Button btnConnect;
        private Label lblStatus;

        public LoginForm()
        {
            this.Text = "Game Ma Sói - Đăng nhập";
            this.Size = new Size(380, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(42, 42, 46); // #2A2A2E
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Font chung
            Font mainFont = new Font("Segoe UI", 10);
            Font boldFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // 🌐 IP Server
            Label lblIP = new Label() { Text = "🌐 IP Server:", Location = new Point(40, 30), AutoSize = true, Font = mainFont };
            txtIP = new TextBox() { Location = new Point(40, 55), Width = 280, Font = mainFont, BackColor = Color.FromArgb(30, 30, 34), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "127.0.0.1" };

            // 🔌 Port
            Label lblPort = new Label() { Text = "🔌 Port:", Location = new Point(40, 95), AutoSize = true, Font = mainFont };
            txtPort = new TextBox() { Location = new Point(40, 120), Width = 280, Font = mainFont, BackColor = Color.FromArgb(30, 30, 34), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "8888" };

            // 👤 Tên người chơi
            Label lblUsername = new Label() { Text = "👤 Tên người chơi:", Location = new Point(40, 160), AutoSize = true, Font = mainFont };
            txtUsername = new TextBox() { Location = new Point(40, 185), Width = 280, Font = mainFont, BackColor = Color.FromArgb(30, 30, 34), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Nút Kết nối (Flat Design)
            btnConnect = new Button() { Text = "KẾT NỐI", Location = new Point(40, 230), Width = 280, Height = 40, Font = boldFont };
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.BackColor = Color.DarkOrange;
            btnConnect.ForeColor = Color.Black;
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.Click += BtnConnect_Click;

            // Thanh Status góc dưới
            lblStatus = new Label() { Text = "🟢 Sẵn sàng...", Location = new Point(10, 290), AutoSize = true, Font = new Font("Segoe UI", 9), ForeColor = Color.LightGreen };

            this.Controls.AddRange(new Control[] { lblIP, txtIP, lblPort, txtPort, lblUsername, txtUsername, btnConnect, lblStatus });
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblStatus.Text = "🔴 Lỗi: Vui lòng nhập tên!";
                lblStatus.ForeColor = Color.Salmon;
                return;
            }
            lblStatus.Text = "🟡 Đang kết nối đến Server...";
            lblStatus.ForeColor = Color.Gold;

            // TODO: Gọi hàm kết nối của TV4. Tạm thời giả lập chuyển màn hình.
            LobbyForm lobby = new LobbyForm();
            lobby.Show();
            this.Hide();
        }
    }
}