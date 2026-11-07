using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
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
        Paused = true;
        pauseOverlay.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Paused = false;
        pauseOverlay.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void Quit()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.mainMenuName);
    }
}
