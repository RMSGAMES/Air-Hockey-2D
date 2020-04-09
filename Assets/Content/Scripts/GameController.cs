using UnityEngine;
using GameAnalyticsSDK;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _gate;
    [SerializeField] private Transform _playerStart;

    [Header("Other")]
    [SerializeField] private PlayerController _playerController;

    [HideInInspector] public int currentLevel;
    private GameData _gameData;

    public static bool isWin { get; set; }

    private void Awake()
    {
        GameAnalytics.Initialize();

        currentLevel = SceneManager.GetActiveScene().buildIndex;
        _gameData = GameData.Instance;
    }

    private void Start()
    {
        isWin = false;
        _gate.position = new Vector2(Random.Range(-0.8f, 0.8f), _gate.position.y);
    }

    public void RestartLevel()
    {
        _playerController.ResetPosition(_playerStart.position);
    }

    public void LoadNextLevel()
    {
        GameAnalytics.NewDesignEvent(currentLevel.ToString());

        if (currentLevel + 1 < _gameData._levels.Count)
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
    }
}