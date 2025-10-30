using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.Interfaces
{
    internal interface IGameUI
    {
        /// <summary>
        /// Displays the available options for the current turn to the user.
        /// </summary>
        /// <remarks>This method is typically used to present a list of actions or choices  that the user
        /// can take during their turn in a game or interactive session. Ensure that the user interface is ready to
        /// handle input before calling this method.</remarks>
        void DisplayTurnOptions();

        /// <summary>
        /// Displays a list of available category options to the user.
        /// </summary>
        /// <remarks>This method is typically used to present selectable categories in a user interface. 
        /// The specific categories displayed depend on the application's current state or configuration.</remarks>
        void DisplayCategoryOptions();

        /// <summary>
        /// Displays the question and its associated choices from the specified card.
        /// </summary>
        /// <param name="card">The card containing the question and choices to display. Cannot be null.</param>
        void DisplayQuestionAndChoices(Card card);

        /// <summary>
        /// Displays the current scores to the console or user interface.
        /// </summary>
        /// <remarks>This method outputs the scores in a predefined format. It does not return any value
        /// and is intended for display purposes only. Ensure that the scores are initialized before calling this
        /// method.</remarks>
        void DisplayScores();

        /// <summary>
        /// Displays a message indicating that the user has won.
        /// </summary>
        /// <remarks>This method is typically called when a win condition is met in the application. The
        /// exact content and presentation of the message may vary depending on the implementation.</remarks>
        void DisplayWinMessage(Player winner);

        /// <summary>
        /// Displays a message to the user indicating that the application is exiting.
        /// </summary>
        /// <remarks>This method is typically used to provide feedback to the user before the application
        /// terminates. Ensure that any necessary cleanup or finalization logic is performed prior to calling this
        /// method.</remarks>
        void DisplayExitMessage();

        /// <summary>
        /// Prompts the user to decide whether to play again.
        /// </summary>
        /// <returns><see langword="true"/> if the user chooses to play again; otherwise, <see langword="false"/>.</returns>
        bool PromptPlayAgain();

        /// <summary>
        /// Displays stealing-related information and options.
        /// </summary>
        void DisplayStealingOptions(IEnumerable<Player> opponents, Category selectedCategory);

        /// <summary>
        /// Clears the display.
        /// </summary>
        void Clear();

        /// <summary>
        /// Writes an empty line to the output.
        /// </summary>
        /// <remarks>This method is typically used to insert a blank line for formatting
        /// purposes.</remarks>
        void WriteEmptyLine();

        /// <summary>
        /// Displays a message in GUI, or writes a line in Console.
        /// </summary>
        void DisplayMessage(string text);

        /// <summary>
        /// Writes text without a line break.
        /// </summary>
        void Write(string text);

        void WriteTextWithColour(string text, ConsoleColor colour);


    }
}
