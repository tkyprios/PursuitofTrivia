using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Pursuit_of_Trivia.Interfaces;
using Pursuit_of_Trivia.Configuration;
using Pursuit_of_Trivia.Extensions;

namespace Pursuit_of_Trivia
{
    /// <summary>
    /// Provides a console-based user interface for interacting with the game.
    /// </summary>
    /// <remarks>This class implements the <see cref="IGameUI"/> interface and provides methods for displaying
    /// game-related information and prompts to the console. It includes functionality for presenting turn options,
    /// question categories, scores, and other game-related messages. The class is designed to handle user input and
    /// output in a text-based console environment.</remarks>
    internal class ConsoleGameUI : IGameUI
    {

        private readonly Table _gameTable;
        private readonly IInputHandler _inputHandler;

        public ConsoleGameUI(Table gameTable, IInputHandler inputHandler)
        {
            _gameTable = gameTable ?? throw new ArgumentNullException(nameof(gameTable));
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        }

        /// <summary>
        /// Displays the available options for the player's turn in the game.
        /// </summary>
        /// <remarks>The options presented include drawing a question card, showing scores, stealing an
        /// opponent's card, or forfeiting the game.</remarks>
        public void DisplayTurnOptions()
        {
            DisplayMessage($"What would you like to do?");
            DisplayMessage("1. Draw a Question Card");
            DisplayMessage("2. Show Scores");
            DisplayMessage("3. Steal an Opponent's Card");
            DisplayMessage("0. Forfeit/Quit");
        }

        /// <summary>
        /// Displays the available question categories and their remaining card counts to the console.
        /// </summary>
        /// <remarks>This method lists all categories defined in the <see cref="Category"/> enumeration,
        /// along with the number of  remaining questions for each category in the master question deck. It also
        /// provides an option to return or go back.</remarks>
        public void DisplayCategoryOptions()
        {
            DisplayMessage("What question category would you like to draw?");
            foreach (Category category in Enum.GetValues(typeof(Category)))
            {
                int remainingCards = _gameTable.MasterQuestionDeck.GetRemainingCardCount(category);
                DisplayMessage($"{(int)category}. {category.GetName()} ({remainingCards} questions in deck)");
            }
            DisplayMessage("0. Return/Back");
        }

        /// <summary>
        /// Presents the question to the player and the potential choices.
        /// </summary>
        /// <param name="card"> The card that contains the question to be displayed. </param>
        public void DisplayQuestionAndChoices(Card card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            Clear();
            DisplayMessage($"Category: {card.QuestionCategory.GetName()}");
            DisplayMessage($"Question: {card.QuestionText}\n");

            for (int i = 0; i < card.Choices.Count; i++)
            {
                DisplayMessage($"{(char)('A' + i)}) {card.Choices[i]}"); // using ASCII character arithmetic, iterate through each choice
            }
        }

        /// <summary>
        /// Displays the scores of all players in the game, categorized by scoring categories.
        /// </summary>
        /// <remarks>This method iterates through all players in the game and outputs their scores for
        /// each category. Scores are displayed in a formatted manner, with progress highlighted in different colors
        /// based on the player's progress toward the required score for each category.</remarks>
        public void DisplayScores()
        {
            if (_gameTable.Players == null || !_gameTable.Players.Any())
                throw new InvalidOperationException("No players available to display scores.");

            int reqScorePerCategoryToWin = GameSettings.Scoring.REQ_SCORE_PER_CATEGORY_TO_WIN;

            Clear();
            // Iterate through players
            foreach (Player player in _gameTable.Players)
            {
                DisplayMessage($"Player: {player.Name}");
                DisplayMessage("Scores per category: ");
                foreach (Category category in Enum.GetValues(typeof(Category)))
                {
                    int currentCategoryScore = player.GetCategoryScore(category);

                    string progressText = $"{currentCategoryScore} / {reqScorePerCategoryToWin}";
                    Write($"{category.GetName()}: ");

                    ConsoleColor progressColor;
                    // Colour based on progress
                    if (currentCategoryScore >= reqScorePerCategoryToWin)
                        progressColor = ConsoleColor.Green;      // Completed
                    else if (currentCategoryScore == reqScorePerCategoryToWin - 1)
                        progressColor = ConsoleColor.DarkYellow; // One step away
                    else
                        progressColor = ConsoleColor.Gray;

                    WriteTextWithColour($"{currentCategoryScore} / {reqScorePerCategoryToWin}\n", progressColor); // colour the progress

                }
                DisplayMessage(); // Add a line of space between plyers
            }

            DisplayMessage("Press any key to return to turn options...");
            Console.ReadKey(true);
            Clear();
        }

        public void DisplayStealingOptions(IEnumerable<Player> opponents, Category selectedCategory)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Displays a congratulatory message for the winning player.
        /// </summary>
        /// <remarks>This method clears the console and outputs a message indicating the winner of the
        /// game. It is intended to be called when the game concludes with a winner.</remarks>
        public void DisplayWinMessage(Player winner)
        {
            Clear();
            
            DisplayMessage($"{winner.Name} has won! Congrats!");
            DisplayMessage("You are officially a Star Wars Nerd!");
            DisplayMessage("Go boast to all your friends.");
        }

        /// <summary>
        /// Displays a farewell message to the user and prompts them to press any key to exit.
        /// </summary>
        /// <remarks>Clears the console before displaying the message. The method waits for the user to
        /// press a key  before exiting, ensuring the message remains visible until acknowledged.</remarks>
        public void DisplayExitMessage()
        {
            Clear();
            DisplayMessage("Thanks for playing Pursuit of Trivia!");
            DisplayMessage("May the Force be with you!");
            DisplayMessage("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Prompts the user to decide whether to play another round.
        /// </summary>
        /// <remarks>Displays a message asking the user to enter 'Y' for yes or 'N' for no.  The method
        /// continues prompting until a valid input is provided.</remarks>
        /// <returns><see langword="true"/> if the user chooses to play another round; otherwise, <see langword="false"/>.</returns>
        public bool PromptPlayAgain()
        {
            do
            {
                Clear();
                DisplayMessage("Would you like to play another round? (Y/N)");

                if (_inputHandler.TryGetYesNoChoice(out char selection))
                {
                    return selection == 'Y';
                }

                DisplayMessage("Please enter a valid choice (Y/N).");

                Console.ReadKey();

            } while (true);

        }


        public void Clear() => Console.Clear();

        public void Write(string text) => Console.Write(text);

        public void WriteEmptyLine() => Console.WriteLine();

        public void DisplayMessage(string text = "") => Console.WriteLine(text);
        /// <summary>
        /// Writes the specified text to the console in the specified color.
        /// </summary>
        /// <remarks>After the text is written, the console's foreground color is restored to its original
        /// value.</remarks>
        /// <param name="text">The text to write to the console. If null, no text is written.</param>
        /// <param name="colour">The <see cref="ConsoleColor"/> to use for the text.</param>
        public void WriteTextWithColour(string text, ConsoleColor colour)
        {
            ConsoleColor originalColour = Console.ForegroundColor; // save the current colour
            Console.ForegroundColor = colour; // change colour
            Write(text);
            Console.ForegroundColor = originalColour;
        }
    }
}
