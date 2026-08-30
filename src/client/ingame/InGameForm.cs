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
            this.BackColor = Color.FromArgb(42, 42, 46); // Đồng bộ nền xám đen với Login/Lobby
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Biến dùng chung để đồng bộ màu sắc và font
            Color accentColor = Color.DarkOrange;
            Color panelBg = Color.FromArgb(30, 30, 34);
            Font titleFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font contentFont = new Font("Segoe UI", 11);

          // --- CỘT TRÁI: DANH SÁCH NGƯỜI CHƠI ---
            Label lblPlayers = new Label() { Text = "DANH SÁCH SỐNG/CHẾT", Location = new Point(20, 20), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            lstPlayers = new ListBox() { Location = new Point(20, 50), Size = new Size(200, 430), BackColor = panelBg, ForeColor = Color.White, Font = contentFont, BorderStyle = BorderStyle.FixedSingle };
            
            // Dữ liệu mẫu 5 người chơi (khớp với kịch bản TC_01)
            lstPlayers.Items.Add("👤 Bạn (Host) - Sống");
            lstPlayers.Items.Add("👤 nguoi_choi_2 - Sống");
            lstPlayers.Items.Add("👤 nguoi_choi_3 - Chết"); // Mô phỏng 1 người đã bị loại
            lstPlayers.Items.Add("👤 nguoi_choi_4 - Sống");
            lstPlayers.Items.Add("👤 nguoi_choi_5 - Sống");
            // --- CỘT GIỮA: KHUNG CHAT ---
            Label lblChat = new Label() { Text = "LỊCH SỬ CHAT", Location = new Point(240, 20), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            rtbChat = new RichTextBox() { Location = new Point(240, 50), Size = new Size(350, 390), BackColor = panelBg, ForeColor = Color.White, ReadOnly = true, Font = contentFont, BorderStyle = BorderStyle.FixedSingle };
            rtbChat.AppendText("[Hệ thống] Trò chơi bắt đầu. Trời tối, mọi người đi ngủ...\n");
            
            txtChatInput = new TextBox() { Location = new Point(240, 455), Size = new Size(260, 30), Font = contentFont };
            
            // Nút GỬI (Có viền trắng như ảnh)
            btnSend = new Button() { Text = "GỬI", Location = new Point(510, 453), Size = new Size(80, 28), BackColor = accentColor, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
            btnSend.FlatAppearance.BorderColor = Color.White;
            btnSend.FlatAppearance.BorderSize = 1;
            // Cho phép ấn Enter để gửi tin nhắn
            txtChatInput.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSend_Click(null, EventArgs.Empty); } };
            
            // Gắn sự kiện Click cho nút GỬI
            btnSend.Click += BtnSend_Click;

            // --- CỘT PHẢI: ĐỒNG HỒ & TƯƠNG TÁC ---
            Label lblTimeTitle = new Label() { Text = "THỜI GIAN", Location = new Point(620, 20), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            lblTimer = new Label() { Text = "00:30", Location = new Point(620, 50), AutoSize = true, ForeColor = Color.Red, Font = new Font("Segoe UI", 26, FontStyle.Bold) };

            Label lblAction = new Label() { Text = "HÀNH ĐỘNG", Location = new Point(620, 120), AutoSize = true, ForeColor = accentColor, Font = titleFont };
            
            // Nút VOTE TREO CỔ (Viền trắng dày)
            btnVote = new Button() { Text = "VOTE TREO CỔ", Location = new Point(620, 150), Size = new Size(160, 40), BackColor = Color.Firebrick, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
            btnVote.FlatAppearance.BorderColor = Color.White;
            btnVote.FlatAppearance.BorderSize = 2;
            btnVote.Click += BtnVote_Click;

            // Nút DÙNG KỸ NĂNG (Viền trắng dày)
            btnSkill = new Button() { Text = "DÙNG KỸ NĂNG", Location = new Point(620, 200), Size = new Size(160, 40), BackColor = Color.DarkSlateBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, FlatStyle = FlatStyle.Flat };
            btnSkill.FlatAppearance.BorderColor = Color.White;
            btnSkill.FlatAppearance.BorderSize = 2;
            btnSkill.Click += BtnSkill_Click;

            // Đảm bảo tắt form là thoát app hoàn toàn, không bị kẹt tiến trình ngầm
            this.FormClosed += (sender, e) => Application.Exit();

            // Thêm tất cả vào Form
            this.Controls.AddRange(new Control[] { lblPlayers, lstPlayers, lblChat, rtbChat, txtChatInput, btnSend, lblTimeTitle, lblTimer, lblAction, btnVote, btnSkill });
        }
        public void AppendChatMessage(string message, Color textColor)
        {
            // Kiểm tra và đẩy luồng nếu cần thiết
            if (rtbChat.InvokeRequired)
            {
                rtbChat.Invoke(new Action(() => AppendChatMessage(message, textColor)));
                return;
            }

            // Xử lý giao diện ở luồng chính
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = textColor;
            rtbChat.AppendText(message + "\n");
            rtbChat.ScrollToCaret(); // Tự động cuộn xuống dòng mới nhất
        }
        public void UpdateTimerDisplay(string timeString)
        {
            if (lblTimer.InvokeRequired)
            {
                lblTimer.Invoke(new Action(() => UpdateTimerDisplay(timeString)));
                return;
            }
            
            lblTimer.Text = timeString;
        }
        public void ToggleActionButtons(bool canVote, bool canUseSkill)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ToggleActionButtons(canVote, canUseSkill)));
                return;
            }

            // Bật/tắt nút Vote
            btnVote.Enabled = canVote;
            btnVote.BackColor = canVote ? Color.Firebrick : Color.Gray; 
            
            // Bật/tắt nút Kỹ năng
            btnSkill.Enabled = canUseSkill;
            btnSkill.BackColor = canUseSkill ? Color.DarkSlateBlue : Color.Gray;
        }
        // --- HÀM XỬ LÝ SỰ KIỆN GỬI TIN NHẮN ---
        private void BtnSend_Click(object? sender, EventArgs e)
        {
            string message = txtChatInput.Text.Trim();

            // Chặn tin nhắn rỗng
            if (string.IsNullOrWhiteSpace(message)) return;

            // Đẩy chữ lên khung lịch sử chat (Màu xanh lơ cho dễ phân biệt)
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            rtbChat.SelectionColor = Color.LightSkyBlue;
            rtbChat.AppendText($"[Bạn]: {message}\n");
            
            // Tự động cuộn xuống dòng mới nhất
            rtbChat.ScrollToCaret();

            // Xóa trắng ô nhập liệu để gõ tin mới
            txtChatInput.Text = "";
        }
        // --- HÀM XỬ LÝ SỰ KIỆN DÙNG KỸ NĂNG ---
        private void BtnSkill_Click(object? sender, EventArgs e)
        {
            // 1. Kiểm tra xem người chơi đã chọn mục tiêu trong ListBox chưa
            if (lstPlayers.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một người chơi trong danh sách bên trái để sử dụng kỹ năng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy nội dung người chơi đang được chọn
            string targetPlayer = lstPlayers.SelectedItem.ToString() ?? "";

            // 3. Phản hồi lên giao diện (In ra lịch sử chat với màu Vàng - Gold)
            AppendChatMessage($"[Kỹ năng]: Bạn đang nhắm mục tiêu vào {targetPlayer}...", Color.Gold);

            // TODO (Dành cho Team Mạng): 
            // Gói dữ liệu mục tiêu (targetPlayer) thành file JSON và bắn lên Server tại đây.
            
            // Tuỳ chọn: Bỏ chọn trong danh sách sau khi dùng xong để tránh bấm nhầm lần sau
            lstPlayers.ClearSelected();
        }
        // --- HÀM XỬ LÝ SỰ KIỆN VOTE TREO CỔ ---
        private void BtnVote_Click(object? sender, EventArgs e)
        {
            // 1. Kiểm tra xem người chơi đã chọn ai để Vote chưa
            if (lstPlayers.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một người chơi trong danh sách bên trái để vote treo cổ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy thông tin người bị Vote
            string targetPlayer = lstPlayers.SelectedItem.ToString() ?? "";

            // 3. Phản hồi lên giao diện (In ra lịch sử chat với màu Đỏ - Firebrick để hợp với màu nút Vote)
            AppendChatMessage($"[Vote]: Bạn đã bỏ phiếu treo cổ {targetPlayer}.", Color.Firebrick);

            // TODO (Dành cho Team Mạng): 
            // Đóng gói quyết định Vote này thành JSON và gửi lên Server.
            
            // Bỏ chọn danh sách sau khi Vote xong
            lstPlayers.ClearSelected();
        }
    }
}