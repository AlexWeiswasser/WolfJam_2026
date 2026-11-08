using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DockPointsCanvas : MonoBehaviour
{
    [SerializeField] private PointSettings[] settings;

    [System.Serializable]
    class PointSettings
    {
        public BoatScript.ShipType shipType;
        public TMP_Text textLabel;
    }

    void Start()
    {
        ResetText();
        DockScript.DockReached += OnShipDelivered;
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
        text.rectTransform.DOKill();
        text.rectTransform.localScale = Vector3.zero;

        text.rectTransform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
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
}
