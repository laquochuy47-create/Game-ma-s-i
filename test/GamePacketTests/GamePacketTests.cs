using SharedLibrary;
using System.Text.Json;

namespace GamePacketTests
{
    public class GamePacketTests
    {

        // Test case 1 to verify that the Serialize and Deserialize methods work correctly and maintain the same data.
        [Fact]
        public void SerializeDeserialize_ShouldKeepSameData()
        {

            // Arrange
            var originalPacket = new GamePacket
            {
                Action = "CHAT",
                SenderId = "player1",
                Payload = "Hello"
            };

            // Act
            string json = GamePacket.Serialize(originalPacket);
            GamePacket result = GamePacket.Deserialize(json);

            // Assert
            Assert.Equal(originalPacket.Action, result.Action);
            Assert.Equal(originalPacket.SenderId, result.SenderId);
            Assert.Equal(originalPacket.Payload, result.Payload);
        }

        //Test case 2 to verify that the Serialize and Deserialize methods can handle Unicode characters correctly.
        [Fact]
        public void SerializeDeserialize_ShouldHandleUnicodeCharacters()
        {

            // Arrange
            var originalPacket = new GamePacket
            {
                Action = "CHAT",
                SenderId = "người_chơi_01",
                Payload = "Sói đã chọn người chơi 3 🐺 !@#$%^&*()"
            };

            // Act
            string json = GamePacket.Serialize(originalPacket);
            GamePacket result = GamePacket.Deserialize(json);

            // Assert
            Assert.Equal(originalPacket.Action, result.Action);
            Assert.Equal(originalPacket.SenderId, result.SenderId);
            Assert.Equal(originalPacket.Payload, result.Payload);
        }

        //Test case 3 to verify that the Serialize and Deserialize methods can handle empty strings correctly.
        [Fact]
        public void SerializeDeserialize_ShouldHandleEmptyStrings()
        {

            // Arrange 
            var originalPacket = new GamePacket
            {
                Action = "",
                SenderId = "",
                Payload = ""
            };

            // Act
            string json = GamePacket.Serialize(originalPacket);
            GamePacket result = GamePacket.Deserialize(json);

            // Assert
            Assert.Equal("", result.Action);
            Assert.Equal("", result.SenderId);
            Assert.Equal("", result.Payload);
        }

        //Test case 4 to verify that the Serialize method creates a JSON string with the correct format.
        [Fact]
        public void Serialize_ShouldCreateJsonWithCorrectFormat()
        {

            // Arrange
            var packet = new GamePacket
            {
                Action = "VOTE",
                SenderId = "player2",
                Payload = "player5"
            };

            // Act
            string json = GamePacket.Serialize(packet);

            // Assert
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            Assert.Equal("VOTE", root.GetProperty("Action").GetString());
            Assert.Equal("player2", root.GetProperty("SenderId").GetString());
            Assert.Equal("player5", root.GetProperty("Payload").GetString());
        }

        //Test case 5 to verify that the Deserialize method throws a JsonException when given invalid JSON.
        [Fact]
        public void Deserialize_InvalidJson_ShouldThrowJsonException()
        {
           
            // Arrange
            string invalidJson = "{ invalid json }";

            // Act & Assert
            Assert.Throws<JsonException>(() =>
            {
            GamePacket.Deserialize(invalidJson);
            }); 
        }
    }
}
