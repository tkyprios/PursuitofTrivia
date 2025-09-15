using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Pursuit_of_Trivia.Extensions;

namespace Pursuit_of_Trivia

///<summary>
/// Deck class represents a collection of trivia question cards organized by their categories.
///</summary>
/// <remarks> Think of this like a massive stack of question cards, each sorted into their respective categories (decks)</remarks>

{
    internal class Deck
    {
        private Dictionary<Category, List<Card>> QuestionDecks;

        private Dictionary<Category, List<string>> FallbackChoices;


        /// <summary>
        /// Constructor for Deck class that initializes a master dictionary to hold all questions categorized by their respective categories.
        /// </summary>
        /// <param name="questionCategories"> Uses IEnumerable to allow iteration through a list of questionCategories (declared in the enum class)</param>
        public Deck(IEnumerable<Category> questionCategories) 
        {
            QuestionDecks = new Dictionary<Category, List<Card>>();

            // Creates an empty list of cards for each category, initializing the dictionary
            foreach (var category in questionCategories)
            {
                QuestionDecks[category] = new List<Card>();
            }

            SeedInitialQuestions();

            InitializeFallbackChoices();
        }


        private Card CreateCard(Category questionCategory, string questionText, string correctAnswer)
        {

            // Logic for finding all answers from same category
            IEnumerable<Card> existingCategoryCards = QuestionDecks[questionCategory];
            List<string> newCardChoices = new List<string>();

            newCardChoices.Add(correctAnswer); // first choice to be added

            foreach (var selectedCard in existingCategoryCards)
            {
                if (newCardChoices.Count == 4) break;
                foreach (string choice in selectedCard.Choices)
                {
                    if (newCardChoices.Contains(choice)) continue;
                    newCardChoices.Add(choice);

                }
            }

            // Use Fallback answers if not enough
            if (newCardChoices.Count < 4)
            {
                IEnumerable<String> categoryFallbackChoices = FallbackChoices[questionCategory].Shuffle();

                foreach (string choice in categoryFallbackChoices)
                {
                    if (newCardChoices.Count == 4) break;
                    if (!newCardChoices.Contains(choice))
                        newCardChoices.Add(choice);
                    }
                    
             }

            // Shuffle the final choices so the correct answer isn't always first. Use 'Shuffle' method created in PursuitOfTrivia.Extensions.
            newCardChoices = newCardChoices.Shuffle();

            //@TODO see what AI thinks of the current setup. Is there a better way to structure?

            // Logic for getting the question and correct answer
            return new Card(questionCategory, questionText, newCardChoices, correctAnswer);
        }

        /// <summary>
        /// A void method to provide a dictionary of choices/answers when needed.
        /// </summary>
        /// <remarks>
        ///           Consider the scenario: Given the Deck class uses answers from other questions of the same category 
        ///           to fill the 'choices' on a card, the first few cards will not have enough other cards to source choices from.
        ///           To fix this, we provide generic answers relevant to each category that can be used as 'fallback'.
        /// </remarks>
        private void InitializeFallbackChoices()
        {
            FallbackChoices = new Dictionary<Category, List<string>>
            {
                [Category.Characters] = new List<string>
                {
                    "Luke Skywalker", "Darth Vader", "Han Solo", "Princess Leia",
                    "Obi-Wan Kenobi", "Yoda", "Chewbacca", "R2-D2", "C-3PO", "Anakin Skywalker",
                    "Mace Windu", "Padme Amidala", "Jango Fett", "Kylo Ren", "Rey", "Finn"

                },

                [Category.PlanetsAndLocations] = new List<string>
                {
                    "Tatooine", "Hoth", "Endor", "Naboo", "Coruscant",
                    "Exegol", "Yavin 4", "Dagobah", "Bespin", "Kamino",
                    "Felucia", "Takodana", "Mustafar", "Kashyyyk", "Alderaan",
                    "Geonosis"
                },

                [Category.ShipsVehiclesAndTech] = new List<string>
                {
                    "TIE Advanced x1", "Slave I", "X-Wing", "Y-Wing",
                    "AT-AT", "AT-ST", "Anakin's Podracer", "The Supremacy",
                    "ARC-170 Starfighter", "Tantive IV", "Protocol Droid", "Astromech Droid",
                    "Millennium Falcon", "Death Star", "TIE Interceptor", "Starkiller Base"
                },

                [Category.EventsAndBattles] = new List<string>
                {
                    "Battle of Geonosis", "Battle of Endor", "Battle of Jakku", "Duel on Mustafar",
                    "Duel on Naboo", "Revenge of the Sith", "Attack of the Clones", "Siege of Mandalore",
                    "Purge of Mandalore", "Battle of Scarif", "Battle of Ryloth", "The Phantom Menace",
                    "Ahsoka Tano vs. Darth Maul", "Yoda vs. Sidius", "Obi-Wan vs. Grevious", "Rey vs. Kylo Ren"
                },

                [Category.QuotesAndLore] = new List<string>
                {
                    "\"There is no emotion, there is peace.\"", "\"Do. Or do not. There is no try.\"", "\"Aren't you a little short for a stormtrooper?\"", "\"I know.\"",
                    "Obi-Wan Kenobi", "Yoda", "Darth Vader", "Darth Sidius",
                    "Lightsaber", "Darth Bane", "Admiral Ackbar", "One master and one apprentice",
                }
            };

        }


        /// <summary>
        /// Populates the question decks with an initial set of trivia questions categorized by topic. Hard coded in this version of the game.
        /// </summary>
        /// <remarks>This method seeds the <see cref="QuestionDecks"/> collection with predefined trivia
        /// questions grouped into their respective categories. This ensures there are some questions, should the dynamic 
        /// system fail. </remarks>
        private void SeedInitialQuestions()
        {
            QuestionDecks[Category.Characters].Add(
                new Card(
                    Category.Characters,
                    "Who trained Count Dooku before he became a Sith Lord?",
                    new List<string> { "Mace Windu", "Qui-Gon Jinn", "Yoda", "Sifo-Dyas" },
                    "Yoda"
                ));
            QuestionDecks[Category.Characters].Add(
                new Card(
                    Category.Characters,
                    "What is Admiral Ackbar's species?",
                    new List<string> { "Quarren", "Mon Calamari", "Ithorian", "Rodian" },
                    "Mon Calamari"
                ));
            QuestionDecks[Category.Characters].Add(
                new Card(
                    Category.Characters,
                    "Which Jedi discovered Ahsoka Tano as a Force-sensitive child?",
                    new List<string> { "Plo Koon", "Shaak Ti", "Yoda", "Anakin Skywalker" },
                    "Plo Koon"
                ));


            QuestionDecks[Category.PlanetsAndLocations].Add(
                new Card(
                    Category.PlanetsAndLocations,
                    "On what planet did Anakin and Obi-Wan duel at the end of Revenge of the Sith?",
                    new List<string> { "Mustafar", "Naboo", "Utapau", "Geonosis" },
                    "Mustafar"
                ));
            QuestionDecks[Category.PlanetsAndLocations].Add(
                new Card(
                    Category.PlanetsAndLocations,
                    "What planet is the homeworld of the Wookies?",
                    new List<string> { "Endor", "Felucia", "Kashyyyk", "Dantooine" },
                    "Kashyyyk"
                ));
            QuestionDecks[Category.PlanetsAndLocations].Add(
                new Card(
                    Category.PlanetsAndLocations,
                    "Where is the Jedi Temple located in the prequel trilogy?",
                    new List<string> { "Ord Mantell", "Jakku", "Malastare", "Coruscant" },
                    "Coruscant"
                ));


            QuestionDecks[Category.ShipsVehiclesAndTech].Add(
                new Card(
                    Category.ShipsVehiclesAndTech,
                    "What class of ship is the Millennium Falcon?",
                    new List<string> { "YT-1300 light freighter", "Firespray-31", "Corellian Corvette", "CR90 transport" },
                    "YT-1300 light freighter"
                ));
            QuestionDecks[Category.ShipsVehiclesAndTech].Add(
                new Card(
                    Category.ShipsVehiclesAndTech,
                    "What is the name of Darth Vader's personal TIE fighter variant?",
                    new List<string> { "TIE Bomber", "TIE Advanced x1", "TIE Defender", "TIE Interceptor" },
                    "TIE Advanced x1"
                ));
            QuestionDecks[Category.ShipsVehiclesAndTech].Add(
                new Card(
                    Category.ShipsVehiclesAndTech,
                    "Which Rebel starfighter is known for its split S-foils?",
                    new List<string> { "A-Wing", "B-Wing", "X-Wing", "Y-Wing" },
                    "X-Wing"
                ));


            QuestionDecks[Category.EventsAndBattles].Add(
                new Card(
                    Category.EventsAndBattles,
                    "What event sparked the beginning of the Clone Wars?",
                    new List<string> { "Siege of Mandalore", "Battle of Geonosis", "Battle of Naboo", "Attack on Kamino" },
                    "Battle of Geonosis"
                ));
            QuestionDecks[Category.EventsAndBattles].Add(
                new Card(
                    Category.EventsAndBattles,
                    "Who killed Darth Maul in their final duel?",
                    new List<string> { "Qui-Gon Jinn", "Yoda", "Anakin Skywalker", "Obi-Wan Kenobi" },
                    "Obi-Wan Kenobi"
                ));
            QuestionDecks[Category.EventsAndBattles].Add(
                new Card(
                    Category.EventsAndBattles,
                    "Which battle led to the destruction of the second Death Star?",
                    new List<string> { "Battle of Yavin", "Battle of Endor", "Battle of Scarif", "Battle of Jakku" },
                    "Battle of Endor"
                ));



            QuestionDecks[Category.QuotesAndLore].Add(
                new Card(
                    Category.QuotesAndLore,
                    "Who said I find your lack of faith disturbing?",
                    new List<string> { "Emperor Palpatine", "Grand Moff Tarkin", "Darth Vader", "Count Dooku" },
                    "Darth Vader"
                ));
            QuestionDecks[Category.QuotesAndLore].Add(
                new Card(
                    Category.QuotesAndLore,
                    "According to Yoda, what does fear lead to?",
                    new List<string> { "Suffering", "Hate", "Anger", "Darkness" },
                    "Anger"
                ));
            QuestionDecks[Category.QuotesAndLore].Add(
                new Card(
                    Category.QuotesAndLore,
                    "What were Obi-Wan Kenobi's final words to Darth Vader before his death?",
                    new List<string> { "If you strike me down, I shall become more powerful than you can possibly imagine.", "You can't win, Darth.", "You'll never defeat me.", "The Force will guide me." },
                    "If you strike me down, I shall become more powerful than you can possibly imagine."
                ));
            
        }

    }
}
