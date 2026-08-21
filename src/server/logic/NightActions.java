package server.logic;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class NightActions {
    private final Map<String, String> werewolfVotes = new ConcurrentHashMap<>();
    private String protectedPlayerId = null;
    private String seerTargetId = null;

    public void recordWerewolfVote(String wolfId, String targetId) {
        werewolfVotes.put(wolfId, targetId);
    }

    public void setProtectedPlayerId(String targetId) {
        this.protectedPlayerId = targetId;
    }

    public void setSeerTargetId(String targetId) {
        this.seerTargetId = targetId;
    }

    public String getSeerTargetId() { return seerTargetId; }

    public String processNightResults() {
        if (werewolfVotes.isEmpty()) return null;

        Map<String, Integer> targetCounts = new ConcurrentHashMap<>();
        for (String targetId : werewolfVotes.values()) {
            targetCounts.put(targetId, targetCounts.getOrDefault(targetId, 0) + 1);
        }

        String killedTarget = null;
        int maxVotes = 0;
        for (Map.Entry<String, Integer> entry : targetCounts.entrySet()) {
            if (entry.getValue() > maxVotes) {
                maxVotes = entry.getValue();
                killedTarget = entry.getKey();
            }
        }

        if (killedTarget != null && killedTarget.equals(protectedPlayerId)) {
            return null; // Được Bảo vệ cứu thành công
        }
        return killedTarget;
    }

    public void clearNightActions() {
        werewolfVotes.clear();
        protectedPlayerId = null;
        seerTargetId = null;
    }
}
