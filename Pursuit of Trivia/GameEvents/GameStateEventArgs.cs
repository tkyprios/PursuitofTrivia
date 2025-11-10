using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.GameEvents
{
    /// <summary>
    /// Provides data for game state events, including whether the game is over and if the current player is the winner.
    /// </summary>
    /// <remarks>This class is used to convey the state of the game during game-related events. It includes
    /// information about  whether the game has ended and whether the current player is the winner.</remarks>
    internal class GameStateEventArgs : GameEventsArgs
    {
        public bool IsOver { get;  }
        public bool IsWinner { get;  }

        /// <summary>
        /// Provides data for the game state event, including the current player,  whether the game is over, and whether
        /// the current player is the winner.
        /// </summary>
        /// <param name="currentPlayer">The player whose turn it is or who is associated with the event.</param>
        /// <param name="isOver">A value indicating whether the game has ended. <see langword="true"/> if the game is over; otherwise, <see
        /// langword="false"/>.</param>
        /// <param name="isWinner">A value indicating whether the current player is the winner. <see langword="true"/> if the current player is
        /// the winner; otherwise, <see langword="false"/>. Defaults to <see langword="false"/>.</param>
        public GameStateEventArgs(Player currentPlayer, bool isOver, bool isWinner = false) : base(currentPlayer)
        {
            IsOver = isOver;
            IsWinner = isWinner;
        }
    }
}
