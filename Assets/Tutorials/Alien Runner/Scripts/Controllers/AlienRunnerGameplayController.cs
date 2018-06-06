using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AlienRunnerGameplayController : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private Button restartGameButton;

    [SerializeField]
    private Text scoreText, pauseText;

    private int score;

	// Use this for initialization
	void Start () {
        scoreText.text = score.ToString() + "m";
        StartCoroutine(CountScore());
	}

    //Infinte score increase every 0.6 seconds
    IEnumerator CountScore()
    {
        yield return new WaitForSeconds(0.6f);
        score++;
        scoreText.text = score.ToString() + "m";
        StartCoroutine(CountScore());
    }


	void OnEnable () {
        PlayerDied.endGame += PlayerDiedEndTheGame;
	}
    void OnDisable()
    {
        PlayerDied.endGame -= PlayerDiedEndTheGame;
    }

    void PlayerDiedEndTheGame()
    {

        if (!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetInt("Score", 0);
        }
        else
        {
            int highScore = PlayerPrefs.GetInt("Score");
            if (highScore < score)
            {
                PlayerPrefs.SetInt("Score", score);
                
            }
        }

        pauseText.text = "Game Over";
        pausePanel.SetActive(true);

        restartGameButton.onClick.RemoveAllListeners();
        restartGameButton.onClick.AddListener(() => RestartGame());
        Time.timeScale = 0f;

    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);

        restartGameButton.onClick.RemoveAllListeners();
        restartGameButton.onClick.AddListener(() => ResumeGame());

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void GoToTitleScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Alien Runner Title Screen");
    }
}
