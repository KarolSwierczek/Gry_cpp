namespace cpp.Sen.Gameplay
{
    using System.Collections.Generic;

    public sealed class InteractionRules
    {
        #region Public Types
        public enum InteractionRuleType
        {
            Peek2,
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
        /// the pklayer peeks cards at the beginning of the round
        /// </summary>
        public class Peek2Rule : IInteractionRule
        {
            #region Public Methods
            public Peek2Rule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.CPU, false }
                };

            private readonly  CardCollections _CardCollections;

            private InteractionRuleType _NextRule = InteractionRuleType.Peek2;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                _NextRule = InteractionRuleType.Peek2;

                if (_CardCollections.Inspect.Count < 2)
                {
                    source.RemoveCard(card);
                    _CardCollections.Inspect.AddCard(card);
                }
                else
                {
                    source.AddCard(_CardCollections.Inspect.RemoveFirstCard());
                    source.AddCard(_CardCollections.Inspect.RemoveFirstCard());
                    _NextRule = InteractionRuleType.Default;
                }
            }
            #endregion Private Methods
        }

        /// <summary>
        /// Interaction Rule that's active when
        /// it's the beginning of players turn
        /// </summary>
        public class DefaultRule : IInteractionRule
        {
            #region Public Methods
            public DefaultRule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, true },
                    { InteractionController.CardCollectionType.Discard, true },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, false },
                    { InteractionController.CardCollectionType.CPU, false }
                };

            private readonly CardCollections _CardCollections;

            private InteractionRuleType _NextRule = InteractionRuleType.FromDraw;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Draw:
                        {
                            _NextRule = InteractionRuleType.FromDraw;
                            if (card.Type != Card.CardType.Normal) { _NextRule = InteractionRuleType.FromDrawSpecial; }

                            var topCard = source.RemoveCard(default);
                            _CardCollections.Inspect.AddCard(topCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Discard:
                        {
                            _NextRule = InteractionRuleType.FromDiscard;

                            var topCard = source.RemoveCard(default);
                            _CardCollections.Inspect.AddCard(topCard);

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
            #region Public Methods
            public FromDiscardRule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.CPU, false }
                };

            private readonly CardCollections _CardCollections;

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                if (source.Type != InteractionController.CardCollectionType.Player)
                {
                    throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type);
                }

                var selectedCard = source.RemoveCard(card);
                _CardCollections.Discard.AddCard(selectedCard);

                var inspectedCard = _CardCollections.Inspect.RemoveFirstCard();
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
            #region Public Methods
            public FromDrawRule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, true },
                    { InteractionController.CardCollectionType.Inspect, false },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.CPU, false }
                };

            private readonly CardCollections _CardCollections;

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Discard:
                        {
                            var inspectedCard = _CardCollections.Inspect.RemoveFirstCard();
                            source.AddCard(inspectedCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Player:
                        {
                            var selectedCard = source.RemoveCard(card);
                            _CardCollections.Discard.AddCard(selectedCard);

                            var inspectedCard = _CardCollections.Inspect.RemoveFirstCard();
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
            #region Public Methods
            public FromDrawSpecialRule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, true },
                    { InteractionController.CardCollectionType.Inspect, true },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.CPU, false }
                };

            private readonly CardCollections _CardCollections;

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                _NextRule = InteractionRuleType.Default;

                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Discard:
                        {                         
                            var selectedCard = _CardCollections.Inspect.RemoveCard(card);
                            _CardCollections.Discard.AddCard(selectedCard);

                            break;
                        }
                    case InteractionController.CardCollectionType.Player:
                        {
                            var selectedCard = source.RemoveCard(card);
                            _CardCollections.Discard.AddCard(selectedCard);

                            var inspectedCard = _CardCollections.Inspect.RemoveFirstCard();
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

                                        _CardCollections.Inspect.AddCard(_CardCollections.Draw.RemoveCard());
                                        _CardCollections.Inspect.AddCard(_CardCollections.Draw.RemoveCard());

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

                            var selectedCard = _CardCollections.Inspect.RemoveCard(card);
                            _CardCollections.Discard.AddCard(selectedCard);

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
            #region Public Methods
            public FromPeek1Rule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, true },
                    { InteractionController.CardCollectionType.Player, true },
                    { InteractionController.CardCollectionType.CPU, true }
                };

            private readonly CardCollections _CardCollections;

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

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                _NextRule = InteractionRuleType.Default;

                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Player:
                    case InteractionController.CardCollectionType.CPU:
                        {
                            if (_CardCollections.Inspect.Count > 0)
                            {
                                ReturnCard(_CardCollections.Inspect);
                            }
                            else
                            {
                                _NextRule = InteractionRuleType.FromPeek1;
                                _PeekSource = source;

                                var selectedCard = source.RemoveCard(card);
                                _CardCollections.Inspect.AddCard(selectedCard);
                                
                            }

                            break;
                        }
                    case InteractionController.CardCollectionType.Inspect:
                        {
                            if(_CardCollections.Inspect.Count > 0) { ReturnCard(_CardCollections.Inspect); }

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
            #region Public Methods
            public FromDraw2Rule(CardCollections cardCollections)
            {
                _CardCollections = cardCollections;
            }
            #endregion Public Methods

            #region Private Variables
            private readonly Dictionary<InteractionController.CardCollectionType, bool> InteractionTable =
                new Dictionary<InteractionController.CardCollectionType, bool> {
                    { InteractionController.CardCollectionType.Draw, false },
                    { InteractionController.CardCollectionType.Discard, false },
                    { InteractionController.CardCollectionType.Inspect, true },
                    { InteractionController.CardCollectionType.Player, false },
                    { InteractionController.CardCollectionType.CPU, false }
                };

            private readonly CardCollections _CardCollections;

            private InteractionRuleType _NextRule = InteractionRuleType.Default;
            InteractionRuleType IInteractionRule.NextRule => _NextRule;
            #endregion Private Variables

            #region Private Methods         
            bool IInteractionRule.CanInteract(InteractionController.CardCollectionType type)
            {
                if (!InteractionTable.ContainsKey(type)) { throw new System.ArgumentException("type " + type + " does not exist in the interaction table!"); }
                return InteractionTable[type];
            }

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                if(source.Type != InteractionController.CardCollectionType.Inspect) 
                { 
                    throw new System.ArgumentException(ToString() + " does not support card collection type " + source.Type); 
                }
             
                var selectedCard = source.RemoveCard(card);
                _CardCollections.Discard.AddCard(selectedCard);

                var isNextCardNormal = _CardCollections.Inspect.GetFirstCard().Type == Card.CardType.Normal;
                _NextRule = isNextCardNormal ? InteractionRuleType.FromDraw : InteractionRuleType.FromDrawSpecial;
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
                    { InteractionController.CardCollectionType.CPU, true }
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

            void IInteractionRule.Interact(Card card, ICardCollection source)
            {
                switch (source.Type)
                {
                    case InteractionController.CardCollectionType.Player:
                    case InteractionController.CardCollectionType.CPU:
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