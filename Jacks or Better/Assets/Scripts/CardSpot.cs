using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSpot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _number;
    [SerializeField] private Image _suitImage;
    [SerializeField] private Image _rankImage;

    private Card _assignedCard;

    public void AssignCard(Card card)
    {
        _assignedCard = card;
    }
}