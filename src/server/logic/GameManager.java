package server.logic;

import shared.model.Player;
import shared.model.Role;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

public class GameManager {
    public enum GamePhase { LOBBY, NIGHT, DAY, VOTING, END_GAME }

    private GamePhase currentPhase = GamePhase.LOBBY;
    private final Map<String, Player> players = new ConcurrentHashMap<>();
    private final DayActions dayActions = new DayActions();

    public void addPlayer(String id, String name) {
        players.put(id, new Player(id, name));
    }

    // TC_01: Chia vai ngẫu nhiên (Chuẩn 5 người)
    public void assignRoles() {
        List<Player> playerList = new ArrayList<>(players.values());
        Collections.shuffle(playerList);

        List<Role> roles = new ArrayList<>(Arrays.asList(
            Role.WEREWOLF, Role.SEER, Role.BODYGUARD, Role.VILLAGER, Role.VILLAGER
        ));
        Collections.shuffle(roles);

        for (int i = 0; i < 5; i++) {
            playerList.get(i).setRole(roles.get(i));
        }
        currentPhase = GamePhase.NIGHT;
    }

    // TC_04: Xử lý khi có người tắt app ngang
    public synchronized String handleDisconnect(String playerId) {
        Player p = players.get(playerId);
        if (p != null) {
            p.setConnected(false);
            p.setAlive(false);
            return WinConditions.checkWin(players.values());
        }
        return "NO_WINNER_YET";
    }

    public GamePhase getCurrentPhase() { return currentPhase; }
    public void setCurrentPhase(GamePhase phase) { this.currentPhase = phase; }
    public Map<String, Player> getPlayers() { return players; }
    public DayActions getDayActions() { return dayActions; }
}