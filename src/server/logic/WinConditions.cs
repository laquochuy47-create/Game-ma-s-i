using System.Collections.Generic;
using WerewolfGame.Shared.Model;

namespace WerewolfGame.Server.Logic
{
    public static class WinConditions
    {
        public static string CheckWin(IEnumerable<Player> players)
        {
            int wolvesCount = 0;
            int villagersCount = 0;

            foreach (var p in players)
            {
                if (p.IsAlive && p.IsConnected)
                {
                    if (p.Role == Role.WEREWOLF) wolvesCount++;
                    else villagersCount++;
                }
            }

            if (wolvesCount == 0) return "VILLAGERS_WIN";
            if (wolvesCount >= villagersCount) return "WEREWOLVES_WIN";
            return "NO_WINNER_YET";
        }
    }
}