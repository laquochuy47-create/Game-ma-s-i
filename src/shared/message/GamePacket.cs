using System;
using System.Text.Json;

namespace SharedLibrary
{
    public class GamePacket
    {
        public string Action { get; set; }
        public string SenderId { get; set; }
        public string Payload { get; set; }

        public static string Serialize(GamePacket packet)
        {
            return JsonSerializer.Serialize(packet);
        }

        public static GamePacket Deserialize(string json)
        {
            return JsonSerializer.Deserialize<GamePacket>(json);
        }
    }
}
