using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pursuit_of_Trivia.Configuration;
using Pursuit_of_Trivia.Interfaces;
using Pursuit_of_Trivia.Extensions;

namespace Pursuit_of_Trivia
{
    internal class TurnManager
    {
        private readonly Table _gameTable;
        private readonly IGameUI _gameUI;
        private readonly IInputHandler _inputHandler;

        public enum TurnResult
        {
            Continue,
            Exit,
            GameWon
        }
       

        public TurnManager(Table gameTable, IGameUI gameUI, IInputHandler inputHandler)
        {
            // Assign gameTable to _gameTable if not null. Throw ArgumentNullException if gameTable is null
            // Table is a critical dependency for TurnManager since it manaes game state including players and question deck
            _gameTable = gameTable ?? throw new ArgumentNullException(nameof(gameTable));

            // Same approach for gameUI and inputHandler
            _gameUI = gameUI ?? throw new ArgumentNullException(nameof(gameUI));

            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        }


        public TurnResult ProcessTurn()
        {
            _gameUI.Clear();
            var currentPlayer = _gameTable.GetCurrentPlayer();
            _gameUI.DisplayMessage($"It's {currentPlayer.Name}'s turn!");

            bool turnComplete = false;

            while (!turnComplete)
            {
                _gameUI.WriteEmptyLine();
                _gameUI.DisplayTurnOptions();

                if (!_inputHandler.TryGetNumberInRange(0, 3, out int selection))
                {
                    _gameUI.DisplayValidationError("Please enter a valid number.");
                    continue; // Move to next iteration of loop
                }

                switch (selection)
                {
                    case 0:
                        _gameUI.DisplayMessage($"Player {currentPlayer.Name} is considering forefeit.");
                        if (_inputHandler.ConfirmYesNoAction("Are you sure you want to forfeit? (Y/N)"))
                        {
                            return TurnResult.Exit;
                        }
                        _gameUI.Clear();
                        break;
                    case 1:
                        if (HandleTriviaQuestion()) turnComplete = true;
                        break;
                    case 2:
                        _gameUI.DisplayScores();
                        break;
                    case 3:
                        if (HandleStealingTurn(currentPlayer)) turnComplete = true;
                        break;
                    default:
                        _gameUI.DisplayValidationError("Invalid selection."); 
                        break;
                }
            }

            if (currentPlayer.HasWon(GameSettings.Scoring.REQ_SCORE_PER_CATEGORY_TO_WIN))
            {
                return TurnResult.GameWon;
            }


            _gameTable.GetNextPlayer();
            return TurnResult.Continue; // Game continues
        }



        /// <summary>
        /// Determines whether a valid question category has been selected by the user and if it has, proceeds to ask a question for that category.
        /// </summary>
        /// <returns> A boolean value on whether a value category has been selected. </returns>
        private bool HandleTriviaQuestion()
        {
            var category = SelectCategory(); // category is of type 'Category?'
            if (category.HasValue) // check if we have value
            {
                PlayTriviaQuestion(category.Value); // Must use .Value to get non-nullable value
                return true;
            }
            else
            {
                _gameUI.Clear();
                return false; // return to menu
            }

        }


        /// <summary>
        /// Prompts the user to select a category from a list of options and returns the selected category.
        /// </summary>
        /// <remarks>Displays a list of category options and repeatedly prompts the user until
        /// a valid selection is made. If the user selects 0, the method returns <see langword="null"/>. Otherwise, it
        /// returns the selected category as a <see cref="Category"/>.</remarks>
        /// <returns>The selected <see cref="Category"/>, or <see langword="null"/> if the user selects 0.</returns>
        private Category? SelectCategory()
        {
            while (true)
            {
                _gameUI.Clear();
                _gameUI.DisplayCategoryOptions();

                if (!_inputHandler.TryGetCategorySelection(out int selection))
                {
                    _gameUI.DisplayValidationError("Please enter a valid category number.");
                    continue;
                }

                if (selection == 0)
                    return null;

                return (Category)selection; // because selection is an int

            }
        }


        /// <summary>
        /// Begins presentation of question, through drawing a card and presenting to current player. Finally validates the player's answer.
        /// </summary>
        /// <param name="category"> The category to draw the question from. </param>
        /// <exception cref="InvalidOperationException"> Throws if no card could be sourced. </exception>
        private void PlayTriviaQuestion(Category category)
        {
            if (_gameTable.MasterQuestionDeck.IsDeckEmpty(category))
            {
                throw new InvalidOperationException($"Cannot draw a card: The '{category.GetName()}' deck is empty.");
            }

            var card = _gameTable.MasterQuestionDeck.DrawQuestionCard(category);
            var player = _gameTable.GetCurrentPlayer();

            if (card != null)
            {
                _gameUI.DisplayQuestionAndChoices(card);
                ProcessAnswer(card, player);
            }

            else
            {
                throw new InvalidOperationException("No card retrieved from the selected category.");
            }

        }


        /// <summary>
        /// Validates/checks the answer that the player provides. 
        /// </summary>
        /// <remarks> First attempts to read the answer the player provides and checks if it is a valid letter.
        /// Then converts the letter value to an index using ASCII arithmetic relative to the letter 'A'.
        /// Then checks the answer.
        /// </remarks>
        /// <param name="card"></param>
        /// <param name="player"></param>
        private void ProcessAnswer(Card card, Player player)
        {
            while (true)
            {
                _gameUI.Write("Answer (A-D): ");

                if (!_inputHandler.TryGetLetterChoice(out char letter))
                {
                    _gameUI.DisplayValidationError("Please enter a valid letter (A-D).");
                    continue;
                }

                _gameUI.WriteEmptyLine();

                // Convert letter to index, where A = 0, B = 1 etc. 
                // Uses 'A' as the reference, where A is the first choice
                int choiceIndex = letter - 'A'; // Using ASCII character arithemtic

                // Get the chosen answer using the index
                string selectedAnswer = card.Choices[choiceIndex];

                if (card.CheckAnswer(selectedAnswer))
                {
                    _gameUI.DisplayAnswerResult(true, player, card);
                    player.AddCard(card);
                }

                else
                {
                   _gameUI.DisplayAnswerResult(false, player, card);
                   _gameTable.MasterQuestionDeck.ReturnCardToBottom(card);

                    var receivingPlayer = _gameTable.GetRandomOtherPlayer(); // the lucky player to steal a card from the player who answered incorrect.

                    _gameUI.DisplayMessage($"The lucky player {receivingPlayer.Name} gets to steal a random card from {player.Name}'s '{card.QuestionCategory.GetName()}' collection.");

                    HandleStealCard(player, card.QuestionCategory, receivingPlayer);


                }

                _gameUI.WaitForInput("\nPress any key to end turn...");
                break;
            }
        }

        /// <summary>
        /// Handles the action of one player attempting to steal a random card from another player's collection within a
        /// specified category.
        /// </summary>
        /// <remarks>If the <paramref name="losingPlayer"/> has no cards in the specified <paramref
        /// name="questionCategory"/>,  no card is transferred, and the operation is considered unsuccessful.</remarks>
        /// <param name="losingPlayer">The player from whom the card is being stolen.</param>
        /// <param name="questionCategory">The category of the card to be stolen. The card will be randomly selected from the losing player's
        /// collection in this category.</param>
        /// <param name="receivingPlayer">The player who will receive the stolen card, if the losing player has a card in the specified category.</param>
        private void HandleStealCard(Player losingPlayer, Category questionCategory, Player receivingPlayer)
        {

            if (losingPlayer.TransferRandomCard(questionCategory, receivingPlayer))
            {
                _gameUI.DisplayStealResult(true, losingPlayer, questionCategory, receivingPlayer);
            }
            else
            {
                _gameUI.DisplayStealResult(false, losingPlayer, questionCategory, receivingPlayer);
            }
        }


        /// <summary>
        /// Handles the process of allowing the current player to attempt to steal a card from another player for their turn.
        /// </summary>
        /// <remarks>This method involves selecting a category to steal from and a target player to steal
        /// from. If either selection is invalid or not made, the method returns <see langword="false"/>. Otherwise, the
        /// steal attempt is processed.</remarks>
        /// <param name="currentPlayer">The player whose turn it is to attempt the steal.</param>
        /// <returns><see langword="true"/> if the steal attempt was initiated successfully; otherwise, <see langword="false"/>
        /// if the steal could not be attempted (e.g., no valid category or user returned to menu).</returns>
        private bool HandleStealingTurn(Player currentPlayer)
        {
            // Check if player has any categories with enough cards
            var categoryToStealFrom = SelectCategoryToStealFrom(currentPlayer);
            if (!categoryToStealFrom.HasValue)
            {
                return false; // has no eligible categories or chose to return to menu
            }

            var playerToStealFrom = SelectPlayerToStealFrom(currentPlayer, categoryToStealFrom.Value);
            if (playerToStealFrom == null)
            {
                return false; // chose to return to menu
            }

            // Attempt the steal 

            int selectedCategoryScore = playerToStealFrom.GetCategoryScore(categoryToStealFrom.Value);

            if (selectedCategoryScore <= 0)
            {
                _gameUI.DisplayMessage($"The selected player {playerToStealFrom} does not have enough cards in category '{categoryToStealFrom.GetName()}'.");
                _gameUI.WaitForInput("Press any key to return to main menu...");
                return false;
            }

            HandleStealCard(playerToStealFrom, categoryToStealFrom.Value, currentPlayer); // unwrap nullable Category type using .Value
            return true;
        }

        /// <summary>
        /// Prompts the player to select a category from which to steal cards, based on the categories they have with
        /// sufficient cards to meet the required threshold.
        /// </summary>
        /// <remarks>A category is eligible for stealing if it contains at least the required number of
        /// cards, as defined by the constant <c>REQ_SCORE_PER_CATEGORY_TO_STEAL</c>. If no eligible categories are
        /// found, the method informs the player and allows them to return to the previous menu.</remarks>
        /// <param name="currentPlayer">The player whose categories are being evaluated for stealing eligibility.</param>
        /// <returns>The selected <see cref="Category"/> to steal from, or <see langword="null"/> if the player chooses to return
        /// without making a selection or if no categories meet required score.</returns>
        private Category? SelectCategoryToStealFrom(Player currentPlayer)
        {
            if (currentPlayer == null)
                throw new ArgumentNullException(nameof(currentPlayer));

            List<Category> categoriesWithSufficientCardsToSteal = currentPlayer.GetTradeWorthyCategories(GameSettings.Scoring.REQ_SCORE_PER_CATEGORY_TO_STEAL);

            if (!categoriesWithSufficientCardsToSteal.Any())
            {
                _gameUI.DisplayMessage($"You need at least {GameSettings.Scoring.REQ_SCORE_PER_CATEGORY_TO_STEAL} cards in a category to steal from another player!");
                _gameUI.WaitForInput("Press any key to return to turn options...");
                _gameUI.Clear();
                return null;
            }

            do
            {
                _gameUI.Clear();

                _gameUI.DisplayCategoriesWithSufficientCardsToSteal(currentPlayer, categoriesWithSufficientCardsToSteal);

                // Validate input
                if (!_inputHandler.TryGetNumberInRange(0, categoriesWithSufficientCardsToSteal.Count, out int selection))
                {
                    _gameUI.DisplayValidationError("Please enter a valid number.");
                    continue;
                }


                // Check if selection is valid
                if (selection == 0)
                    return null;

                return categoriesWithSufficientCardsToSteal[selection - 1]; // the player to steal from
            } while (true);



        }

        /// <summary>
        /// Prompts the current player to select an opponent to steal a card from.
        /// </summary>
        /// <remarks>This method displays a list of opponents (excluding the current player) and allows
        /// the current player to make a selection. The method ensures that the input is validated and prompts the
        /// player to try again if an invalid selection is made.</remarks>
        /// <param name="currentPlayer">The player making the selection. This player will not appear in the list of options.</param>
        /// <returns>The selected opponent to steal a card from, or <see langword="null"/> if the player chooses to return
        /// without making a selection.</returns>
        private Player SelectPlayerToStealFrom(Player currentPlayer, Category selectedCategory)
        {
            if (currentPlayer == null)
                throw new ArgumentNullException(nameof(currentPlayer));

            var opponents = _gameTable.Players
                .Where(player => player != currentPlayer)
                .ToList();

            do
            {
                _gameUI.Clear();

                _gameUI.DisplayOpponentScoreForCategoryList(opponents, selectedCategory);

                // Validate input
                if (!_inputHandler.TryGetNumberInRange(0, opponents.Count, out int selection))
                {
                    _gameUI.DisplayValidationError("Please enter a valid number.");
                    continue;
                }

                // return to menu
                if (selection == 0)
                    return null;

                 return opponents[selection - 1]; // the player to steal from
            } while (true);


        }
    }
}
