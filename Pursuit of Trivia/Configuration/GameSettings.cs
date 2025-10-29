using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.Configuration
{
    /// <summary>
    /// Provides configuration settings for the game, including scoring rules and player constraints.
    /// </summary>
    /// <remarks>This class contains nested static classes that group related settings: <list type="bullet">
    /// <item> <term><see cref="Scoring"/></term> <description>Defines constants related to scoring rules, such as the
    /// number of cards required to win or steal.</description> </item> <item> <term><see cref="Players"/></term>
    /// <description>Defines constants related to player constraints, such as the minimum and maximum number of players
    /// and username length requirements.</description> </item> </list></remarks>
    internal static class GameSettings
    {
        /// <summary>
        /// Provides constants related to scoring rules in the game.
        /// </summary>
        public static class Scoring
        {
            /// <summary>
            /// The number of cards required in a category to win the game
            /// </summary>
            public const int REQ_SCORE_PER_CATEGORY_TO_WIN = 3;

            /// <summary>
            /// The number of cards required in a category to be eligible for stealing
            /// </summary>
            public const int REQ_SCORE_PER_CATEGORY_TO_STEAL = REQ_SCORE_PER_CATEGORY_TO_WIN + 2;
        }

        /// <summary>
        /// Provides constants related to player configuration, such as the minimum and maximum number of players and
        /// constraints for player usernames.
        /// </summary>
        public static class Players
        {
            /// <summary>
            /// Minimum required players for a game
            /// </summary>
            public const int MIN_PLAYERS = 2; // @TODO TOUSE

            /// <summary>
            /// Maximum allowed players in a game
            /// </summary>
            public const int MAX_PLAYERS = 5; // @TODO TOUSE

            /// <summary>
            /// Minimum length for player usernames
            /// </summary>
            public const int MIN_USERNAME_LENGTH = 1; // TODO @TOUSE
        }
    }
}
