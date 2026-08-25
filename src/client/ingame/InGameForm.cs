using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client
{
    public class InGameForm : Form
    {
        private ListBox lstPlayers;
        private RichTextBox rtbChat;
        private TextBox txtChatInput;
        private Button btnSend;
        private Label lblTimer;
        private Button btnVote;
        private Button btnSkill;

        public InGameForm()
        {
            // 1. Cài đặt Form cơ bản
            this.Text = "Game Ma Sói - Đang chơi";
            this.Size = new Size(820, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;

            // --- CỘT TRÁI: DANH SÁCH NGƯỜI CHƠI ---
            Label lblPlayers = new Label() { Text = "DANH SÁCH SỐNG/CHẾT", Location = new Point(20, 20), AutoSize = true, ForeColor = Color.DarkOrange, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lstPlayers = new ListBox() { Location = new Point(20, 50), Size = new Size(200, 430), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White, Font = new Font("Segoe UI", 11) };
            lstPlayers.Items.Add("nguoi_choi_1 - Sống");
            lstPlayers.Items.Add("nguoi_choi_2 - Chết"); // Mô phỏng người đã chết

            // --- CỘT GIỮA: KHUNG CHAT ---
            Label lblChat = new Label() { Text = "LỊCH SỬ CHAT", Location = new Point(240, 20), AutoSize = true, ForeColor = Color.DarkOrange, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            rtbChat = new RichTextBox() { Location = new Point(240, 50), Size = new Size(350, 390), BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White, ReadOnly = true, Font = new Font("Segoe UI", 10) };
            rtbChat.AppendText("[Hệ thống] Trò chơi bắt đầu. Trời tối, mọi người đi ngủ...\n");
            
            txtChatInput = new TextBox() { Location = new Point(240, 455), Size = new Size(260, 30), Font = new Font("Segoe UI", 10) };
            btnSend = new Button() { Text = "GỬI", Location = new Point(510, 454), Size = new Size(80, 26), BackColor = Color.DarkOrange, ForeColor = Color.Black, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            // --- CỘT PHẢI: ĐỒNG HỒ & TƯƠNG TÁC ---
            Label lblTimeTitle = new Label() { Text = "THỜI GIAN", Location = new Point(620, 20), AutoSize = true, ForeColor = Color.DarkOrange, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            lblTimer = new Label() { Text = "00:30", Location = new Point(620, 50), AutoSize = true, ForeColor = Color.Red, Font = new Font("Segoe UI", 26, FontStyle.Bold) };

            Label lblAction = new Label() { Text = "HÀNH ĐỘNG", Location = new Point(620, 120), AutoSize = true, ForeColor = Color.DarkOrange, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnVote = new Button() { Text = "VOTE TREO CỔ", Location = new Point(620, 150), Size = new Size(150, 40), BackColor = Color.Firebrick, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnSkill = new Button() { Text = "DÙNG KỸ NĂNG", Location = new Point(620, 200), Size = new Size(150, 40), BackColor = Color.DarkSlateBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };

            // Thêm tất cả vào Form
            this.Controls.Add(lblPlayers); this.Controls.Add(lstPlayers);
            this.Controls.Add(lblChat); this.Controls.Add(rtbChat);
            this.Controls.Add(txtChatInput); this.Controls.Add(btnSend);
            this.Controls.Add(lblTimeTitle); this.Controls.Add(lblTimer);
            this.Controls.Add(lblAction); this.Controls.Add(btnVote); this.Controls.Add(btnSkill);
        }
        
    }
}