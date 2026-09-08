# 🐺 Game Ma Sói

> **Game Ma Sói** là dự án game multiplayer được phát triển nhằm mô phỏng trò chơi **Ma Sói/Werewolf** theo mô hình client-server. Người chơi tham gia vào một phòng chơi, nhận vai trò bí mật và tương tác với những người chơi khác thông qua các giai đoạn của trò chơi.

---

## 📌 Giới thiệu

Game Ma Sói được xây dựng với mục tiêu áp dụng kiến thức về:

* Lập trình mạng
* Mô hình Client – Server
* Giao tiếp giữa nhiều người chơi
* Quản lý phòng chơi
* Xử lý trạng thái game
* Thiết kế giao thức truyền thông
* Kiểm thử phần mềm
* Phân chia module trong một dự án phần mềm

Repository được tổ chức thành các phần **Client**, **Server**, **Shared**, tài liệu thiết kế và kiểm thử.

---

## 🎮 Tính năng chính

### 👤 Người chơi

* Tham gia game.
* Tạo hoặc tham gia phòng.
* Nhập tên người chơi.
* Nhận vai trò trong game.
* Theo dõi trạng thái trận đấu.
* Tham gia các lượt ban ngày và ban đêm.
* Thực hiện hành động tương ứng với vai trò.
* Bỏ phiếu trong giai đoạn thảo luận.
* Theo dõi kết quả trận đấu.

### 🏠 Phòng chơi

* Tạo phòng.
* Tham gia phòng bằng mã phòng.
* Quản lý danh sách người chơi.
* Theo dõi trạng thái phòng.
* Bắt đầu trận đấu khi đủ điều kiện.

### 🌙 Hệ thống game

Trận đấu được chia thành các giai đoạn:

```text
Lobby
  ↓
Phân chia vai trò
  ↓
Ban đêm
  ↓
Ban ngày
  ↓
Thảo luận
  ↓
Bỏ phiếu
  ↓
Kiểm tra điều kiện thắng
  ↓
Kết thúc / Sang vòng tiếp theo
```

---

## 🧑‍🤝‍🧑 Các vai trò

Tùy cấu hình của trận đấu, game có thể hỗ trợ các vai trò như:

| Vai trò             | Chức năng                                          |
| ------------------- | -------------------------------------------------- |
| 🐺 Ma Sói           | Hoạt động bí mật và loại người chơi theo luật game |
| 👨 Dân làng         | Tìm và loại Ma Sói thông qua thảo luận và bỏ phiếu |
| 🔮 Tiên tri         | Có khả năng kiểm tra thông tin của người chơi      |
| 🩺 Bảo vệ           | Có khả năng bảo vệ người chơi                      |
| 🎭 Vai trò đặc biệt | Có thể được bổ sung tùy phiên bản                  |

> **Lưu ý:** Danh sách vai trò thực tế phụ thuộc vào phiên bản và cấu hình được triển khai trong source code.

---

# 🏗️ Kiến trúc hệ thống

Project được tổ chức theo mô hình:

```text
                 ┌──────────────────┐
                 │      Client      │
                 │    Người chơi    │
                 └────────┬─────────┘
                          │
                    Network / API
                          │
                          ▼
                 ┌──────────────────┐
                 │      Server      │
                 │  Game Processing │
                 └────────┬─────────┘
                          │
                          ▼
                 ┌──────────────────┐
                 │      Shared      │
                 │ Models / Protocol│
                 └──────────────────┘
```

### Client

Client chịu trách nhiệm:

* Giao diện người chơi.
* Nhập và hiển thị dữ liệu.
* Gửi yêu cầu đến server.
* Nhận dữ liệu từ server.
* Hiển thị trạng thái game.

### Server

Server chịu trách nhiệm:

* Quản lý kết nối.
* Quản lý phòng chơi.
* Quản lý người chơi.
* Phân chia vai trò.
* Xử lý luật game.
* Đồng bộ trạng thái game.
* Kiểm tra điều kiện chiến thắng.

### Shared

Chứa các thành phần dùng chung giữa Client và Server như:

* Model.
* DTO.
* Message.
* Protocol.
* Enum.
* Các kiểu dữ liệu dùng trong giao tiếp.

---

# 📂 Cấu trúc thư mục

```text
Game-ma-s-i/
│
├── docs/
│   ├── architecture/
│   ├── mockup/
│   ├── protocol/
│   └── test-plan/
│
├── src/
│   ├── client/
│   ├── server/
│   └── shared/
│
├── test/
│   ├── client/
│   │   └── network/
│   ├── integration/
│   └── server/
│
├── .gitignore
├── MaSoi.slnx
└── Task_Werewolf.xlsx
```

Cấu trúc này tương ứng với repository hiện tại, trong đó `docs` được chia thành tài liệu kiến trúc, mockup, protocol và test plan; `test` gồm client/network, integration và server.

---

# 🛠️ Công nghệ sử dụng

| Thành phần      | Công nghệ                             |
| --------------- | ------------------------------------- |
| Ngôn ngữ        | C#                                    |
| IDE             | Visual Studio / Visual Studio Code    |
| Project         | .NET                                  |
| Kiến trúc       | Client – Server                       |
| Network         | Network Socket / Protocol của project |
| Version Control | Git + GitHub                          |
| Testing         | Unit Test / Integration Test          |

---

# ⚙️ Yêu cầu môi trường

Trước khi chạy project, cần cài đặt:

* **.NET SDK**
* **Visual Studio 2022** hoặc **Visual Studio Code**
* **Git**

Kiểm tra .NET:

```bash
dotnet --version
```

Kiểm tra Git:

```bash
git --version
```

---

# 🚀 Cài đặt project

## 1. Clone repository

```bash
git clone https://github.com/laquochuy47-create/Game-ma-s-i.git
```

Di chuyển vào thư mục:

```bash
cd Game-ma-s-i
```

---

## 2. Mở project

Có thể mở solution:

```text
MaSoi.slnx
```

bằng Visual Studio.

Hoặc mở thư mục project bằng VS Code:

```bash
code .
```

---

## 3. Restore dependencies

Chạy:

```bash
dotnet restore
```

---

## 4. Build project

```bash
dotnet build
```

Nếu build thành công, hệ thống đã sẵn sàng để chạy.

---

# ▶️ Chạy chương trình

## Chạy Server

Di chuyển đến project Server:

```bash
cd src/server
```

Sau đó:

```bash
dotnet run
```

Server sẽ khởi động và chờ các Client kết nối.

---

## Chạy Client

Mở một terminal khác:

```bash
cd src/client
```

Chạy:

```bash
dotnet run
```

Có thể mở nhiều Client để mô phỏng nhiều người chơi.

Ví dụ:

```text
Terminal 1 → Server
Terminal 2 → Client 1
Terminal 3 → Client 2
Terminal 4 → Client 3
...
```

---

# 🎯 Luồng hoạt động

```text
                    START
                      │
                      ▼
              ┌───────────────┐
              │ Khởi động App │
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │ Kết nối Server│
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │  Tạo / Join   │
              │     Room      │
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │ Đủ người chơi?│
              └───────┬───────┘
                    Yes│
                      ▼
              ┌───────────────┐
              │ Chia vai trò  │
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │  Night Phase  │
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │   Day Phase   │
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │     Vote      │
              └───────┬───────┘
                      │
                      ▼
              ┌───────────────┐
              │ Check Winner  │
              └───────┬───────┘
                      │
                ┌─────┴─────┐
                │           │
              Chưa          Đã thắng
                │           │
                └─────┐     ▼
                      │   END
                      ▼
                  Next Round
```

---

# 🧪 Kiểm thử

Project có thư mục `test` dành cho kiểm thử:

```text
test/
├── client/
│   └── network/
├── integration/
└── server/
```

Các nhóm kiểm thử có thể bao gồm:

### Unit Test

Kiểm tra từng module hoặc class riêng biệt.

Ví dụ:

```text
GameManager
RoomManager
PlayerManager
RoleManager
```

### Network Test

Kiểm tra:

* Client kết nối Server.
* Client ngắt kết nối.
* Gửi message.
* Nhận message.
* Nhiều Client kết nối cùng lúc.

### Integration Test

Kiểm tra toàn bộ luồng:

```text
Client
   ↓
Network
   ↓
Server
   ↓
Game Logic
   ↓
Response
   ↓
Client
```

---

# 📚 Tài liệu dự án

Các tài liệu được lưu trong thư mục:

```text
docs/
```

Bao gồm:

| Thư mục         | Nội dung            |
| --------------- | ------------------- |
| `architecture/` | Kiến trúc hệ thống  |
| `mockup/`       | Thiết kế giao diện  |
| `protocol/`     | Giao thức giao tiếp |
| `test-plan/`    | Kế hoạch kiểm thử   |

---

# 📋 Task Management

File:

```text
Task_Werewolf.xlsx
```

được sử dụng để quản lý và phân chia nhiệm vụ của dự án.

Có thể sử dụng file này để theo dõi:

* Thành viên.
* Sprint.
* Task.
* Trạng thái công việc.
* Tiến độ.
* Phân công nhiệm vụ.

---

# 🔧 Git Workflow

Clone project:

```bash
git clone https://github.com/laquochuy47-create/Game-ma-s-i.git
```

Tạo branch mới:

```bash
git checkout -b feature/ten-feature
```

Kiểm tra thay đổi:

```bash
git status
```

Commit:

```bash
git add .
git commit -m "feat: mô tả thay đổi"
```

Push:

```bash
git push origin feature/ten-feature
```

Sau đó tạo **Pull Request** trên GitHub.

---

# 📝 Quy ước Commit

Khuyến nghị sử dụng Conventional Commits:

```text
feat: thêm chức năng tạo phòng
fix: sửa lỗi kết nối client
docs: cập nhật README
test: thêm integration test
refactor: cải thiện game manager
style: format source code
```

Ví dụ:

```bash
git commit -m "feat: add room creation"
```

---

# 👥 Thành viên

| STT | Thành viên   | Vai trò                     |
| --: | ------------ | --------------------------- |
|   1 | Thành viên 1 | Project Manager / Developer |
|   2 | Thành viên 2 | Backend Developer           |
|   3 | Thành viên 3 | Client Developer            |
|   4 | Thành viên 4 | Network Developer           |
|   5 | Thành viên 5 | Tester                      |
|   6 | Thành viên 6 | Documentation / UI          |

> Thay đổi danh sách thành viên theo nhóm thực tế của dự án.

---

# 📌 Trạng thái dự án

* [x] Khởi tạo repository
* [x] Thiết lập cấu trúc Client / Server / Shared
* [x] Tổ chức tài liệu
* [x] Tổ chức thư mục kiểm thử
* [ ] Hoàn thiện toàn bộ gameplay
* [ ] Hoàn thiện UI
* [ ] Hoàn thiện test
* [ ] Đóng gói phiên bản release

---

# 🐛 Báo lỗi

Nếu phát hiện lỗi, hãy tạo **Issue** trên GitHub và cung cấp:

1. Mô tả lỗi.
2. Các bước tái hiện.
3. Kết quả thực tế.
4. Kết quả mong muốn.
5. Screenshot hoặc log nếu có.

Mẫu:

```text
## Bug Description

Mô tả lỗi:

## Steps to Reproduce

1.
2.
3.

## Expected Result

...

## Actual Result

...

## Environment

OS:
.NET:
Version:
```

---

# 🤝 Đóng góp

Quy trình đóng góp:

```text
Fork / Clone
     ↓
Create Branch
     ↓
Develop
     ↓
Test
     ↓
Commit
     ↓
Push
     ↓
Pull Request
     ↓
Code Review
     ↓
Merge
```

Mọi thay đổi nên được kiểm thử trước khi tạo Pull Request.

---

# 📄 License

Dự án được phát triển cho mục đích **học tập và nghiên cứu**.

---

# 👨‍💻 Repository

**GitHub:**
[Game-ma-s-i – GitHub](https://github.com/laquochuy47-create/Game-ma-s-i?utm_source=chatgpt.com)

---

## ⭐ Mục tiêu dự án

Dự án hướng tới việc xây dựng một game Ma Sói multiplayer hoàn chỉnh, đồng thời giúp nhóm thực hành các kiến thức về:

* Lập trình mạng
* Client – Server
* Socket
* Game State Management
* Software Architecture
* Unit Testing
* Integration Testing
* Git/GitHub
* Agile/Sprint Development

**Made for learning & software engineering practice.**
