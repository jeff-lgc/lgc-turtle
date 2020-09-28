using System.IO;
using Newtonsoft.Json;

namespace lgc_turtle_challenge
{
    public sealed class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class GameSettings
    {
        public Position BoardSize { get; set; }
        public Position StartPosition { get; set; }
        public Position ExitPosition { get; set; }
        
        public Position[] Mines { get; set; }

        public static GameSettings Create(string settingsFile)
        {
            string text = File.ReadAllText(settingsFile);
            var settings = JsonConvert.DeserializeObject<GameSettings>(text);
            return settings;
        } 
    
    
    }
}