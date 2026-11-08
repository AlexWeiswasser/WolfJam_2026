using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerHandler.pauseEvent += Pause;
    }

    private void OnDisable()
    {
        PlayerHandler.pauseEvent -= Pause;
    }

    [SerializeField] GameObject pauseOverlay;
    [SerializeField] Image backgroundImage; 
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject pauseButton;

    bool allowedToPause = false;
    public static bool Paused = false;

    void Awake()
    {
        Paused = false;
    }
    
    public void Pause()
    {
        if (Paused)
        {
            UnPause();
            return;
        }

        Paused = true;
        pauseOverlay.SetActive(true);
        Time.timeScale = 0;

        Cursor.visible = true;
    }

    public void UnPause()
    {
        Paused = false;
        pauseOverlay.SetActive(false);
        Time.timeScale = 1;

        Cursor.visible = false;
    }

    public void Quit()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.mainMenuName);
    }
}
