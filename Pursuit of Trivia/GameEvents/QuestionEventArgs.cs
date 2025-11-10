using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia.GameEvents
{
    /// <summary>
    /// Provides data for events related to a question in the game, including the associated card and its category.
    /// </summary>
    /// <remarks>This event argument is used to pass information about a question, such as the card containing
    /// the question and the category of the question, to event handlers.</remarks>
    internal class QuestionEventArgs : GameEventsArgs
    {
        public Card Card { get;  }
        public Category Category { get;  }

        public QuestionEventArgs(Player player, Card card) : base(player)
        {
            Card = card ?? throw new ArgumentException(nameof(card)); // make sure card is not null
            Category = card.QuestionCategory; // get the category from the card
        }
    }
}
