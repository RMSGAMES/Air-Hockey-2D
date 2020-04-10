using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _currentLevel;

    [Header("Other")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private UIPanelsController _panelsController;

    #region Private
    private GameData _gameData;
    #endregion

    public void Start()
    {
         _gameData = GameData.Instance;
        _currentLevel.text = _gameData._levels[_gameController.currentLevel].showName;
    }

    protected void OnEnable()
    {
        EventHandler.onGameWin += ShowFinishUI;
        EventHandler.onGameLose += ShowFinishUI;
    }

    protected void OnDisable()
    {
        EventHandler.onGameWin -= ShowFinishUI;
        EventHandler.onGameLose -= ShowFinishUI;
    }

    public void ShowFinishUI()
    {
        if (GameController.isWin)
        {
            _audioManager.PlayWonGame();
            _panelsController.ChangeWindow(1);
        }
        else
        {
            _audioManager.PlayLostGame();
            _panelsController.ChangeWindow(2);
        }
    }

    public void LoadLevel(int id)
    {
        _gameController.LoadLevel(id);
    }

    public void NextLevel()
    {
        _gameController.LoadNextLevel();
    }

    public void RestartLevel()
    {
        _panelsController.ChangeWindow(0);
        _gameController.RestartLevel();
    }
}