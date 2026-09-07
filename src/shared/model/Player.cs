namespace WerewolfGame.Shared.Model
{
    public class Player
    {
        public string Id { get; }
        public string Name { get; }
        public Role Role { get; set; }
        public bool IsAlive { get; set; } = true;
        public bool IsConnected { get; set; } = true;

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}