using System.Collections;
using System.Linq;
using UnityEngine;

public class GameTable : MonoBehaviour
{
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private GameTableCard[] _tableCards;
    [SerializeField] private DealButton _dealButton;

    [SerializeField] private float _dealDelay = 0.3f;

    public event System.Action OnPrepared;
    public event System.Action OnFirstDealt;
    public event System.Action OnSecondDealt;
    public event System.Action<Hand> OnCombinationFound;
    public event System.Action<Hand> OnResolved;

    private int _dealsLeft;
    private Hand _resolvedHand;

    public void Prepare()
    {
        RetrieveAll();
        ResetHold();
        ResetFade();
        _dealsLeft = 2;
        _dealButton.SetText("DEAL");
        _dealButton.OnPressed += DealCards;
        OnPrepared?.Invoke();
    }

    private void Restart()
    {
        _dealButton.OnPressed -= Restart;
        OnResolved?.Invoke(_resolvedHand);
        ReturnAllCards();
        Prepare();
    }

    private void DealCards()
    {
        _dealButton.OnPressed -= DealCards;
        _dealButton.SetInteractable(false);
        AllowHold(false);

        _dealsLeft--;
        if (_dealsLeft == 1)
            OnFirstDealt?.Invoke();
        else if (_dealsLeft == 0)
            OnSecondDealt?.Invoke();
    
        GameTableCard[] cards = _tableCards.Where(c => !c.hold).ToArray();
        Card[] cardsToDeal = _deckManager.GetRandomCards(cards.Length);
        Card[] cardsToReturn = cards.Where(c => c.assignedCard != null).Select(c => c.assignedCard).ToArray();
        _deckManager.ReturnCards(cardsToReturn);
        StartCoroutine(DealCardsRoutine(cards, cardsToDeal));
    }

    private IEnumerator DealCardsRoutine(GameTableCard[] cards, Card[] cardsToDeal)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].ShowBack();
            cards[i].AssignCard(cardsToDeal[i]);
            yield return new WaitForSeconds(_dealDelay);
        }
        foreach (GameTableCard card in cards)
        {
            card.ShowFront();
            yield return new WaitForSeconds(_dealDelay);
        }
        Resolve();
    }

    private void Resolve()
    {
        GameHand hand = new GameHand(_tableCards);
        GameTableCard[] combinationCards = hand.FindCombination(out _resolvedHand);

        if (_dealsLeft > 0)
        {
            _dealButton.OnPressed += DealCards;
            AllowHold(true);

            if (combinationCards != null)
                combinationCards.ToList().ForEach(c => c.ForceHold(true));
        }
        else
        {
            _dealButton.OnPressed += Restart;
            ResetHold();
            AllowHold(false);
            OnCombinationFound?.Invoke(_resolvedHand);

            if (combinationCards == null)
            {
                _dealButton.SetText("RESTART");
                _tableCards.ToList().ForEach(c => c.SetFade(true));
            }
            else
            {
                _dealButton.SetText("COLLECT");
                _tableCards
                    .Where(c => !combinationCards.Contains(c))
                    .ToList()
                    .ForEach(c => c.SetFade(true));
            }
        }
        _dealButton.SetInteractable(true);
    }

    private void AllowHold(bool on) =>
        _tableCards.ToList().ForEach(c => c.AllowHold(on));

    private void ResetHold() =>
        _tableCards.ToList().ForEach(c => c.ForceHold(false));

    private void ResetFade() =>
        _tableCards.ToList().ForEach(c => c.SetFade(false));

    private void RetrieveAll() =>
        _tableCards.ToList().ForEach(c => c.Retrieve());

    private void ReturnAllCards() =>
        _deckManager.ReturnCards(_tableCards.Where(c => c.assignedCard != null).Select(c => c.assignedCard).ToArray());

    public void ResetGame()
    {
        ReturnAllCards();
        _dealButton.OnPressed -= DealCards;
        _dealButton.OnPressed -= Restart;
    }

    public void SetDealAvailable(bool on) =>
        _dealButton.SetInteractable(on);
}