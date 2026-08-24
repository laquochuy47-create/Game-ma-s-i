namespace GameClient.Model
{
    public class PlayerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; }
    }

    public class GameStateModel
    {
        public string Phase { get; set; } // "DAY", "NIGHT", "VOTING"
        public int TimeRemaining { get; set; }
        public List<PlayerModel> Players { get; set; }
    }
}