namespace lgc_turtle_challenge
{
    internal static class GameEngineFactory
    {
        
        // GameEngineFactory is its own class to allow alternative implementations
        // to be tried with minimum fuss and without the need to change other code.
        // In a more complete example, we might give some sort of hint to the factory
        // (perhaps via settings) as to which engine to use.
        public static IGameEngine Create(GameSettings settings, string[] moves)
        {
            return new DefaultGameEngine(settings, moves);
        }
    }
}