using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lgc_turtle_challenge
{
    class Program
    {
        static int Main(string[] args)
        {

            // TODO: Validate that command line has the required parameters
            
            // The game settings are stored as JSON and are deserialized
            // into a type safe class.
            GameSettings gameSettings = GameSettings.Create(args[0]);

            // The list of moves is a text file
            // consisting of "f" for move forward, "r" for rotate clockwise
            // 90 degrees.
            string[] gameMoves = GameMoves.Create(args[1]);

            // Create an instance of the game engine and play the game.
            // A separate factory class is used so that alternative
            // engine implementations can be supplied.
            IGameEngine gameEngine = GameEngineFactory.Create(gameSettings, gameMoves);
            GameStatus gameStatus = gameEngine.Play();            

            // Decouple the game result from the message displayed.
            // In the real world this would allow for localization.
            Dictionary<GameStatus, string> statusToDisplayMap = new Dictionary<GameStatus, string>()
            {
                {GameStatus.Success, "SUCCESS: The exit was found"},
                {GameStatus.Failed, "FAILED: A mine was hit!"},
                {GameStatus.Finished, "FAILED: Ran out of moves - still lost!"},
                {GameStatus.Playing, "FAILED: Still playing!"} 
            };

            // This should never ever happen.
            Debug.Assert(gameStatus != GameStatus.Playing);
            
            // Display an appropriate result to the user
            Console.WriteLine($"{statusToDisplayMap[gameStatus]}");

            // Always return a meaningful exit code to the OS
            return (int) gameStatus;
        }
        
  
        
        
    }
}