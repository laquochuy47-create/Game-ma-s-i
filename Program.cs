using System;
using WerewolfGame.Server.Logic;

namespace WerewolfGame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager game = new GameManager();

            // 1. Thêm 5 người chơi
            game.AddPlayer("1", "An");
            game.AddPlayer("2", "Bình");
            game.AddPlayer("3", "Cường");
            game.AddPlayer("4", "Dũng");
            game.AddPlayer("5", "Giang");

            game.AssignRoles();
            Console.WriteLine("=== BẮT ĐẦU VÁN CHƠI ===");
            foreach (var player in game.Players.Values)
            {
                Console.WriteLine($"- {player.Name} (ID: {player.Id}): {player.Role}");
            }

            // 2. MÔ PHỎNG ĐÊM 1 (NIGHT)
            Console.WriteLine("\n--- Ban đêm bắt đầu ---");
            // Giả sử ID 4 (Dũng - Ma sói) chọn cắn ID 1 (An)
            game.NightActions.AddWolfVote("4", "1");
            // Giả sử Bảo vệ chọn bảo vệ ID 2 (Bình) -> An không được bảo vệ
            game.NightActions.SetProtectedPlayer("2"); 

            // Chuyển sang ngày
            string dayResult = game.AdvancePhase();
            Console.WriteLine(dayResult);

            // 3. MÔ PHỎNG NGÀY 1 (DAY -> VOTING)
            string votingStartResult = game.AdvancePhase();
            Console.WriteLine($"\n{votingStartResult}");

            // Mọi người bỏ phiếu treo cổ ID 4 (Ma Sói)
            game.DayActions.AddVote("2", "4");
            game.DayActions.AddVote("3", "4");
            game.DayActions.AddVote("5", "4");

            // Kết thúc bỏ phiếu & kiểm tra kết quả
            string nightStartResult = game.AdvancePhase();
            Console.WriteLine($"\n{nightStartResult}");
        }
    }
}