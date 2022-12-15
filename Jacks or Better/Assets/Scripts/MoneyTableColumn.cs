using UnityEngine;
using UnityEngine.UI;

public class MoneyTableColumn : MonoBehaviour
{
    [SerializeField] private int _bet;
    public int bet => _bet;

    [SerializeField] private BetButton _betButton;
    [SerializeField] private Image _selection;

    public event System.Action<MoneyTableColumn> OnSelected;

    private void Start() =>
        _betButton.OnPressed += Select;

    private void OnDestroy() =>
        _betButton.OnPressed -= Select;

    private void Select() =>
        OnSelected?.Invoke(this);

    public void SetSelected(bool on)
    {
        _betButton.SetPressed(on);
        _selection.color = on ? new Color(1f,1f,1f,0.3f) : Color.clear;
    }

    public void SetSelectable(bool on) =>
        _betButton.SetInteractable(on);
}