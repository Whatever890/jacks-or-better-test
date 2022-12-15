using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private MoneyTable _moneyTable;
    [SerializeField] private GameTable _gameTable;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameMenu;

    private void Start()
    {
        _deckManager.Init();
        _moneyTable.Init();
    }

    public void LoadMainMenu()
    {
        _mainMenu.SetActive(true);
        _gameMenu.SetActive(false);
        _gameTable.ResetGame();
    }

    public void LoadGame()
    {
        _mainMenu.SetActive(false);
        _gameMenu.SetActive(true);
        _moneyTable.Prepare();
        _gameTable.Prepare();
    }
}