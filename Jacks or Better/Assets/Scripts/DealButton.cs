using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DealButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    private bool _interactable = true;
    public event System.Action OnPressed;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!_interactable)
            return;

        OnPressed?.Invoke();
    }

    public void SetText(string text) =>
        _text.text = text;

    public void SetInteractable(bool on)
    {
        _interactable = on;
        _image.color = on ? Color.white : Color.gray;
    }
}