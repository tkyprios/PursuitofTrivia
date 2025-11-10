using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pursuit_of_Trivia;
using static Pursuit_of_Trivia.TurnManager;

namespace Pursuit_of_Trivia.GameEvents
{
    /// <summary>
    /// Provides data for events related to the state of a player's turn in the game.
    /// </summary>
    /// <remarks>This event argument contains information about the player whose turn is being processed and
    /// the result of the turn. It is typically used to convey the outcome of a turn to event handlers.</remarks>
    internal class TurnStateEventArgs : GameEventsArgs
    {
        public TurnResult Result { get; }

        /// <summary>
        /// Provides data for the event that occurs during a player's turn
        /// </summary>
        /// <param name="player">The player whose turn it is. Cannot be <see langword="null"/>.</param>
        /// <param name="result">The result of the player's turn, indicating the outcome or state of the turn.</param>
        public TurnStateEventArgs(Player player, TurnResult result) : base(player)
        {
            Result = result;
        }

    }
}
