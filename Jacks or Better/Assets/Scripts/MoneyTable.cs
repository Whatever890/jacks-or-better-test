using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class MoneyTable : MonoBehaviour
{
    [SerializeField] private GameTable _gameTable;
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private MoneyTableColumn[] _columns;
    [SerializeField] private MoneyTableRow[] _rows;

    private Dictionary<Hand, MoneyTableRow> _handRows;

    private bool _columnSwitchAllowed;
    private MoneyTableColumn _activeColumn;
    private MoneyTableRow _activeRow;

    public void Init()
    {
        _handRows = new Dictionary<Hand, MoneyTableRow>();
        foreach (MoneyTableRow row in _rows)
        {
            row.SetSelected(false);
            if (!_handRows.ContainsKey(row.hand))
                _handRows.Add(row.hand, row);
            else
                Debug.LogError($"Duplicate {row.hand} in rows.");
        }

        foreach (MoneyTableColumn column in _columns)
        {
            column.SetSelected(false);
            column.OnSelected += SelectColumn;
        }
        _gameTable.OnPrepared += DeselectRow;
        _gameTable.OnFirstDealt += PayBet;
        _gameTable.OnCombinationFound += SelectCombinationRow;
        _gameTable.OnResolved += GetPrize;
        SetMoneyCount();
    }

    public void Prepare()
    {
        SetColumnSwitchAllow(true);
        SelectColumn(_columns[0]);
    }

    private void SelectColumn(MoneyTableColumn selected)
    {
        if (_activeColumn != null)
            _activeColumn.SetSelected(false);

        selected.SetSelected(true);
        _activeColumn = selected;
        SetDealAllow();
    }

    private void PayBet()
    {
        MoneyManager.ChangeAmountBy(-_activeColumn.bet);
        SetMoneyCount();
        SetColumnSwitchAllow(false);
    }

    private void SelectCombinationRow(Hand foundHand)
    {
        if (foundHand == Hand.None)
            return;
        SelectRow(_handRows[foundHand]);
    }

    private void GetPrize(Hand resolvedHand)
    {
        if (resolvedHand != Hand.None)
        {
            MoneyManager.ChangeAmountBy(_handRows[resolvedHand].betMultiplyValue * _activeColumn.bet);
            SetMoneyCount();
        }
        SetColumnSwitchAllow(true);
        SetDealAllow();
    }

    private void SelectRow(MoneyTableRow selected)
    {
        selected.SetSelected(true);
        _activeRow = selected;
    }

    private void DeselectRow()
    {
        if (_activeRow == null)
            return;
        _activeRow.SetSelected(false);
        _activeRow = null;
    }

    private void SetMoneyCount()
    {
        _moneyCount.text = $"${MoneyManager.GetAmount()}";
    }

    private void SetDealAllow() =>
        _gameTable.SetDealAvailable(MoneyManager.GetAmount() >= _activeColumn.bet);

    private void SetColumnSwitchAllow(bool on)
    {
        _columnSwitchAllowed = on;
        _columns.Where(c => c != _activeColumn).ToList().ForEach(c => c.SetSelectable(on));
    }
}