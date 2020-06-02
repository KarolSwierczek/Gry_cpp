namespace cpp.Sen.Gameplay
{
    using System.Collections.Generic;

    public sealed class InteractionRules
    {
        #region Public Types
        public enum InteractionRuleType
        {
            Default,
            FromDiscard,
            FromDraw,
            FromDrawSpecial,
            FromPeek1,
            FromDraw2,
            FromSwap2,
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// it's the beginning of players turn
        /// </summary>
        public class DefaultRule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, true },
                    { InteractionController.CardCollectionType.Discard, true },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, false },
                    { InteractionController.CardCollectionType.NPC, false }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.FromDraw;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Draw:
                        {
                            _NextRule = InteractionRuleType.FromDraw;
                            if (card.Type != Card.CardType.Normal) { _NextRule = InteractionRuleType.FromDrawSpecial; }

                            var topCard = source.RemoveCard(default);
                            inspect.AddCard(topCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Discard:
                        {
                            _NextRule = InteractionRuleType.FromDiscard;

                            var topCard = source.RemoveCard(default);
                            inspect.AddCard(topCard);

                            break;
                        }
                    default:
                        throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }
            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// player drew card from discard pile
        /// </summary>
        public class FromDiscardRule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.NPC, false }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                if (source.Type != InteractionController.CardCollectionType.Player)
                {
                    throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }

                var selectedCard = source.RemoveCard(card);
                discard.AddCard(selectedCard);

                var inspectedCard = inspect.RemoveFirstCard();
                source.AddCard(inspectedCard);
            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// player drew a normal card from draw pile
        /// </summary>
        public class FromDrawRule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, true },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.NPC, false }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Discard:
                        {
                            var inspectedCard = inspect.RemoveFirstCard();
                            source.AddCard(inspectedCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Player:
                        {
                            var selectedCard = source.RemoveCard(card);
                            discard.AddCard(selectedCard);

                            var inspectedCard = inspect.RemoveFirstCard();
                            source.AddCard(inspectedCard);

                            break;
                        }
                    default:
                        throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }
            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// player drew a special card from draw pile
        /// </summary>
        public class FromDrawSpecialRule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, true },
                    { InteractionController.CardCollectionType.Inspect, true },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.NPC, false }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                _NextRule = InteractionRuleType.Default;

                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Discard:
                        {                         
                            var selectedCard = inspect.RemoveCard(card);
                            discard.AddCard(selectedCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Player:
                        {
                            var selectedCard = source.RemoveCard(card);
                            discard.AddCard(selectedCard);

                            var inspectedCard = inspect.RemoveFirstCard();
                            source.AddCard(inspectedCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Inspect:
                        {
                            //todo: communicate that a special card has been activated and not discarded
                            switch (card.Type)
                            {
                                case Card.CardType.Draw:
                                    {
                                        _NextRule = InteractionRuleType.FromDraw2; 
                                        
                                        inspect.AddCard(draw.RemoveCard());
                                        inspect.AddCard(draw.RemoveCard());

                                        break;
                                    }
                                case Card.CardType.Swap:
                                    {
                                        _NextRule = InteractionRuleType.FromSwap2;
                                        break;
                                    }
                                case Card.CardType.Peek:
                                    {
                                        _NextRule = InteractionRuleType.FromPeek1;
                                        break;
                                    }
                                case Card.CardType.Normal:
                                default:
                                    throw new System.Exception("Normal card type is not supported in " + ToString());
                            }

                            var selectedCard = inspect.RemoveCard(card);
                            discard.AddCard(selectedCard);

                            break;
                        }
                    default:
                        throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }
            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// player activated a "peek 1" card
        /// </summary>
        public class FromPeek1Rule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, true },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.NPC, true }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.FromPeek1;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            private ICardCollection _PeekSource;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                _NextRule = InteractionRuleType.Default;

                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Player:
                    case InteractionController.CardCollectionType.NPC:
                        {
                            if (inspect.Count > 0)
                            {
                                ReturnCard(inspect);
                            }
                            else
                            {
                                _NextRule = InteractionRuleType.FromPeek1;
                                _PeekSource = source;

                                var selectedCard = source.RemoveCard(card);
                                inspect.AddCard(selectedCard);
                                
                            }

                            break;
                        }
                    case InteractionController.CardCollectionType.Inspect:
                        {
                            if(inspect.Count > 0) { ReturnCard(inspect); }

                            break;
                        }
                    default:
                        throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }
            }

            private void ReturnCard(PlayerHand inspect)
            {
                var peekedCard = inspect.RemoveFirstCard();
                _PeekSource.AddCard(peekedCard);
                _PeekSource = null;
            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// player activated a "draw 2" card
        /// </summary>
        public class FromDraw2Rule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, true },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.NPC, false }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Inspect:
                        {
                            if(inspect.Count == 2)
                            {
                                _NextRule = InteractionRuleType.FromDraw2;

                                var selectedCard = source.RemoveCard(card);
                                discard.AddCard(selectedCard);
                            }

                            break;
                        }
                    case InteractionController.CardCollectionType.Player:
                        {
                            if (inspect.Count < 2)
                            {
                                var selectedCard = source.RemoveCard(card);
                                discard.AddCard(selectedCard);

                                var inspectedCard = inspect.RemoveFirstCard();
                                source.AddCard(inspectedCard);
                            }

                            break;
                        }
                    default:
                        throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }

            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// player activated a "swap 2" card
        /// </summary>
        public class FromSwap2Rule : IInteractionRule
        {
            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.NPC, true }
                };

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            private Card _SelectedCard;
            private ICardCollection _SelectedCollection;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source, PlayerHand inspect, CardStack draw, CardStack discard)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Player:
                    case InteractionController.CardCollectionType.NPC:
                        {
                            if (_SelectedCard == null)
                            {
                                _NextRule = InteractionRuleType.FromSwap2;

                                _SelectedCard = source.RemoveCard(card);
                                _SelectedCollection = source;
                            }
                            else
                            {
                                var selectedCard = source.RemoveCard(card);
                                source.AddCard(_SelectedCard);

                                _SelectedCollection.AddCard(selectedCard);

                                _SelectedCard = null;
                                _SelectedCollection = null;
                            }

                            break;
                        }
                    default:
                        throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }

            }
            #endregion Private Methods
        }
        #endregion Public Types
    }
}