using System;

namespace Utils
{
	public enum CardSuit
	{
		DIAMONDS,
		CLUBS,
		HEARTS,
		SPADES
	}

	public enum CardRank
	{
		TWO = 2, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN,
		JACK,
		QUEEN,
		KING,
        ACE
	}

    public class Card : IComparable<Card>
    {


        public CardSuit Suit { get; }
        public CardRank Rank { get; }

        public Card(CardRank rank, CardSuit suit)
        {
            this.Suit = suit;
            this.Rank = rank;
        }

        public char GetSuitChar() 
        {
            switch(this.Suit) {
                case CardSuit.CLUBS:
                    return 'C';
                case CardSuit.DIAMONDS:
                    return 'D';
                case CardSuit.HEARTS:
                    return 'H';
                case CardSuit.SPADES:
                    return 'S';
                default:
                    return '\0';              
            }
        }

        public char GetRankChar()
        {
            switch(this.Rank) {
                case CardRank.ACE:
                    return 'A';
                case CardRank.JACK:
                    return 'J';
                case CardRank.QUEEN:
                    return 'Q';
                case CardRank.KING:
                    return 'K';
				default:
                    return ((int)this.Rank).ToString()[0];
            }
        }

        public int GetValue()
        {
            return (int)this.Rank;
        }

        public static bool operator==(Card a, Card b) 
        {
            if (ReferenceEquals(a, b)) return true;
            else if (a == null || b == null) return false;
            else return a.Equals(b);
        }

        public static bool operator!=(Card a, Card b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Card);
        }

        public bool Equals(Card c)
        {
            if (c == null) return false;
            return this.Rank == c.Rank && this.Suit == c.Suit;
        }

        public override int GetHashCode()
        {
            return this.Suit.GetHashCode() * this.Rank.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", this.GetRankChar(), this.GetSuitChar());
        }

        public int CompareTo(Card other)
        {
            // we compare cards by their rank, not by their suit
            return this.Rank.CompareTo(other.Rank);
        }
    }
}
