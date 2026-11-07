using System.Collections;
using System.Drawing;
using UnityEngine;

public class ScoreUIScript : MonoBehaviour
{
    [SerializeField] CustomText scoreText;
    int internalPointCount = 0;
    int currentTargetPoints = 0;
    [SerializeField] float timeBetweenPoints = 0.05f;
    float accumulator = 0;

    void OnEnable()
    {
        PointTracker.OnPointsGained += UpdatePoints;
    }

    void OnDisable()
    {
        PointTracker.OnPointsGained -= UpdatePoints;
    }

    void Start()
    {
        internalPointCount = 0;
        UpdatePoints(0, new Vector2Int(0, 0));
    }

    void Update()
    {
        if (accumulator < timeBetweenPoints)
        {
            accumulator += Time.deltaTime;
            return;   
        }
        int pointsGainedThisFrame = (int)(accumulator / timeBetweenPoints);
        accumulator -= timeBetweenPoints * pointsGainedThisFrame;

        if (internalPointCount < currentTargetPoints)
        {
            internalPointCount += pointsGainedThisFrame;
            UpdateText();
        }
        else if (internalPointCount > currentTargetPoints)
        {
            internalPointCount = currentTargetPoints;
        }
    }

    void UpdatePoints(int pointsGained, Vector2Int range)
    {
        currentTargetPoints = range.y;
    }

    void UpdateText()
    {
        scoreText.SetText(internalPointCount.ToString());
    }
}
