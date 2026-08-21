package server.logic;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import shared.model.Player;
import shared.model.Role;

public class GameManager {
    public enum GamePhase { LOBBY, NIGHT, DAY, VOTING, END_GAME }

    private GamePhase currentPhase = GamePhase.LOBBY;
    private final Map<String, Player> players = new ConcurrentHashMap<>();
    private final DayActions dayActions = new DayActions();
    private final NightActions nightActions = new NightActions();

    public void addPlayer(String id, String name) {
        if (currentPhase == GamePhase.LOBBY) {
            players.put(id, new Player(id, name));
        }
    }

    public boolean isReadyToStart() {
        return players.size() >= 5 && currentPhase == GamePhase.LOBBY;
    }

    public void assignRoles() {
        if (!isReadyToStart()) return;

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

    public synchronized String handleDisconnect(String playerId) {
        Player p = players.get(playerId);
        if (p != null) {
            p.setConnected(false);
            p.setAlive(false);
            return WinConditions.checkWin(players.values());
        }
        return "NO_WINNER_YET";
    }

    public void resetGame() {
        players.clear();
        dayActions.clearVotes();
        nightActions.clearNightActions();
        currentPhase = GamePhase.LOBBY;
    }

    public GamePhase getCurrentPhase() { return currentPhase; }
    public void setCurrentPhase(GamePhase phase) { this.currentPhase = phase; }
    public Map<String, Player> getPlayers() { return players; }
    public DayActions getDayActions() { return dayActions; }
    public NightActions getNightActions() { return nightActions; }
}