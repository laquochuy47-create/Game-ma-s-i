# Game-ma-s-i

Thành viên 1: Phát triển Core Server. Thiết lập kiến trúc mạng sử dụng TCP Sockets đa luồng. Code hệ thống lắng nghe, xử lý chấp nhận (Accept) hoặc từ chối (Reject) kết nối từ Client. Xử lý phân luồng gửi tin nhắn chung (Broadcast) và tin nhắn riêng (Multicast). Bắt lỗi ngoại lệ khi Client ngắt kết nối đột ngột.

Thành viên 2: Xử lý Game Logic Server. Phân tích logic các pha Ngày và Đêm trong game. Code chức năng quản lý danh sách người chơi. Xây dựng tính năng tự động chia phe và phân vai ngẫu nhiên. Xử lý kết quả khi người chơi Vote và xét các điều kiện thắng/thua.

Thành viên 3: Quản lý Đồng bộ & Message. Thiết kế cấu trúc các gói tin Message bằng định dạng JSON. Xử lý logic đóng gói và giải mã (parse) các gói tin JSON. Đồng bộ bộ đếm thời gian (Countdown timer) cho hai pha Ngày/Đêm. Xử lý logic hiển thị các thông báo hệ thống chung.

Thành viên 4: Phát triển Core Client & Network. Thiết lập kết nối mạng từ phía Client gửi tới Server. Code cơ chế nhận và gửi gói tin sử dụng kỹ thuật bất đồng bộ (asynch). Bắt các sự kiện (event) để cập nhật giao diện người dùng (UI) mà không làm treo máy. Tối ưu hóa logic gửi các hành động tương tác như cắn, soi.

Thành viên 5: Thiết kế Client GUI (Ngoài phòng game). Vẽ Mockup giao diện (GUI) cho màn hình Login và màn hình chờ Lobby. Code giao diện màn hình Login cho phép nhập IP, Port và Tên người chơi. Code giao diện màn hình Lobby để hiển thị danh sách người chơi đang online. Ghép luồng để xử lý việc chuyển từ màn hình Login sang Lobby.

Thành viên 6: Thiết kế Client GUI (Trong phòng game/In-game). Vẽ Mockup giao diện cho màn hình lúc đang chơi (In-game). Code bố cục (layout) hiển thị danh sách người chơi. Code phần hiển thị lịch sử Chat, các nút Vote và Kỹ năng. Hiển thị trạng thái người sống/chết cập nhật theo thời gian thực (real-time).

Thành viên 7: Quản lý Chất lượng (QA) & Báo cáo. Khởi tạo và thiết lập kho lưu trữ (Repo) chuẩn. Xây dựng kịch bản để test luồng hoạt động của game. Thực hiện các bài test chịu tải đồng thời (Concurrency test). Thực hiện kịch bản test việc mất kết nối ngang.
