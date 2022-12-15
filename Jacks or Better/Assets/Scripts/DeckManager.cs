using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Sprite _clubsSprite;
    [SerializeField] private Sprite _diamondsSprite;
    [SerializeField] private Sprite _heartsSprite;
    [SerializeField] private Sprite _spadesSprite;

    [SerializeField] private Color _blackColor;
    [SerializeField] private Color _redColor;

    private Dictionary<Suit, Sprite> _suitSprites;

    private List<Card> _deck;

    public void Init()
    {
        _suitSprites = new Dictionary<Suit, Sprite>()
        {
            {Suit.Clubs, _clubsSprite},
            {Suit.Diamonds, _diamondsSprite},
            {Suit.Hearts, _heartsSprite},
            {Suit.Spades, _spadesSprite}
        };
        _deck = new List<Card>();

        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                string sign = (int)rank <= 10 ? ((int)rank).ToString() : rank.ToString();
                Color color = suit == Suit.Clubs || suit == Suit.Spades ? _blackColor : _redColor;
                Card card = new Card(suit, rank, _suitSprites[suit], sign, color);
                _deck.Add(card);
            }
        }
    }

    public Card[] GetRandomCards(int count)
    {
        Card[] cards = new Card[count];
        for (int i = 0; i < count; i++)
        {
            Card randCard = _deck[Random.Range(0, _deck.Count)];
            cards[i] = randCard;
            _deck.Remove(randCard);
        }
        return cards;
    }

    public void ReturnCards(Card[] cards)
    {
        _deck.AddRange(cards);
    }
}