using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] GameObject gameOverOverlay;
    [SerializeField] CustomText finalScoreText;
    void OnEnable()
    {
        ReputationUIScript.OnReputationReachedZero += ShowDeath;
    }

    void OnDisable()
    {
        ReputationUIScript.OnReputationReachedZero -= ShowDeath;
    }

    void Awake()
    {
        GameOver = false;
    }
    public static bool GameOver = false;

    void ShowDeath()
    {
        Time.timeScale = 0;
        GameOver = true;
        gameOverOverlay.SetActive(true);
        finalScoreText.SetText($"Final Score:\n{PointTracker.GetPoints()}");

		Cursor.visible = true;
	}

	public void Retry()
    {
        PointTracker.ResetMult();
        PointTracker.ResetPoints();
        ReputationTracker.ResetReputation();
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
		PointTracker.ResetMult();
		PointTracker.ResetPoints();
        ReputationTracker.ResetReputation();
		SceneLoader.Instance.LoadScene(SceneLoader.mainMenuName);
    }
}
