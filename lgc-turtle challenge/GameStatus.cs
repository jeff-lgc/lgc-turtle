namespace lgc_turtle_challenge
{
    public enum GameStatus
    {
        Success,   // The exit point was found
        Failed,    // A mine was hit
        Finished,  // Ran out of moves and failed to find exit or hit mine
        Playing    // Still playing    
    }
}