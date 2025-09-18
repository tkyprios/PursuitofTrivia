using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia
{
    internal class Player
    {
        public string Name { get; private set; }

        // Note: Creates new ReadOnlyCollection wrapper on each access. 
        // Acceptable for this use case given infrequent access and small collection size.
        public IReadOnlyList<Card> EarnedCards => _earnedCards.AsReadOnly(); // encapsulation to avoid accidental edits

        private Dictionary<Category, int> _categoryScores; // use of '_' convention for private field
        private List<Card> _earnedCards;
        private const int REQ_CARD_SCORE = 3;

        public Player(string name)
        {
            Name = name;
            _earnedCards = new List<Card>();
            _categoryScores = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .ToDictionary(
                    keySelector: category => category, // map each Enum value (category) to a key with the same name
                    elementSelector: _ => 0 // we don't care about the value name, we just initialize to 0
                );
        }

        /// <summary>
        /// Retrieves the score associated with the specified category.
        /// </summary>
        /// <param name="category">The category for which to retrieve the score.</param>
        public int GetCategoryScore(Category category) => _categoryScores[category];

        /// <summary>
        /// Adds a card to the collection of earned cards and updates the score for the associated category.
        /// </summary>
        /// <param name="card">The card to add. The card must not be <see langword="null"/> and must have a valid question category.</param>
        public void AddCard(Card card)
        {
            if (card == null)
                throw new ArgumentException($"{nameof(card)} cannot be null.");

            _earnedCards.Add(card);
            _categoryScores[card.QuestionCategory]++;
        }

        /// <summary>
        /// Removes the specified card from the collection of earned cards.
        /// </summary>
        /// <param name="card">The card to remove. The card must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the card was successfully removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="card"/> is <see langword="null"/>.</exception>
        public bool RemoveCard(Card card)
        {
            if (card == null)
                throw new ArgumentException($"{nameof(card)} cannot be null.");

            if (_earnedCards.Remove(card))
            {
                _categoryScores[card.QuestionCategory]--;
                return true;
            }

            return false;

        }

        /// <summary>
        /// Determines whether the player has met the required score for all categories.
        /// </summary>
        /// <remarks>A category is considered to meet the requirement if its score is greater than or
        /// equal to the predefined threshold value. This method evaluates all categories to determine the
        /// result.</remarks>
        /// <returns><see langword="true"/> if the player has achieved the required score in all categories; otherwise, <see
        /// langword="false"/>.</returns>
        public bool HasWon()
        {
            return _categoryScores.All(keyValuePair => {
                Category category = keyValuePair.Key;     
                int scoreValue = keyValuePair.Value;      
                return scoreValue >= REQ_CARD_SCORE;
            });
        }

        private bool CategoryScoreAchieved(Category category) => _categoryScores[category] >= REQ_CARD_SCORE;

    }
}
