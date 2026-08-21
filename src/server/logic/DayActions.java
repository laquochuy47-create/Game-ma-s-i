package server.logic;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class DayActions {
    // Dùng ConcurrentHashMap để không bị lỗi khi 4 người cùng vote 1 lúc
    private final Map<String, String> votes = new ConcurrentHashMap<>();

    public void processVote(String voterId, String targetId) {
        votes.put(voterId, targetId);
    }

    public String getEliminatedPlayerId() {
        if (votes.isEmpty()) return null;
        Map<String, Integer> voteCounts = new ConcurrentHashMap<>();
        for (String targetId : votes.values()) {
            voteCounts.put(targetId, voteCounts.getOrDefault(targetId, 0) + 1);
        }

        String mostVoted = null;
        int maxVotes = 0;
        for (Map.Entry<String, Integer> entry : voteCounts.entrySet()) {
            if (entry.getValue() > maxVotes) {
                maxVotes = entry.getValue();
                mostVoted = entry.getKey();
            }
        }
        return mostVoted; // Trả về ID người bị treo cổ
    }
}