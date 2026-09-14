using System.Collections.Concurrent;
using System.Collections.Generic;
using WerewolfGame.Shared.Model;

namespace WerewolfGame.Server.Logic
{
    public class NightActions
    {
        private readonly ConcurrentDictionary<string, string> wolfVotes = new ConcurrentDictionary<string, string>();
        private string? protectedPlayerId = null;
        private string? seerTargetId = null;
        private string? seerId = null;

        public void AddWolfVote(string wolfId, string targetId)
        {
            wolfVotes[wolfId] = targetId;
        }

        public void SetProtectedPlayer(string targetId)
        {
            protectedPlayerId = targetId;
        }

        public void SetSeerAction(string seerId, string targetId)
        {
            this.seerId = seerId;
            seerTargetId = targetId;
        }

        public void ClearNightActions()
        {
            wolfVotes.Clear();
            protectedPlayerId = null;
            seerTargetId = null;
            seerId = null;
        }

        public string? ResolveNightDeath()
        {
            if (wolfVotes.IsEmpty) return null;

            var voteCounts = new Dictionary<string, int>();
            foreach (var target in wolfVotes.Values)
            {
                voteCounts[target] = voteCounts.GetValueOrDefault(target, 0) + 1;
            }

            string? targetToKill = null;
            int maxVotes = 0;

            foreach (var entry in voteCounts)
            {
                if (entry.Value > maxVotes)
                {
                    maxVotes = entry.Value;
                    targetToKill = entry.Key;
                }
            }

            if (targetToKill != null && targetToKill.Equals(protectedPlayerId))
            {
                return null;
            }

            return targetToKill;
        }

        public string? GetSeerResult(IDictionary<string, Player> players)
        {
            if (seerTargetId == null || seerId == null) return null;

            if (players.TryGetValue(seerTargetId, out var target))
            {
                bool isWolf = (target.Role == Role.WEREWOLF);
                return $"{seerId}:{seerTargetId}:{isWolf.ToString().ToLower()}";
            }

            return null;
        }
    }
}