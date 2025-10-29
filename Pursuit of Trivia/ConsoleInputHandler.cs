using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pursuit_of_Trivia.Interfaces;

namespace Pursuit_of_Trivia
{
    internal class ConsoleInputHandler : IInputHandler
    {

        /// <summary>
        /// Attempts to get a valid category selection from the player.
        /// </summary>
        /// <param name="category">When this method returns, contains the selected Category if input was valid, 
        /// or 0 if the player chose to return.</param>
        /// <returns>true if the input was valid; otherwise, false.</returns>
        public bool TryGetCategorySelection(out int category)
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
        /// Checks if a given input is a valid letter choice (A-D)
        /// </summary>
        /// <param name="letter"> The letter to output to. </param>
        /// <returns> True if a valid letter choice, false if not. </returns>
        public bool TryGetLetterChoice(out char letter)
        {
            // Validate user input is a valid letter choice
            letter = '\0'; // null/invalid character for default

            var input = Console.ReadLine()?.Trim().ToUpper();

            // Check if input is exactly one character
            if (string.IsNullOrEmpty(input) || input.Length != 1)
                return false;

            letter = input[0]; // then assign input to the letter.

            return letter >= 'A' && letter <= 'D'; // check if input is a valid letter.
        }

        /// <summary>
        /// Attempts to read a single character input from the console and determine if it represents a valid 'Yes' or
        /// 'No' choice.
        /// </summary>
        /// <remarks>This method reads input from the console, trims whitespace, and converts the input to
        /// uppercase. It validates that the input is a single character and checks if it matches 'Y' or 'N'.</remarks>
        /// <param name="ynLetter">When this method returns, contains the character entered by the user, converted to uppercase, if the input
        /// is valid; otherwise, contains the null character ('\0').</param>
        /// <returns><see langword="true"/> if the input is a valid 'Yes' or 'No' choice (either 'Y' or 'N'); otherwise, <see
        /// langword="false"/>.</returns>
        public bool TryGetYesNoChoice(out char ynLetter)
        {
            // Validate user input is a valid letter choice
            ynLetter = '\0'; // null/invalid character for default

            var input = Console.ReadLine()?.Trim().ToUpper();

            // Check if input is exactly one character
            if (string.IsNullOrEmpty(input) || input.Length != 1)
                return false;

            ynLetter = input[0]; // then assign input to the letter.

            return ynLetter == 'Y' || ynLetter == 'N'; // check if input is a valid letter.
        }

        /// <summary>
        /// Attempts to read and parse an integer from the console input and determines if it falls within the specified
        /// range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the valid range.</param>
        /// <param name="maxValue">The inclusive upper bound of the valid range.</param>
        /// <param name="selection">When this method returns, contains the parsed integer if the input was valid and within the specified range;
        /// otherwise, contains 0.</param>
        /// <returns><see langword="true"/> if the input was successfully parsed as an integer and falls within the specified
        /// range; otherwise, <see langword="false"/>.</returns>
        public bool TryGetNumberInRange(int minValue, int maxValue, out int selection)
        {
            selection = 0;
            var input = Console.ReadLine();

            if (!int.TryParse(input, out selection))
                return false;

            return selection >= minValue && selection <= maxValue;
        }


        /// <summary>
        /// Prompts the user to confirm an action by entering 'Y' or 'N'.
        /// </summary>
        /// <remarks>The method repeatedly prompts the user until a valid input ('Y' or 'N') is
        /// provided.</remarks>
        /// <param name="prompt">The message displayed to the user to request confirmation.</param>
        /// <returns><see langword="true"/> if the user confirms the action by entering 'Y'; otherwise, <see langword="false"/>.</returns>
        public bool ConfirmYesNoAction(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                if (TryGetYesNoChoice(out char choice))
                {
                    return choice == 'Y';
                }
                Console.WriteLine("Please enter Y or N.");
            }
        }
    }
}
