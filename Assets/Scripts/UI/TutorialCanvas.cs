using System;
using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] GameObject tutorialOverlay;
    [SerializeField] GameObject tutorialArea;
    [SerializeField] UIButton beginButton;

    public static event Action OnTutorialComplete;
    public static bool TutorialCompleted = false;

    void OnEnable()
    {
        DockScript.TutorialDockReached += ActivateBeginButton;
    }

    void OnDisable()
    {
        DockScript.TutorialDockReached -= ActivateBeginButton;
    }

    void Awake()
    {
        TutorialCompleted = false;
    }

    public void ActivateBeginButton()
    {
        beginButton.SetEnabled(true);
    }

    public void Complete()
    {
        TutorialCompleted = true;
        tutorialOverlay.SetActive(false);
        tutorialArea.SetActive(false);
    }
}
