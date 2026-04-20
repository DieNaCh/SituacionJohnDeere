using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public GameObject gameOverUI;
    public GameObject victoryUI;

    public int score = 0;
    public int winScore = 10;

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (victoryUI != null)
            victoryUI.SetActive(false);

        UpdateScore();

        if (SerialManager.Instance != null)
        {
            SerialManager.Instance.SendScore(score);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();


        if (SerialManager.Instance != null)
        {
            SerialManager.Instance.SendScore(score);
        }
        
        if (score >= winScore)
        {
            WinGame();
        }
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Maíces: " + score;
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}