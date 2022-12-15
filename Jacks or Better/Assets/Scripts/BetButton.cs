using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BetButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _pressedSprite;
    [SerializeField] private Sprite _unpressedSprite;

    private bool _interactable = true;
    public event System.Action OnPressed;
    private bool _isPressed = false;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!_interactable || _isPressed)
            return;

        OnPressed?.Invoke();
    }

    public void SetPressed(bool on)
    {
        _isPressed = on;
        _image.sprite = on ? _pressedSprite : _unpressedSprite;
    }

    public void SetInteractable(bool on)
    {
        _interactable = on;
        _image.color = on ? Color.white : Color.gray;
    }
}