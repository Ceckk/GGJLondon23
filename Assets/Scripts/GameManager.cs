using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI _highscoreLabel;

    private int _highscore;

    public void UpdateHighScore()
    {
        _highscore = EnemyManager.Instance.SpawnRound * EnemyManager.Instance.SpawnRound * 10 + EnemyManager.Instance.KillCount * 100;
        _highscoreLabel.text = _highscore.ToString();
    }

    public void End()
    {
        PlayerPrefs.SetInt("score", _highscore);
        SceneManager.LoadScene(2);
    }
}
