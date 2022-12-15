using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameTableCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _backSide;
    [SerializeField] private GameObject _frontSide;

    [SerializeField] private Image[] _suitImages;
    [SerializeField] private TextMeshProUGUI[] _rankSigns;

    [SerializeField] private GameObject _holdVisual;
    [SerializeField] private GameObject _fade;

    private Card _assignedCard;
    public Card assignedCard => _assignedCard;

    private bool _holdAllowed = false;
    private bool _hold = false;
    public bool hold => _hold;

    public void AssignCard(Card card)
    {
        _assignedCard = card;
        _suitImages.ToList().ForEach(i => i.sprite = card.suitSprite);
        _rankSigns.ToList().ForEach(s =>
        {
            s.text = card.rankSign;
            s.color = card.color;
        });
    }

    public void ShowBack()
    {
        _backSide.SetActive(true);
        _frontSide.SetActive(false);
    }

    public void ShowFront()
    {
        _backSide.SetActive(false);
        _frontSide.SetActive(true);
    }

    public void Retrieve()
    {
        _assignedCard = null;
        _backSide.SetActive(false);
        _frontSide.SetActive(false);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!_holdAllowed)
            return;

        _hold = !_hold;
        _holdVisual.SetActive(_hold);
    }

    public void AllowHold(bool on) =>
        _holdAllowed = on;

    public void ForceHold(bool on)
    {
        _hold = on;
        _holdVisual.SetActive(on);
    }

    public void SetFade(bool on) =>
        _fade.SetActive(on);
}