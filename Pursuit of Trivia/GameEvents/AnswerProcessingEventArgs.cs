using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.GameEvents
{
    /// <summary>
    /// Provides data for the event that occurs when an answer is processed in the game.
    /// </summary>
    /// <remarks>This event argument includes information about the correctness of the answer and the
    /// associated card.</remarks>
    internal class AnswerProcessingEventArgs : GameEventsArgs
    {
        public bool IsCorrect { get; }
        public Card Card { get;  }

        /// <summary>
        /// Provides data for the event that occurs when a player's answer is processed.
        /// </summary>
        /// <param name="player">The player whose answer is being processed.</param>
        /// <param name="isCorrect">A value indicating whether the player's answer is correct.  <see langword="true"/> if the answer is correct;
        /// otherwise, <see langword="false"/>.</param>
        /// <param name="card">The card associated with the player's answer.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="card"/> is <see langword="null"/>.</exception>
        public AnswerProcessingEventArgs(Player player, bool isCorrect, Card card) : base(player)
        {
            IsCorrect = isCorrect;
            Card = card ?? throw new ArgumentNullException(nameof(card));
        }
    }
}
