using UnityEngine;
using UnityEngine.UI;

public class MoneyTableRow : MonoBehaviour
{
    [SerializeField] private Hand _hand;
    [SerializeField] private int _betMultiplyValue;
    [SerializeField] private Image _selection;

    public Hand hand => _hand;
    public int betMultiplyValue => _betMultiplyValue;

    public void SetSelected(bool on) =>
        _selection.color = on ? new Color(1f,1f,1f,0.3f) : Color.clear;
}