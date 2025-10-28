using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.Interfaces
{
    internal interface IInputHandler
    {
        /// <summary>
        /// Attempts to retrieve the currently selected category.
        /// </summary>
        /// <param name="category">When this method returns, contains the identifier of the selected category if the operation succeeds;
        /// otherwise, the value is undefined.</param>
        /// <returns><see langword="true"/> if a category is successfully retrieved; otherwise, <see langword="false"/>.</returns>
        bool TryGetCategorySelection(out int category);

        /// <summary>
        /// Attempts to retrieve a letter choice and returns a value indicating whether the operation succeeded.
        /// </summary>
        /// <param name="letter">When this method returns, contains the letter choice if the operation succeeded; otherwise, the default
        /// value for <see cref="char"/>.</param>
        /// <returns><see langword="true"/> if a letter choice was successfully retrieved; otherwise, <see langword="false"/>.</returns>
        bool TryGetLetterChoice(out char letter);

        /// <summary>
        /// Attempts to retrieve a user's choice as a 'Yes' or 'No' response.
        /// </summary>
        /// <param name="response">When this method returns, contains the character representing the user's choice:  'Y' for Yes or 'N' for No,
        /// if the operation succeeds. The value is undefined if the operation fails.</param>
        /// <returns><see langword="true"/> if a valid 'Yes' or 'No' choice was successfully retrieved; otherwise, <see
        /// langword="false"/>.</returns>
        bool TryGetYesNoChoice(out char response);

        /// <summary>
        /// Attempts to retrieve a number within the specified range.
        /// </summary>
        /// <remarks>The method ensures that the returned number, if any, is within the range defined by
        /// <paramref name="minValue"/>  and <paramref name="maxValue"/>. The range is inclusive of both
        /// bounds.</remarks>
        /// <param name="minValue">The inclusive lower bound of the valid range.</param>
        /// <param name="maxValue">The inclusive upper bound of the valid range.</param>
        /// <param name="selection">When this method returns, contains the number within the specified range if the operation succeeds; 
        /// otherwise, contains 0. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if a valid number within the range is successfully retrieved; otherwise,  <see
        /// langword="false"/>.</returns>
        bool TryGetNumberInRange(int minValue, int maxValue, out int selection);

        /// <summary>
        /// Displays a prompt to the user and waits for a "yes" or "no" confirmation.
        /// </summary>
        /// <param name="prompt">The message to display to the user, asking for confirmation.</param>
        /// <returns><see langword="true"/> if the user confirms with "yes"; otherwise, <see langword="false"/>.</returns>
        bool ConfirmYesNoAction(string prompt);
    }
}
