using UnityEngine;

public class Card
{
    private Suit _suit;
    private Rank _rank;
    private Sprite _suitSprite;
    private string _rankSign;
    private Color _color;

    public Suit suit => _suit;
    public Rank rank => _rank;
    public Sprite suitSprite => _suitSprite;
    public string rankSign => _rankSign;
    public Color color => _color;

    public Card(Suit suit, Rank rank, Sprite suitSprite, string rankSign, Color color)
    {
        _suit = suit;
        _rank = rank;
        _suitSprite = suitSprite;
        _rankSign = rankSign;
        _color = color;
    }
}