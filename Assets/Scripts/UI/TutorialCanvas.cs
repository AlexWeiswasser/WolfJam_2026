using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Cursor.visible = false;
    }

    public void ActivateBeginButton()
    {
        beginButton.SetEnabled(true);
		Cursor.visible = true;
	}

	public void Complete()
    {
        TutorialCompleted = true;
        tutorialOverlay.SetActive(false);
        tutorialArea.SetActive(false);
		Cursor.visible = false;
	}
}
