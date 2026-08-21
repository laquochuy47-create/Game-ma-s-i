package server.logic;

import shared.model.Player;
import shared.model.Role;
import java.util.Collection;

public class WinConditions {
    public static String checkWin(Collection<Player> players) {
        int wolvesCount = 0;
        int villagersCount = 0;

        for (Player p : players) {
            if (p.isAlive() && p.isConnected()) {
                if (p.getRole() == Role.WEREWOLF) wolvesCount++;
                else villagersCount++;
            }
        }

        if (wolvesCount == 0) return "VILLAGERS_WIN";
        if (wolvesCount >= villagersCount) return "WEREWOLVES_WIN";
        return "NO_WINNER_YET";
    }
}