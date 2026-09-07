using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
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
            this.Size = new Size(420, 680); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Load hình nền nếu có
            try 
            {
                string exeFolder = System.AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(exeFolder, "ảnh login.png");
                this.BackgroundImage = Image.FromFile(imagePath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            } 
            catch 
            {
                this.BackColor = Color.FromArgb(20, 24, 30); 
            }

            Font labelFont = new Font("Times New Roman", 12, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 9);
            
            Color moonGlowColor = Color.FromArgb(160, 245, 235);
            Color btnGlassColor = Color.FromArgb(120, 20, 50, 60);

            int xCenter = 140;
            int ctrlWidth = 140;

            // IP Server
            Label lblIP = new Label() { Text = "🌐 IP Server:", Location = new Point(xCenter, 280), AutoSize = true, Font = labelFont, BackColor = Color.Transparent, ForeColor = moonGlowColor };
            txtIP = new TextBox() { Location = new Point(xCenter, 305), Width = ctrlWidth, Font = inputFont, BackColor = Color.FromArgb(25, 25, 30), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "127.0.0.1" };

            // Port
            Label lblPort = new Label() { Text = "🔌 Port:", Location = new Point(xCenter, 345), AutoSize = true, Font = labelFont, BackColor = Color.Transparent, ForeColor = moonGlowColor };
            txtPort = new TextBox() { Location = new Point(xCenter, 370), Width = ctrlWidth, Font = inputFont, BackColor = Color.FromArgb(25, 25, 30), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "8888" };

            // Tên người chơi
            Label lblUsername = new Label() { Text = "👤 Tên người chơi:", Location = new Point(xCenter, 410), AutoSize = true, Font = labelFont, BackColor = Color.Transparent, ForeColor = moonGlowColor };
            txtUsername = new TextBox() { Location = new Point(xCenter, 435), Width = ctrlWidth, Font = inputFont, BackColor = Color.FromArgb(25, 25, 30), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            // Nút KẾT NỐI 
            btnConnect = new Button() { Text = "KẾT NỐI", Location = new Point(xCenter, 490), Width = ctrlWidth, Height = 40, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnConnect.BackColor = btnGlassColor; 
            btnConnect.ForeColor = moonGlowColor; 
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.FlatAppearance.BorderSize = 0; 
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.Click += BtnConnect_Click;

            // Bo tròn góc nút bấm
            btnConnect.Paint += (sender, e) =>
            {
                GraphicsPath path = new GraphicsPath();
                int radius = 15; 
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btnConnect.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btnConnect.Width - radius, btnConnect.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btnConnect.Height - radius, radius, radius, 90, 90);
                btnConnect.Region = new Region(path);
            };

            lblStatus = new Label() { Text = "🟢 Sẵn sàng kết nối...", Location = new Point(10, 600), AutoSize = true, Font = new Font("Segoe UI", 9), BackColor = Color.Transparent, ForeColor = moonGlowColor };

            this.Controls.AddRange(new Control[] { lblIP, txtIP, lblPort, txtPort, lblUsername, txtUsername, btnConnect, lblStatus });
        }

        private async void BtnConnect_Click(object? sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string username = txtUsername.Text.Trim(); 

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("🔴 Lỗi: Tên người chơi không được để trống!");
                return;
            }

            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                ShowError("🔴 Lỗi: Port phải là số nguyên!");
                return;
            }

            lblStatus.Text = "🟡 Đang kết nối đến Server...";
            lblStatus.ForeColor = Color.Gold;
            btnConnect.Enabled = false;

            try
            {
                // Giả lập kết nối trước, sau này kết nối với ClientNetworkService
                await Task.Delay(1000);

                lblStatus.Text = "🟢 Thành công! Đang vào phòng...";
                lblStatus.ForeColor = Color.LightGreen;

                // Tạm thời hiển thị thông báo kết nối thành công
                MessageBox.Show($"Đăng nhập thành công với tên: {username}", "Thông báo");
            }
            catch (Exception ex)
            {
                ShowError($"🔴 Lỗi kết nối: {ex.Message}");
            }
            finally 
            {
                btnConnect.Enabled = true;
            }
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Salmon;
        }
    }
}