using System;
using TMPro;
using UnityEngine;

public class ReputationUIScript : MonoBehaviour
{
    [SerializeField] RectTransform reputationFill;
    [SerializeField] CustomText reputationText;
    [SerializeField] float timeBetweenPoints = 0.03f;

    readonly int MinAnchorSize = 0;
    readonly int MaxAnchorSize = 200;
    int Difference => MaxAnchorSize - MinAnchorSize;

    int internalRepCount = ReputationTracker.MaxReputation;
    int currentTargetRep = ReputationTracker.MaxReputation;
    float accumulator = 0;

    public static event Action OnReputationReachedZero;

    void Start()
    {
        internalRepCount = ReputationTracker.MaxReputation;
        DisplayReputation();
    }

    void OnEnable()
    {
        ReputationTracker.OnReputationChanged += UpdateReputation;
    }

    void OnDisable()
    {
        ReputationTracker.OnReputationChanged -= UpdateReputation;
    }

    void Update()
    {
        if (!GameOverCanvas.GameOver)
        {
            if (internalRepCount <= 0)
            {
                OnReputationReachedZero?.Invoke();
            }
        }

        float timeBetween = timeBetweenPoints;
        if (accumulator < timeBetween)
        {
            accumulator += Time.deltaTime;
            return;   
        }
        int scoreGainedThisFrame = (int)(accumulator / timeBetween);
        accumulator -= timeBetween * scoreGainedThisFrame;

        if (internalRepCount < currentTargetRep)
        {
            internalRepCount += scoreGainedThisFrame;
            DisplayReputation();
        }
        else if (internalRepCount > currentTargetRep)
        {
            internalRepCount -= scoreGainedThisFrame;
            DisplayReputation();
        }

        
    }

    void UpdateReputation(int reputationGained, Vector2Int range)
    {
        currentTargetRep = range.y;
    }

    void DisplayReputation()
    {
        reputationText.SetText($"{internalRepCount}/{ReputationTracker.MaxReputation}");
        float ratio = Mathf.Clamp01((float)internalRepCount / ReputationTracker.MaxReputation);
        float width = MinAnchorSize + Difference * ratio;

        Vector2 size = reputationFill.sizeDelta;
        size.x = width;
        reputationFill.sizeDelta = size;
    }
}
