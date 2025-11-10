using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.GameEvents
{
    /// <summary>
    /// Provides data for the event that occurs when a player attempts to steal from another player.
    /// </summary>
    /// <remarks>This event argument contains information about the outcome of the stealing attempt,  the
    /// player who lost the card, and the category of the stolen card.</remarks>
    internal class StealingEventArgs : GameEventsArgs
    {
        public bool IsSuccessful { get;  }
        public Player LosingPlayer { get;  }
        public Category Category { get; }

        /// <summary>
        /// Provides data for the event that occurs when a player attempts to steal from another player.
        /// </summary>
        /// <param name="receivingPlayer">The player who is attempting to steal.</param>
        /// <param name="isSuccessful">A value indicating whether the stealing attempt was successful.  <see langword="true"/> if the attempt
        /// succeeded; otherwise, <see langword="false"/>.</param>
        /// <param name="losingPlayer">The player from whom the card is being stolen.  Cannot be <see langword="null"/>.</param>
        /// <param name="category">The category of the card being stolen.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="losingPlayer"/> is <see langword="null"/>.</exception>
        public StealingEventArgs(Player receivingPlayer, bool isSuccessful, Player losingPlayer, Category category) : base(receivingPlayer)
        {
            IsSuccessful = isSuccessful;
            LosingPlayer = losingPlayer ?? throw new ArgumentNullException(nameof(losingPlayer)); // check if the target player is null
            Category = category;
        }
    }
}
