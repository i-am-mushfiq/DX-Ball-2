using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int NumLevels = 2;

    private Ball _ball;
    private Paddle _paddle;
    private Brick[] _bricks;

    public int Level { get; private set; } = 1;
    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindSceneReferences();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void FindSceneReferences()
    {
        _ball = FindObjectOfType<Ball>();
        _paddle = FindObjectOfType<Paddle>();
        _bricks = FindObjectsOfType<Brick>();
    }

    private void LoadLevel(int level)
    {
        Level = level;

        if (level > NumLevels)
        {
            LoadLevel(1);
            return;
        }

        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.LoadScene($"Level{level}");
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        FindSceneReferences();
    }

    public void OnBallMiss()
    {
        Lives--;

        if (Lives > 0)
        {
            ResetLevel();
        }
        else
        {
            GameOver();
        }
    }

    private void ResetLevel()
    {
        _paddle.ResetPaddle();
        // _ball.ResetBall();
    }

    private void GameOver()
    {
        NewGame();
    }

    private void NewGame()
    {
        Score = 0;
        Lives = 3;

        LoadLevel(1);
    }

    public void OnBrickHit(Brick brick)
    {
        Score += brick.Points;

        if (Cleared())
        {
            LoadLevel(Level + 1);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < _bricks.Length; i++)
        {
            if (_bricks[i].gameObject.activeInHierarchy && !_bricks[i].Unbreakable)
            {
                return false;
            }
        }

        return true;
    }
}
