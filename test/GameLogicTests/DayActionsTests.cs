using WerewolfGame.Server.Logic;

namespace GameLogicTests
{
    public class DayActionsTests
    {
        [Fact]
        public void GetLynchedPlayer_PlayerWithMostVotes_ShouldBeLynched()
        {

            // Arrange
            var dayActions = new DayActions();
            
            dayActions.AddVote("player1", "player3");
            dayActions.AddVote("player2", "player3");
            dayActions.AddVote("player3", "player1");
            dayActions.AddVote("player4", "player3");

            // Act
            string? result = dayActions.GetLynchedPlayer();

            // Assert
            Assert.Equal("player3", result); 
        }

        [Fact]
        public void GetLynchedPlayer_WhenVotesAreTied_ShouldReturnNull()
        {
            // Arrange
            var dayActions = new DayActions();

            dayActions.AddVote("player1", "player3");
            dayActions.AddVote("player2", "player3");

            dayActions.AddVote("player3", "player1");
            dayActions.AddVote("player4", "player1");

            // Act
            string? result = dayActions.GetLynchedPlayer();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLynchedPlayer_WhenNoVotesAreCast_ShouldReturnNull()
        {
            // Arrange
            var dayActions = new DayActions();

            // Act
            string? result = dayActions.GetLynchedPlayer();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetlynchedPlayer_WhenPlayerChangesVote_ShouldCountOnlyLatestVote() 
        {
            // Arrange
            var dayActions = new DayActions();

            dayActions.AddVote("player1", "player2");
            dayActions.AddVote("player2", "player3");

            dayActions.AddVote("player3", "player1");
            dayActions.AddVote("player4", "player1");

            // player1 đổi phiếu từ player2 sang player3
            dayActions.AddVote("player1", "player3");

            // Act
            string? result = dayActions.GetLynchedPlayer();

            // Assert
            Assert.Null(result); // Không có ai bị treo cổ vì phiếu bầu bị chia đều
        }

        [Fact]
        public void GetLynchedPlayer_AfterClearVotes_ShouldReturnNull()
        {
            // Arrange
            var dayActions = new DayActions();

            dayActions.AddVote("player1", "player2");
            dayActions.AddVote("player2", "player3");

            dayActions.AddVote("player3", "player1");
            dayActions.AddVote("player4", "player1");

            // Xóa tất cả phiếu bầu
            dayActions.ClearVotes();
            
            // Act
            string? result = dayActions.GetLynchedPlayer();

            // Assert
            Assert.Null(result); // Không có ai bị treo cổ vì không có phiếu bầu nào
        }
    }
}
