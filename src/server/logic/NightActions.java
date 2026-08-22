package server.logic;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import shared.model.Player;
import shared.model.Role;

public class NightActions {
    private final Map<String, String> wolfVotes = new ConcurrentHashMap<>();
    private String protectedPlayerId = null;
    private String seerTargetId = null;
    private String seerId = null;

    public void addWolfVote(String wolfId, String targetId) {
        wolfVotes.put(wolfId, targetId);
    }

    public void setProtectedPlayer(String targetId) {
        this.protectedPlayerId = targetId;
    }

    public void setSeerAction(String seerId, String targetId) {
        this.seerId = seerId;
        this.seerTargetId = targetId;
    }

    public void clearNightActions() {
        wolfVotes.clear();
        protectedPlayerId = null;
        seerTargetId = null;
        seerId = null;
    }

    public String resolveNightDeath() {
        if (wolfVotes.isEmpty()) return null;

        Map<String, Integer> voteCounts = new HashMap<>();
        for (String target : wolfVotes.values()) {
            voteCounts.put(target, voteCounts.getOrDefault(target, 0) + 1);
        }

        String targetToKill = null;
        int maxVotes = 0;

        for (Map.Entry<String, Integer> entry : voteCounts.entrySet()) {
            if (entry.getValue() > maxVotes) {
                maxVotes = entry.getValue();
                targetToKill = entry.getKey();
            }
        }

        if (targetToKill != null && targetToKill.equals(protectedPlayerId)) {
            return null;
        }

        return targetToKill;
    }

    public String getSeerResult(Map<String, Player> players) {
        if (seerTargetId == null || seerId == null) return null;
        
        Player target = players.get(seerTargetId);
        if (target != null) {
            boolean isWolf = (target.getRole() == Role.WEREWOLF);
            return seerId + ":" + seerTargetId + ":" + isWolf;
        }
        return null;
    }
}