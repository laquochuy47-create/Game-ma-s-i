using System.Collections.Generic;
using WerewolfGame.Server.Logic;
using WerewolfGame.Shared.Model;

namespace GameLogicTests
{
    public class NightActionsTests
    {
        [Fact]
        public void ResolveNightDeath_WhenWolfVotesTarget_ShouldReturnTarget()
        {
            // Arrange
            var nightActions = new NightActions();

            nightActions.AddWolfVote("wolf1", "player3");
            nightActions.AddWolfVote("wolf2", "player3");

            // Act
            string? result = nightActions.ResolveNightDeath();
            
            // Assert
            Assert.Equal("player3", result);
        }

        [Fact]
        public void ResolveNightDeath_WhenTargetIsProtected_ShouldReturnNull()
        {
            // Arrange
            var nightActions = new NightActions();

            nightActions.AddWolfVote("wolf1", "player3");
            nightActions.AddWolfVote("wolf2", "player3");

            nightActions.SetProtectedPlayer("player3");

            // Act
            string? result = nightActions.ResolveNightDeath();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetSeerResult_WhenTargetIsWerewolf_ShouldReturnTrue()
        {
            // Arrange
            var nightActions = new NightActions();

            var player = new Dictionary<string, Player>
            {
                { "player3", 
                    new Player("player3", "Player 3") 
                    { 
                        Role = Role.WEREWOLF 
                    } 
                }
            };

            nightActions.SetSeerAction("seer1", "player3");

            // Act
            string? result = nightActions.GetSeerResult(player);

            // Assert
            Assert.Equal("seer1:player3:true", result);
        }

        [Fact]
        public void GetSeerResult_WhenTargetIsNotWerewolf_ShouldReturnFalse()
        {
            // Arrange
            var nightActions = new NightActions();

            var player = new Dictionary<string, Player>
            {
                { "player3",
                    new Player("player3", "Player 3")
                    {
                        Role = Role.VILLAGER
                    }
                }
            };

            nightActions.SetSeerAction("seer1", "player3");

            // Act
            string? result = nightActions.GetSeerResult(player);

            // Assert
            Assert.Equal("seer1:player3:false", result);
        }

        [Fact]
        public void GetSeerResult_WhenSeerHasNotActed_ShouldReturnNull()
        {
            // Arrange
            var nightActions = new NightActions();

            var player = new Dictionary<string, Player>
            {
                { "player3",
                    new Player("player3", "Player 3")
                    {
                        Role = Role.VILLAGER
                    }
                }
            };

            // Không gọi SetSeerAction()

            // Act
            string? result = nightActions.GetSeerResult(player);

            // Assert
            Assert.Null(result);
        }
    }
}
