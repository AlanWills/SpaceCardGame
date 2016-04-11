using _2DEngine;
using CardGameEngine;
using System;
using System.Diagnostics;

namespace SpaceCardGame
{
    /// <summary>
    /// A script which runs decision algorithms for an AI player
    /// </summary>
    public class AITurnScript : Script
    {
        #region Properties and Fields

        /// <summary>
        /// A reference to the AI player we will be controlling in this script
        /// </summary>
        private GamePlayer AIPlayer { get; set; }
        
        /// <summary>
        /// A reference to the board section we will be controlling in this script
        /// </summary>
        private PlayerBoardSection BoardSection { get; set; }

        /// <summary>
        /// A reference to our battle screen
        /// </summary>
        private BattleScreen BattleScreen { get; set; }

        private float timeBetweenCardLays = 1;
        private float timeBetweenAttacks = 1;
        private float currentTimeBetweenCardLays = 0;
        private float currentTimeBetweenAttacks = 0;

        // Used in debug only
        private static bool aiLayCards = true;
        private static bool aiFight = true;

        #endregion

        public AITurnScript(GamePlayer player, PlayerBoardSection playerBoardSection) :
            base()
        {
            AIPlayer = player;
            BoardSection = playerBoardSection;
        }

        #region Virtual Functions

        /// <summary>
        /// Set up the reference to our battle screen and hides the Progress button 
        /// </summary>
        public override void Begin()
        {
            base.Begin();

            Debug.Assert(ParentScreen is BattleScreen);
            BattleScreen = ParentScreen as BattleScreen;

            BattleScreen.ProgressTurnButton.Hide();
        }

        /// <summary>
        /// Updates our player's state based on the turn state and various other factors
        /// </summary>
        /// <param name="elapsedGameTime"></param>
        public override void Update(float elapsedGameTime)
        {
            base.Update(elapsedGameTime);

            if (BattleScreen.TurnState == TurnState.kPlaceCards)
            {
                // Updates our AI player when in the card placement state
                OnPlaceCardsState(elapsedGameTime);
            }
            else
            {
                OnBattleState(elapsedGameTime);
            }
        }

        #endregion

        #region Turn State Functions

        /// <summary>
        /// Lay as many resource cards as possible and add appropriate ships/weapons etc.
        /// </summary>
        private void OnPlaceCardsState(float elapsedGameTime)
        {
            currentTimeBetweenCardLays += elapsedGameTime;

            if (aiLayCards)
            {
                // Check to see if we have laid the resource cards we can this turn and we have resources in our hand we can lay
                if (AIPlayer.CurrentHand.Exists(GetCardLayPredicate<ResourceCardData>()))
                {
                    // Lay a resource card
                    CardData resourceCardData = AIPlayer.CurrentHand.Find(GetCardLayPredicate<ResourceCardData>());

                    if (currentTimeBetweenCardLays >= timeBetweenCardLays)
                    {
                        // TODO Can improve this by analysing the resource costs of the other cards and working out what cards would be best to lay
                        LayCard(resourceCardData);
                    }
                }
                // Check to see if we have laid the ships we can and we have ships in our hand we can lay
                else if (AIPlayer.CurrentShipsPlaced < GamePlayer.MaxShipNumber && AIPlayer.CurrentHand.Exists(GetCardLayPredicate<ShipCardData>()))
                {
                    if (currentTimeBetweenCardLays >= timeBetweenCardLays)
                    {
                        // Lay a ship card
                        CardData shipCardData = AIPlayer.CurrentHand.Find(GetCardLayPredicate<ShipCardData>());

                        LayCard(shipCardData);
                    }
                }
                else
                {
                    currentTimeBetweenCardLays = 0;
                    BattleScreen.ProgressTurnButton.ForceClick();
                }
            }
            else
            {
                currentTimeBetweenCardLays = 0;
                BattleScreen.ProgressTurnButton.ForceClick();
            }
        }

        /// <summary>
        /// Attack the opponent ships
        /// </summary>
        private void OnBattleState(float elapsedGameTime)
        {
            currentTimeBetweenAttacks += elapsedGameTime;

            if (aiFight)
            {
                if (ContinueBattlePhase())
                {
                    if (currentTimeBetweenAttacks > timeBetweenAttacks)
                    {
                        foreach (CardShipPair pair in BoardSection.PlayerGameBoardSection.PlayerShipCardControl)
                        {
                            // TODO Can improve this by analysing the best opponent ship to attack
                            AttackShip(pair.Ship);
                        }
                    }
                }
                else
                {
                    currentTimeBetweenAttacks = 0;

                    BattleScreen.ProgressTurnButton.ForceClick();
                    Die();
                }
            }
            else
            {
                currentTimeBetweenAttacks = 0;

                BattleScreen.ProgressTurnButton.ForceClick();
                Die();
            }
        }

        /// <summary>
        /// Shows the ProgressTurnButton again
        /// </summary>
        public override void Die()
        {
            base.Die();

            BattleScreen.ProgressTurnButton.Show();
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Creates and adds a card using the inputted card data and resets the timer so we have spacing between adding cards.
        /// </summary>
        /// <param name="cardData"></param>
        private void LayCard(CardData cardData)
        {
            Debug.Assert(currentTimeBetweenCardLays >= timeBetweenCardLays);

            BaseUICard cardThumbnail = BoardSection.PlayerUIBoardSection.PlayerHandUI.FindCardThumbnail(cardData);
            cardThumbnail.Die();

            // Set the position of the card so that when we add it to the game board section it will be added to a slot
            BoardSection.PlayerGameBoardSection.AddCard(cardData, cardThumbnail.Size);

            currentTimeBetweenCardLays = 0;
        }

        /// <summary>
        /// A function which analyses lots of small sub conditions for whether the AI can still make a move.
        /// These include:
        /// It has ships
        /// The opponent has ships
        /// It's ships are ready
        /// It's ships have shots left
        /// </summary>
        /// <returns></returns>
        private bool ContinueBattlePhase()
        {
            // The AI has no ships, or the player has no ships, so return false
            if (AIPlayer.CurrentShipsPlaced == 0 || BattleScreen.Player.CurrentShipsPlaced == 0)
            {
                return false;
            }
            
            // If we cannot find a ship which is ready, then return false
            if (BoardSection.PlayerGameBoardSection.PlayerShipCardControl.Exists(GetReadyShipPredicate()))
            {
                return false;
            }

            // If we cannot find a ship which has a turret with shots left, then return false
            if (BoardSection.PlayerGameBoardSection.PlayerShipCardControl.Exists(GetReadyShipAndTurretWithShotsPredicate()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Searches for a ship to attack, attacks it and resets our timer for between attacks
        /// </summary>
        /// <param name="attackingShip"></param>
        private void AttackShip(Ship attackingShip)
        {
            foreach (CardShipPair pair in BattleScreen.Board.NonActivePlayerBoardSection.PlayerGameBoardSection.PlayerShipCardControl)
            {
                if ((pair.CardObject as IDamageable).Health > 0)
                {
                    float targetAngle = MathUtils.AngleBetweenPoints(attackingShip.WorldPosition, pair.CardObject.WorldPosition);
                    attackingShip.Turret.LocalRotation = targetAngle - attackingShip.WorldRotation;

                    // Attack the selected ship
                    attackingShip.Turret.Attack(pair.Ship);
                    break;
                }
            }

            currentTimeBetweenAttacks = 0;
        }

        #endregion

        #region Logical Predicates

        /// <summary>
        /// Returns a predicate for finding all the cards of the inputted type that can also be laid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Predicate<CardData> GetCardLayPredicate<T>() where T : CardData
        {
            string error = "";
            return new Predicate<CardData>(x => (x is T) && (x as T).CanLay(AIPlayer, ref error));
        }

        /// <summary>
        /// A predicate to find a ship which is ready
        /// </summary>
        /// <returns></returns>
        private Predicate<BaseObject> GetReadyShipPredicate()
        {
            return new Predicate<BaseObject>(x => (x is CardShipPair) && (x as CardShipPair).IsReady);
        }

        /// <summary>
        /// A predicate to find a ship which is ready and has turrets with shots left
        /// </summary>
        /// <returns></returns>
        private Predicate<BaseObject> GetReadyShipAndTurretWithShotsPredicate()
        {
            return new Predicate<BaseObject>(x => (x is CardShipPair) && (x as CardShipPair).IsReady && (x as CardShipPair).Ship.Turret.ShotsLeft > 0);
        }

        #endregion
    }
}
