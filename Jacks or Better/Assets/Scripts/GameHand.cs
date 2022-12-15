using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameHand
{
    private GameTableCard[] _cards;

    private bool _isStraight;
    private bool _isRoyal;
    private bool _isFlush;
    private GameTableCard[][] _rankCombinations;

    public GameHand(GameTableCard[] cards)
    {
        _cards = cards;
    }

    public GameTableCard[] FindCombination(out Hand handType)
    {
        _cards = _cards.OrderBy(c => c.assignedCard.rank).ToArray();

        _isStraight = true;
        for (int i = 0; i < _cards.Length-1; i++)
        {
            int dif = (int)_cards[i+1].assignedCard.rank - (int)_cards[i+1].assignedCard.rank;
            if (dif != 1)
            {
                _isStraight = false;
                break;
            }
        }

        _isRoyal = _isStraight && _cards[0].assignedCard.rank == Rank.Ten;

        GameTableCard first = _cards[0];
        _isFlush = _cards.Skip(1).All(c => first.assignedCard.suit == c.assignedCard.suit);

        _rankCombinations = _cards
            .GroupBy(c => c.assignedCard.rank)
            .Select(g => g.ToArray())
            .Where(g => g.Length > 1)
            .ToArray();


        if (_isRoyal)
        {
            handType = Hand.RoyalFlush;
            return _cards;
        }
        if (_isStraight && _isFlush)
        {
            handType = Hand.StraightFlush;
            return _cards;
        }
        if (_rankCombinations.Length == 1 && _rankCombinations[0].Length == 4)
        {
            handType = Hand.FourOfAKind;
            return _rankCombinations[0];
        }
        if (_rankCombinations.Length == 2 && _rankCombinations.Any(c => c.Length == 3))
        {
            handType = Hand.FullHouse;
            return _cards;
        }
        if (_isFlush)
        {
            handType = Hand.Flush;
            return _cards;
        }
        if (_isStraight)
        {
            handType = Hand.Straight;
            return _cards;
        }
        if (_rankCombinations.Length == 1 && _rankCombinations[0].Length == 3)
        {
            handType = Hand.ThreeOfAKind;
            return _rankCombinations[0];
        }
        if (_rankCombinations.Length == 2)
        {
            handType = Hand.TwoPair;
            return _rankCombinations[0].Concat(_rankCombinations[1]).ToArray();
        }
        if (_rankCombinations.Length == 1 && _rankCombinations[0][0].assignedCard.rank >= Rank.J)
        {
            handType = Hand.JacksOrBetter;
            return _rankCombinations[0];
        }
        handType = Hand.None;
        return null;
    }
}