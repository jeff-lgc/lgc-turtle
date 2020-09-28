using System;
using System.Collections.Generic;
using System.IO;

namespace lgc_turtle_challenge
{
    internal static class GameMoves
    {
        public static string[] Create(string movesFile)
        {
            string[] moves = File.ReadAllLines(movesFile);
            return moves;
        }
        
        // FUTURE: Could provide other methods for generating games moves.
        // For example:
        //    random generation
        //    a learning engine that takes a previous sets of moves to try a new solution

        // For example only
        public static string[] CreateRandomMoves()
        {
            int moves = new Random().Next(5, 50);
            
            var theMoves = new Stack<string>(moves);
            Random random = new Random();
            for (int n = 0; n < moves; n++)
            {
                int a = random.Next(1, 100);
                string action = a <= 50 ? "m" : "r";
                theMoves.Push(action);
            }

            return theMoves.ToArray();
        }
    }
}