using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DockPointsCanvas : MonoBehaviour
{
    [SerializeField] private PointSettings[] settings;

    TMP_Text recentText;

    [System.Serializable]
    class PointSettings
    {
        public BoatScript.ShipType shipType;
        public TMP_Text textLabel;
    }

    void Start()
    {
        ResetText();
    }

	void OnEnable()
	{
		DockScript.DockReached += OnShipDelivered;
		DockScript.TutorialDockReached += OnTutorialDockReached;
		PointTracker.OnPointsGained += OnPointsGained;
	}

	void OnDisable()
	{
		DockScript.DockReached -= OnShipDelivered;
		DockScript.TutorialDockReached -= OnTutorialDockReached;
		PointTracker.OnPointsGained -= OnPointsGained;
	}

	private void OnShipDelivered(BoatScript.ShipType type)
    {
        StartCoroutine(TweenLabel(GetPointSettings(type).textLabel));
    }

    private PointSettings GetPointSettings(BoatScript.ShipType type)
    {
        for (int i = 0; i < settings.Length; i++)
        {
            if (settings[i].shipType == type)
            {
                return settings[i];
            }
        }

        return null;
    }

    private IEnumerator TweenLabel(TMP_Text text)
    {
        recentText = text;
        text.rectTransform.DOKill();
        text.rectTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

        text.rectTransform.DOShakeAnchorPos(0.2f, 5, 50);
        text.rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(2);
        text.rectTransform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InCubic);
    }

    private void ResetText()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            settings[i].textLabel.rectTransform.localScale = Vector3.zero;
        }
    }

    // hacky way to make tutorial not show points label
    private void OnTutorialDockReached()
    {
        var label = GetPointSettings(BoatScript.ShipType.Viking).textLabel;
        label.rectTransform.DOKill();
        label.rectTransform.localScale = Vector2.zero;
    }

    private void OnPointsGained(int points, Vector2Int gained)
    {
        recentText.text = points.ToString();
    }
}
