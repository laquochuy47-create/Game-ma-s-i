using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WerewolfGame.Shared.Model;

namespace WerewolfGame.Server.Logic
{
    public enum GamePhase { LOBBY, NIGHT, DAY, VOTING, END_GAME }

    public class GameManager
    {
        private static readonly Random rng = new Random();

        public GamePhase CurrentPhase { get; set; } = GamePhase.LOBBY;
        public ConcurrentDictionary<string, Player> Players { get; } = new ConcurrentDictionary<string, Player>();
        public DayActions DayActions { get; } = new DayActions();
        public NightActions NightActions { get; } = new NightActions();

        public void AddPlayer(string id, string name)
        {
            if (CurrentPhase == GamePhase.LOBBY)
            {
                Players[id] = new Player(id, name);
            }
        }

        public bool IsReadyToStart()
        {
            return Players.Count >= 5 && CurrentPhase == GamePhase.LOBBY;
        }

        public void AssignRoles()
        {
            if (!IsReadyToStart()) return;

            List<Player> playerList = Players.Values.ToList();
            Shuffle(playerList);

            List<Role> roles = new List<Role>
            {
                Role.WEREWOLF, Role.SEER, Role.BODYGUARD, Role.VILLAGER, Role.VILLAGER
            };

            while (roles.Count < playerList.Count)
            {
                roles.Add(Role.VILLAGER);
            }
            Shuffle(roles);

            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].Role = roles[i];
            }

            CurrentPhase = GamePhase.NIGHT;
        }

        public string AdvancePhase()
        {
            string resultMessage = "";

            switch (CurrentPhase)
            {
                case GamePhase.NIGHT:
                    // Đã thêm ? vào sau string
                    string? nightVictim = NightActions.ResolveNightDeath();
                    if (nightVictim != null)
                    {
                        if (Players.TryGetValue(nightVictim, out var p))
                        {
                            p.IsAlive = false;
                            resultMessage = "Trời đã sáng! Đêm qua " + p.Name + " đã bị sát hại.";
                        }
                    }
                    else
                    {
                        resultMessage = "Trời đã sáng! Đêm qua là một đêm bình yên, không ai chết.";
                    }
                    CurrentPhase = GamePhase.DAY;
                    break;

                case GamePhase.DAY:
                    CurrentPhase = GamePhase.VOTING;
                    resultMessage = "Thời gian thảo luận đã hết. Bắt đầu bỏ phiếu!";
                    break;

                case GamePhase.VOTING:
                    // Đã thêm ? vào sau string
                    string? lynchedPlayer = DayActions.GetLynchedPlayer();
                    if (lynchedPlayer != null)
                    {
                        if (Players.TryGetValue(lynchedPlayer, out var p))
                        {
                            p.IsAlive = false;
                            resultMessage = "Kết thúc bỏ phiếu. " + p.Name + " đã bị treo cổ!";
                        }
                    }
                    else
                    {
                        resultMessage = "Kết thúc bỏ phiếu. Không ai bị treo cổ vì hòa phiếu!";
                    }

                    NightActions.ClearNightActions();
                    DayActions.ClearVotes();
                    CurrentPhase = GamePhase.NIGHT;
                    resultMessage += "\nTrời tối, mọi người đi ngủ!";
                    break;
            }

            string winStatus = WinConditions.CheckWin(Players.Values);
            if (!winStatus.Equals("NO_WINNER_YET"))
            {
                CurrentPhase = GamePhase.END_GAME;
                resultMessage += "\nTRÒ CHƠI KẾT THÚC! " + winStatus;
            }

            return resultMessage;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string HandleDisconnect(string playerId)
        {
            if (Players.TryGetValue(playerId, out var p))
            {
                p.IsConnected = false;
                p.IsAlive = false;
                return WinConditions.CheckWin(Players.Values);
            }
            return "NO_WINNER_YET";
        }

        public void ResetGame()
        {
            Players.Clear();
            DayActions.ClearVotes();
            NightActions.ClearNightActions();
            CurrentPhase = GamePhase.LOBBY;
        }

        private static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}