using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    private Player _player;
    private Invaders _invaders;
    private Bunker[] _bunkers;

    public Text GameOverUI;
    public Text ScoreText;
    public Text LivesText;

    public int Score;
    public int Lives;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _invaders = FindObjectOfType<Invaders>();
        _bunkers = FindObjectsOfType<Bunker>();
    }

    private void Start()
    {
        _player.Killed += OnPlayerKilled;
        _invaders.Killed += OnInvaderKilled;

        NewGame();
    }

    private void Update()
    {
        if (Lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        GameOverUI.enabled = false;

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        _invaders.ResetInvaders();
        _invaders.gameObject.SetActive(true);

        for (int i = 0; i < _bunkers.Length; i++)
        {
            _bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = _player.transform.position;
        position.x = 0f;
        _player.transform.position = position;
        _player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        GameOverUI.enabled =true;
        _invaders.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.Score = score;
        ScoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this.Lives = Mathf.Max(lives, 0);
        LivesText.text = lives.ToString();
    }

    private void OnPlayerKilled()
    {
        SetLives(Lives - 1);

        _player.gameObject.SetActive(false);

        if (Lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        SetScore(Score + invader.score);

        if (_invaders.AmountKilled == _invaders.TotalAmount)
        {
            NewRound();
        }
    }

}
