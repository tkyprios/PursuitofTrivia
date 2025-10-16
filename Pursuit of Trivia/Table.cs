using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia
{
    /// <summary>
    /// Class that represents the game table, managing players and the master question deck.
    /// Implements the "what" (who is next, who is current)
    /// </summary>
    internal class Table
    {
        public List<Player> Players { get; private set; }

        public Deck MasterQuestionDeck { get; private set; }

        private int currentPlayerIndex = 0; // tracks current player, initialize to 0


        public Table(Deck masterQuestionDeck)
        {
            Players = new List<Player>();
            MasterQuestionDeck = masterQuestionDeck;
        }

        public void AddPlayerToGame(Player player)
        {
            Players.Add(player);
            Console.WriteLine($"Player {player.Name} has been added to the game!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        public void AddBotToGame()
        {
            Player bot = new Player("Bot");
            Players.Add(bot);
            Console.WriteLine("A bot has been added to the game!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        /// <summary>
        /// Retrieves the player whose turn it is in the game.
        /// </summary>
        /// <returns>The <see cref="Player"/> object representing the current player.</returns>
        public Player GetCurrentPlayer()
        {
            return Players[currentPlayerIndex];
        }

        /// <summary>
        /// Advances to the next player in the sequence and returns the current player.
        /// </summary>
        /// <remarks>This method updates the internal state to point to the next player in the list of
        /// players.  If the end of the list is reached, the sequence continues from the beginning.</remarks>
        /// <returns>The next player in the sequence. The sequence wraps around to the first player after the last player, using Modulo.
        /// Always stays within the bounds (0 to Players.Count - 1) - e.g.currentPlayerIndex = 0, (0 + 1) % 3 = 1 etc.</returns>
        public Player GetNextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Count;
            return GetCurrentPlayer();
        }
    }
}
