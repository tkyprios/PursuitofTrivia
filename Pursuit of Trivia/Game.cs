using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.NetworkInformation;

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
                DisplayTurnOptions();

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
                        if (HandleTriviaQuestion()) turnComplete = true;
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

            // CHECK WINNING CONDITION

            GameTable.GetNextPlayer();


        }


        /// <summary>
        /// Determines whether a valid question category has been selected by the user and if it has, proceeds to ask a question for that category.
        /// </summary>
        /// <returns> A boolean value on whether a value category has been selected. </returns>
        private bool HandleTriviaQuestion()
        {
            var category = SelectCategory(); // category is of type 'Category?'
            if (category.HasValue) // check if we have value
            {
                PlayTriviaQuestion(category.Value); // Must use .Value to get non-nullable value
                return true;
            }
            else
            {
                Console.Clear();
                return false; // return to menu
            }    
  
        }

        /// <summary>
        /// Prompts the user to select a category from a list of options and returns the selected category.
        /// </summary>
        /// <remarks>Displays a list of category options and repeatedly prompts the user until
        /// a valid selection is made. If the user selects 0, the method returns <see langword="null"/>. Otherwise, it
        /// returns the selected category as a <see cref="Category"/>.</remarks>
        /// <returns>The selected <see cref="Category"/>, or <see langword="null"/> if the user selects 0.</returns>
        private Category? SelectCategory()
        {
            while (true)
            {
                Console.Clear();
                DisplayCategoryOptions();

                if (!TryGetCategorySelection(out int selection))
                {
                    Console.WriteLine("Please enter a valid category number.");
                    Console.ReadKey();
                    continue;
                }

                if (selection == 0)
                    return null;

                return (Category)selection; // because selection is an int

            }
        }

        /// <summary>
        /// Attempts to get a valid category selection from the player.
        /// </summary>
        /// <param name="category">When this method returns, contains the selected Category if input was valid, 
        /// or 0 if the player chose to return.</param>
        /// <returns>true if the input was valid; otherwise, false.</returns>
        private bool TryGetCategorySelection(out int category)
        {
            category = 0;
            var input = Console.ReadLine();

            // First check if we can parse the input as a number
            if (!int.TryParse(input, out category))
            {
                return false;
            }

            // Check if it's a valid category number or the return option (0)
            if (category == 0 || Enum.IsDefined(typeof(Category), category))
            {
                return true;
            }

            return false;
        }

        
        /// <summary>
        /// Begins presentation of question, through drawing a card and presenting to current player. Finally validates the player's answer.
        /// </summary>
        /// <param name="category"> The category to draw the question from. </param>
        /// <exception cref="InvalidOperationException"> Throws if no card could be sourced. </exception>
        private void PlayTriviaQuestion(Category category)
        {
            if (GameTable.MasterQuestionDeck.IsDeckEmpty(category))
            {
                throw new InvalidOperationException($"Cannot draw a card: The {category} deck is empty.");
            }

            var card = GameTable.MasterQuestionDeck.DrawQuestionCard(category);
            var player = GameTable.GetCurrentPlayer();

            if (card != null)
            {
                DisplayQuestionAndChoices(card);
                ProcessAnswer(card, player);
            }

            else
            {
                throw new InvalidOperationException("No card retrieved from the selected category.");
            }
            

        }

        /// <summary>
        /// Presents the question to the player and the potential choices.
        /// </summary>
        /// <param name="card"> The card that contains the question to be displayed. </param>
        private void DisplayQuestionAndChoices(Card card)
        {
            Console.Clear();
            Console.WriteLine($"Category: {card.QuestionCategory}");
            Console.WriteLine($"Question: {card.QuestionText}\n");

            for (int i = 0; i < card.Choices.Count; i++)
            {
                Console.WriteLine($"{(char)('A' + i)}) {card.Choices[i]}"); // using ASCII character arithmetic, iterate through each choice
            }
        }

        /// <summary>
        /// Validates/checks the answer that the player provides. 
        /// </summary>
        /// <remarks> First attempts to read the answer the player provides and checks if it is a valid letter.
        /// Then converts the letter value to an index using ASCII arithmetic relative to the letter 'A'.
        /// Then checks the answer.
        /// </remarks>
        /// <param name="card"></param>
        /// <param name="player"></param>
        private void ProcessAnswer(Card card, Player player)
        {
            while (true)
            {
                Console.Write("Answer (A-D): ");

                if (!TryGetLetterChoice(out char letter))
                {
                    Console.WriteLine("Please enter a valid letter (A-D).");
                    continue;
                }


                // Convert letter to index, where A = 0, B = 1 etc. 
                // Uses 'A' as the reference, where A is the first choice
                int choiceIndex = letter - 'A'; // Using ASCII character arithemtic

                // Get the chosen answer using the index
                string selectedAnswer = card.Choices[choiceIndex];

                if (card.CheckAnswer(selectedAnswer))
                {
                    Console.WriteLine($"Correct! {player.Name} earned this card and 1 point towards category '{card.QuestionCategory}'.");
                    player.AddCard(card);
                }

                else
                {
                    Console.WriteLine($"Incorrect! {player.Name} failed to earn this card. Returning to bottom of deck...");
                    GameTable.MasterQuestionDeck.ReturnCardToBottom(card);
                }

                Console.WriteLine("\nPress any key to end turn...");
                Console.ReadKey(true);
                break;
            }
            

        }

        /// <summary>
        /// Checks if a given input is a valid letter choice (A-D)
        /// </summary>
        /// <param name="letter"> The letter to output to. </param>
        /// <returns> True if a valid letter choice, false if not. </returns>
        private bool TryGetLetterChoice(out char letter)
        {
            // Validate user input is a valid letter choice
            letter = '\0'; // null/invalid character for default

            var input = Console.ReadLine()?.Trim().ToUpper();

            // Check if input is exactly one character
            if (string.IsNullOrEmpty(input) || input.Length != 1)
                return false;

            letter = input[0]; // then assign input to the letter.

            return letter <= 'A' && letter <= 'D'; // check if input is a valid letter.
        }

        private void DisplayCategoryOptions()
        {
            Console.WriteLine("What question category would you like to draw?");
            foreach (Category category in Enum.GetValues(typeof(Category)))
            {
                int remainingCards = GameTable.MasterQuestionDeck.GetRemainingCardCount(category);
                Console.WriteLine($"{(int)category}. {category} ({remainingCards} questions in deck)");
            }
            Console.WriteLine("0. Return/Back");
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
            Console.WriteLine("3. Steal an Opponent's Card");
            Console.WriteLine("0. Forfeit/Quit");
        }




    }
}
