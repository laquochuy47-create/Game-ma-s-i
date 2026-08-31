package server.logic;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class DayActions {
    private final Map<String, String> votes = new ConcurrentHashMap<>();

    public void addVote(String voterId, String targetId) {
        votes.put(voterId, targetId);
    }

    public String getLynchedPlayer() {
        if (votes.isEmpty()) return null;
        
        Map<String, Integer> voteCounts = new ConcurrentHashMap<>();
        for (String targetId : votes.values()) {
            voteCounts.put(targetId, voteCounts.getOrDefault(targetId, 0) + 1);
        }

        String mostVoted = null;
        int maxVotes = 0;
        boolean isTie = false;

        for (Map.Entry<String, Integer> entry : voteCounts.entrySet()) {
            if (entry.getValue() > maxVotes) {
                maxVotes = entry.getValue();
                mostVoted = entry.getKey();
                isTie = false;
            } else if (entry.getValue() == maxVotes) {
                isTie = true;
            }
        }
        
        return isTie ? null : mostVoted;
    }

    public void clearVotes() {
        votes.clear();
    }
}