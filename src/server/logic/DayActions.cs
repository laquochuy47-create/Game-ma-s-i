using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WerewolfGame.Server.Logic
{
    public class DayActions
    {
        private readonly ConcurrentDictionary<string, string> votes = new ConcurrentDictionary<string, string>();

        public void AddVote(string voterId, string targetId)
        {
            votes[voterId] = targetId;
        }

        public string? GetLynchedPlayer()
        {
            if (votes.IsEmpty) return null;

            var voteCounts = new ConcurrentDictionary<string, int>();
            foreach (var targetId in votes.Values)
            {
                voteCounts.AddOrUpdate(targetId, 1, (key, oldValue) => oldValue + 1);
            }

            string? mostVoted = null;
            int maxVotes = 0;
            bool isTie = false;

            foreach (var entry in voteCounts)
            {
                if (entry.Value > maxVotes)
                {
                    maxVotes = entry.Value;
                    mostVoted = entry.Key;
                    isTie = false;
                }
                else if (entry.Value == maxVotes)
                {
                    isTie = true;
                }
            }

            return isTie ? null : mostVoted;
        }

        public void ClearVotes()
        {
            votes.Clear();
        }
    }
}