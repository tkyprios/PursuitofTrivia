using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.GameEvents
{
    /// <summary>
    /// Serves as the base class for event argument types used in game-related events.
    /// </summary>
    /// <remarks>This class is intended to be inherited by specific event argument types that provide
    /// additional data for game events. Note that an abstract class was favoured over an interface here,
    /// as all classes that will inherit from this would serve similar functions. It inherits from EventArgs to represent
    /// an event in the program.</remarks>
    internal abstract class GameEventsArgs : EventArgs
    {
        public Player CurrentPlayer { get; }

        protected GameEventsArgs (Player player)
        {
            CurrentPlayer = player ?? throw new ArgumentNullException(nameof(player)); // make sure the player isn't null
        }

    }
}
