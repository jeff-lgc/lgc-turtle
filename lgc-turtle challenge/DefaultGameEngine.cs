using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lgc_turtle_challenge
{

    internal class GameInvalidMoveException: Exception
    {
        // Implementation elided for brevity        
    }

    internal class GameInvalidMoveInstructionException : Exception
    {
        // Implementation elided for brevity        
    }

    internal class GameHitMineException : Exception
    {
        // Implementation elided for brevity        
    }

    internal class GameExitNotFoundException : Exception
    {
        // Implementation elided for brevity        
    }

    internal class GameSuccessException : Exception
    {
        // A slight contradiction in terms!
    }
    
    internal class Turtle
    {
        private enum Direction
        {
            North,
            East,
            South,
            West
        }

        private readonly int[] Directions =
        {
            (int) Direction.North,
            (int) Direction.East,
            (int) Direction.South,
            (int) Direction.West
        };

        private int _currentDirection = 0;
        
        public readonly Position CurrentPosition;
        
        // Maps direction to handler functions. 
        delegate void MoveMethod();
        private readonly Dictionary<Direction, MoveMethod> _moveMap;
        
        public Turtle(Position startPosition)
        {
            CurrentPosition = startPosition;

            // Create a map of Direction to a handler function that updates 
            // the current position.
            // Preferred this over a switch statement in the game loop,
            // as it allows for composing new directions (e.g. NorthWest etc.)
            
            _moveMap = new Dictionary<Direction, MoveMethod>()
            {
                {Direction.North, MoveNorth},
                {Direction.East, MoveEast},
                {Direction.South, MoveSouth},
                {Direction.West, MoveWest}
            };

        }

        public void Rotate()
        {
            // Ensure the direction rotates from West to North correctly;
            _currentDirection++;
            _currentDirection %= 4;

        }

        public void Move()
        {
            var moveMethod = _moveMap[(Direction)_currentDirection];
            moveMethod();                        
        }

        private void MoveNorth()
        {
            CurrentPosition.Y++;
        }
        
        private void MoveSouth()
        {
            CurrentPosition.Y--;
        }
        
        private void MoveEast()
        {
            CurrentPosition.X++;
        }
        
        private void MoveWest()
        {
            CurrentPosition.X--;
        }
        
    }


    public class DefaultGameEngine : IGameEngine
    {
        private enum PositionType
        {
            Exit,
            Mine,
            Safe
        }
        
        private GameSettings Settings { get; }
        private string[] Moves { get; }

        private readonly PositionType[,] _gameBoard; 

        public DefaultGameEngine(GameSettings settings, string[] moves)
        {
            Settings = settings;
            Moves = moves;
            
            // TODO: In a real world application there would be some 
            // sanity checking of the settings: namely that all specified 
            // positions are within the dimensions specified by the board.
            // For this default implementation, it is assumed all data is ok.
            
            // Initialize the game board
            // First set all the elements to 0 (a safe square)
            _gameBoard = new PositionType[settings.BoardSize.X, settings.BoardSize.Y];
            for (var x = 0; x < settings.BoardSize.X; x++)
            {
                for (var y = 0; y < settings.BoardSize.Y; y++)
                {
                    _gameBoard[x,y] = PositionType.Safe;
                }
            }
            
            // Now set the exit point
            _gameBoard[settings.ExitPosition.X, settings.ExitPosition.Y] = PositionType.Exit;
            
            // Finally, set the mine positions
            foreach (Position mine in settings.Mines)
            {
                _gameBoard[mine.X, mine.Y] = PositionType.Mine;
            }
            

        }
        
        public GameStatus Play()
        {
            const string move = "m";
            const string rotate = "r";
            
            GameStatus status = GameStatus.Playing;
            Turtle turtle = new Turtle(Settings.StartPosition);

            try
            {
                foreach (var theMove in Moves)
                {
                    switch (theMove.ToLower())
                    {
                        case move:
                            turtle.Move();
                            break;

                        case rotate:
                            turtle.Rotate();
                            break;

                        default:
                            // For this default implementation, throw an exception for
                            // an instruction that is not recognised.
                            // In a more forgiving version, the invalid instruction 
                            // could be ignored.
                            throw new GameInvalidMoveInstructionException();
                    }

                    // Check for valid position. Separate tests in X and Y direction for clarity/simpler logic

                    if (turtle.CurrentPosition.X < 0 || turtle.CurrentPosition.X == Settings.BoardSize.X)
                    {
                        throw new GameInvalidMoveException();
                    }

                    if (turtle.CurrentPosition.Y < 0 || turtle.CurrentPosition.Y == Settings.BoardSize.Y)
                    {
                        throw new GameInvalidMoveException();
                    }

                    // The position is valid, so update the game state as appropriate

                    PositionType currentPositionType =
                        _gameBoard[turtle.CurrentPosition.X, turtle.CurrentPosition.Y];

                    switch (currentPositionType)
                    {
                        case PositionType.Safe:
                            // Just continue with the next move
                            continue;

                        case PositionType.Mine:
                            throw new GameHitMineException();

                        case PositionType.Exit:
                            // The exit has been found!
                            
                            // This is counter-intuitive and there is a valid
                            // argument that making "success" an exception, is
                            // the wrong thing to do. However, it does reduce the need
                            // for checks at the end of the loop. See comments below.
                            throw new GameSuccessException();

                        default:
                            Debug.Assert(false, "A new PositionType has not been handled");
                            throw new Exception();
                    }

                    // Below are two commented out sections of code.
                    // By making "success" an exception, we don't need to 
                    // carry out these checks here. It makes the code simpler.
                    // Left in here for explanation only.
                    
                    // if (status != GameStatus.Playing)
                    // {
                    //     break;
                    // }
                }

                // if (status == GameStatus.Playing)
                // {
                //     throw new GameExitNotFoundException();
                // }

                // The game is over, but the exit has not been found
                status = GameStatus.Finished;

            }
            catch (GameHitMineException)
            {
                status = GameStatus.Failed;
            }
            catch (GameSuccessException)
            {
                status = GameStatus.Success;
            }

            return status;
        }
    }
}