using System;
using System.Text;
using System.Collections.Generic;
using Extensions;

namespace Utils
{
    public class Deck
    {
        private IList<Card> cards;
        private Random generator = new Random();

        public Deck()
        {
            cards = new List<Card>();
            ResetDeck();
        }

        public Deck(Card[] subdeck)
        {
            this.cards = subdeck;
        }

        public void ShuffleDeck()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                cards.Swap(GetRandomIndex(), GetRandomIndex());
            }
        }

        public void ResetDeck()
        {
            cards.Clear();

            foreach (var suit in (CardSuit[])Enum.GetValues(typeof(CardSuit)))
            {
                foreach (var rank in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    cards.Add(new Card(rank, suit));
                }
            }
        }

        public void AddCardTop(Card card)
        {
            cards.Insert(0, card);
        }

        public void AddCardBottom(Card card)
        {
            cards.Add(card);
        }

        public void AddCardRandom(Card card)
        {
            cards.Insert(GetRandomIndex(), card);
        }

        public Card DrawCard()
        {
			Card ret = cards[0];
			cards.RemoveAt(0);
            return ret;
        }

        private int GetRandomIndex()
		{
			return generator.Next(cards.Count);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in cards)
            {
                sb.Append(item.ToString() + " ");
            }
            return sb.Remove(sb.Length - 1, 1).ToString(); // remove last space
        }
    }
}
