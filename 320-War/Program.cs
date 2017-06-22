using System;
using System.Diagnostics;
using System.Collections.Generic;
using Utils;

namespace War
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WarGame game = new WarGame();
            game.Play();
        }
    }

    class WarGame
	{
        
		private class Player
		{
            public Deck Deck { get; }

			public Player(Card[] cards)
			{
				Deck = new Deck(cards);
			}

            public Player(Deck d)
            {
                Deck = d;
            }
		}

        Player player1;
        Player player2;

        private int turn;
        private int warCount = 0;
        private const int CARDS_TO_DRAW_WAR = 4;



        public WarGame()
        {
            InitDecks();
            turn = 0;
        }

        public void Play()
        {
            while (!IsFinished())
			{
				warCount = 0;
                turn++;
                Console.WriteLine("\nTurn number {0}:", turn);
                Card p1Card = player1.Deck.DrawCard();
                Card p2Card = player2.Deck.DrawCard();
                Console.WriteLine("Player 1 card: {0}. Player 2 card: {1}",
                                  p1Card, p2Card);

                Player winner = CompareCards(p1Card, p2Card);

				Console.WriteLine("{0} won the round!", GetPlayerName(winner));
				AddCardToWinner(p1Card, p2Card, winner);
            }

			if (player1.Deck.CardsLeft() == 0)
				Console.WriteLine("Player 2 wins!");
			else
				Console.WriteLine("Player 1 wins!");
        }

        private Player CompareCards(Card p1Card, Card p2Card)
		{
            if (p1Card.GetValue() > p2Card.GetValue())
                return player1;
            else if (p1Card.GetValue() < p2Card.GetValue())
                return player2;
			else
				return War(p1Card, p2Card);
        }

        private Player War(Card p1Card, Card p2Card)
        {
            warCount++;
            if (warCount == 2)
            {
                Console.WriteLine(":O");
            }
            Console.WriteLine("War started!!");
            List<Card> p1WarCards = new List<Card>();
            List<Card> p2WarCards = new List<Card>();

            if (player1.Deck.CardsLeft() == 0)
            {
                Console.WriteLine("Player 1 ran out of cards!");
                return player2;
            }
            else if (player2.Deck.CardsLeft() == 0)
            {
                Console.WriteLine("Player 2 ran out of cards!");
                return player1;
            }

            int leastCardsDeck = player1.Deck.CardsLeft() < player2.Deck.CardsLeft() ?
                                            player1.Deck.CardsLeft() : player2.Deck.CardsLeft();
            int cardsToDraw = Math.Min(CARDS_TO_DRAW_WAR, leastCardsDeck);
            for (int i = 0; i < cardsToDraw; i++)
            {
                p1WarCards.Add(player1.Deck.DrawCard());
                p2WarCards.Add(player2.Deck.DrawCard());
            }



            Console.WriteLine("Player 1 card: {0}. Player 2 card: {1}",
                                  p1WarCards[cardsToDraw - 1], p2WarCards[cardsToDraw - 1]);
            // we compare the last card of the drawn cards
            Player winner = CompareCards(p1WarCards[cardsToDraw - 1],
                                         p2WarCards[cardsToDraw - 1]);

            AddCardsToWinner(p1WarCards, p2WarCards, winner);
            return winner;
        }

        private void AddCardsToWinner(IEnumerable<Card> cardsDrawP1, 
                                      IEnumerable<Card> cardsDrawnP2, Player winner)
        {
            foreach(var card in cardsDrawP1) 
                winner.Deck.AddCardBottom(card);
            
            foreach (var card in cardsDrawnP2)
                winner.Deck.AddCardBottom(card);
        }

        private void AddCardToWinner(Card cardDrawP1, Card cardDrawnP2, Player winner)
        {
            winner.Deck.AddCardBottom(cardDrawP1);
            winner.Deck.AddCardBottom(cardDrawnP2);
        }

        private bool IsFinished()
        {
            return player1.Deck.CardsLeft() == 0 || player2.Deck.CardsLeft() == 0;
        }

        private void InitDecks()
		{
			Deck d = new Deck();
			d.ShuffleDeck();
			Card[] cardsP1 = new Card[d.CardsLeft() / 2];
			Card[] cardsP2 = new Card[d.CardsLeft() / 2];
			for (int i = 0; i < cardsP1.Length; i++)
			{
				cardsP1[i] = d.DrawCard();
				cardsP2[i] = d.DrawCard();
			}

            player1 = new Player(cardsP1);
            player2 = new Player(cardsP2);
        }

        private string GetPlayerName(Player p)
        {
            Debug.Assert(p.Equals(player1) || p.Equals(player2));

            if (p.Equals(player1)) return "P1";
            else return "P2";
        }
    }
}
