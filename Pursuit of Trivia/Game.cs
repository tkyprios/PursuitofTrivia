using System;
using System.Collections.Generic;
using System.Linq;

namespace Pursuit_of_Trivia
{
    /// <summary>
    /// Class representing the overall game state, including setting up players (and bots) and initializing the game table with the master question deck.
    /// Implements the "how" (turn logic, game flow, win conditions)
    /// </summary>
    internal class Game
    {
        public Table GameTable { get; set; }

        /// <summary>
        /// Constructor for the Game class, initializes the game table with a master deck of questions covering all categories.
        /// </summary>
        public Game() 
        {
            GameTable = new Table(new Deck(GetAllQuestionCategories()));
        }

        /// <summary>
        /// Retrieves a list of all available question categories.
        /// </summary>
        /// <remarks>The method returns all values of the <see cref="Category"/> enumeration as a list. 
        /// <returns>A list of all categories defined in the <see cref="Category"/> enumeration.</returns>
        private List<Category> GetAllQuestionCategories() => 
            Enum.GetValues(typeof(Category)).Cast<Category>().ToList();

        /// <summary>
        /// Sets up the players for the game by prompting the user for their username and initializing the current
        /// player and a bot player.
        /// </summary>
        private void SetupPlayers()
        {
            Console.Clear();

            Console.WriteLine("Let's get you set up!");

            Console.WriteLine("What would you like your username to be?");
            string playerUsername = Console.ReadLine();

            var player = new Player(playerUsername);
            Console.WriteLine($"Welcome, {player.Name}! Press any key to start the game...");
            Console.ReadKey(true);
            
            GameTable.AddPlayerToGame(player);
            GameTable.AddBotToGame();
        }

        public void StartGame()
        {
            SetupPlayers();
            bool gameRunning = true;

            while (gameRunning)
            {
                ProcessTurn();

                // Check win condition

                // Handle game end
            }
        }


        private void ProcessTurn()
        {
            Console.Clear();
            var currentPlayer = GameTable.GetCurrentPlayer();
            Console.WriteLine($"It's {currentPlayer}'s turn!");

            bool turnComplete = false;

            while (!turnComplete)
            {
                DisplayTurnOptions(); // Should this be in this function or in a StartGame()?

                if (!int.TryParse(Console.ReadLine(), out int playerSelection))
                {
                    Console.WriteLine("Please enter a valid number.");
                    Console.Clear();
                    DisplayTurnOptions();
                }

                switch (playerSelection)
                {
                    case 0:
                        Console.WriteLine($"Player {currentPlayer} has forfeited. Exiting game...");
                        Environment.Exit(0); // use of gameRunning?
                        return;
                    case 1:
                        HandleDrawCard();
                        turnComplete = true;
                        break;
                    case 2:
                        DisplayScores();
                        break;
                    //case 3:
                    //  HandleStealCard()
                    //  turnComplete = true;
                    //  break;
                    default:
                        Console.WriteLine("Invalid input. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }



            GameTable.GetNextPlayer();


        }

        private void HandleDrawCard()
        {
            // Category selection
            Console.WriteLine("What question category would you like to draw?");

            // Question presentation

            // Answer validation

            // Score updating - UpdateScores()
        }

        private void DisplayScores()
        {
            throw new NotImplementedException();

            // Each player's category scores
            // Cards earned
            // Progress towards winning
        }



        private void DisplayTurnOptions()
        {
            Console.WriteLine($"What would you like to do?");
            Console.WriteLine("1. Draw a Question Card");
            Console.WriteLine("2. Show Scores");
            Console.WriteLine("0. Forfeit/Quit");
        }




    }
}
