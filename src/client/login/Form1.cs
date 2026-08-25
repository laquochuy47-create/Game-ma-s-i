using System;
using System.Windows.Forms;
using SharedLibrary; // Chú ý phải có dòng này để dùng GamePacket

namespace WerewolfClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Hàm này nằm BÊN TRONG class Form1
        private async void button1_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng xử lý mạng
            ClientNetworkService client = new ClientNetworkService();

            // Đăng ký sự kiện: Khi Server gửi tin nhắn về, sẽ hiện hộp thoại lên
            client.OnPacketReceived += (packet) =>
            {
                // Gọi UI thread để hiển thị MessageBox
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"Nhận được tin từ Server: {packet.Payload}");
                }));
            };

            try
            {
                // Yêu cầu kết nối đến Server (127.0.0.1 là máy cục bộ)
                await client.ConnectAsync("127.0.0.1", 8080);
                MessageBox.Show("Đã kết nối thành công!");

                // Gửi thử một tin nhắn lên Server
                await client.SendChatAsync("Xin chào Quản trò, tôi là người chơi mới!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}