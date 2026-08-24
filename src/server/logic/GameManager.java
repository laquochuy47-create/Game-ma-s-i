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
        
        while (roles.size() < playerList.size()) {
            roles.add(Role.VILLAGER);
        }
        Collections.shuffle(roles);

        for (int i = 0; i < playerList.size(); i++) {
            playerList.get(i).setRole(roles.get(i));
        }
        
        currentPhase = GamePhase.NIGHT;
    }

    public String advancePhase() {
        String resultMessage = "";
        switch (currentPhase) {
            case NIGHT -> {
                String nightVictim = nightActions.resolveNightDeath();
                if (nightVictim != null) {
                    Player p = players.get(nightVictim);
                    if (p != null) {
                        p.setAlive(false);
                        resultMessage = "Trời đã sáng! Đêm qua " + p.getName() + " đã bị sát hại.";
                    }
                } else {
                    resultMessage = "Trời đã sáng! Đêm qua là một đêm bình yên, không ai chết.";
                }
                currentPhase = GamePhase.DAY;
            }
                
            case DAY -> {
                currentPhase = GamePhase.VOTING;
                resultMessage = "Thời gian thảo luận đã hết. Bắt đầu bỏ phiếu!";
            }
                
            case VOTING -> {
                String lynchedPlayer = dayActions.getLynchedPlayer();
                if (lynchedPlayer != null) {
                    Player p = players.get(lynchedPlayer);
                    if (p != null) {
                        p.setAlive(false);
                        resultMessage = "Kết thúc bỏ phiếu. " + p.getName() + " đã bị treo cổ!";
                    }
                } else {
                    resultMessage = "Kết thúc bỏ phiếu. Không ai bị treo cổ vì hòa phiếu!";
                }
                
                nightActions.clearNightActions();
                dayActions.clearVotes();
                currentPhase = GamePhase.NIGHT;
                resultMessage += "\nTrời tối, mọi người đi ngủ!";
            }
                
            default -> {
            }
        }

        String winStatus = WinConditions.checkWin(new ArrayList<>(players.values()));
        if (!winStatus.equals("NO_WINNER_YET")) {
            currentPhase = GamePhase.END_GAME;
            resultMessage += "\nTRÒ CHƠI KẾT THÚC! " + winStatus;
        }

        return resultMessage;
    }

    public synchronized String handleDisconnect(String playerId) {
        Player p = players.get(playerId);
        if (p != null) {
            p.setConnected(false);
            p.setAlive(false);
            return WinConditions.checkWin(new ArrayList<>(players.values()));
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