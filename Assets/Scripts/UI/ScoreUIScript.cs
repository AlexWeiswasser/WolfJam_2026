using System.Collections;
using System.Drawing;
using UnityEngine;

public class ScoreUIScript : MonoBehaviour
{
    [SerializeField] CustomText scoreText;
    int internalPointCount = 0;
    int currentTargetPoints = 0;
    [SerializeField] float timeBetweenPoints = 0.05f;
    [SerializeField] int pointDifToGoDoubleSpeed = 50;
    [SerializeField] int pointDifToGoTripleSpeed = 100;
    [SerializeField] int pointDifToGoASAP = 250;
    bool doubleSpeed = false;
    bool tripleSpeed = false;
    bool asap = false;
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
        float timeBetween = doubleSpeed ? timeBetweenPoints * 0.5f : timeBetweenPoints;
        timeBetween = tripleSpeed ? timeBetweenPoints * 0.333f : timeBetween;
        timeBetween = asap ? timeBetweenPoints * 0.02f : timeBetween;
        if (accumulator < timeBetween)
        {
            accumulator += Time.deltaTime;
            return;   
        }
        int pointsGainedThisFrame = (int)(accumulator / timeBetween);
        accumulator -= timeBetween * pointsGainedThisFrame;

        if (internalPointCount < currentTargetPoints)
        {
            internalPointCount += pointsGainedThisFrame;
            if (internalPointCount > currentTargetPoints) internalPointCount = currentTargetPoints;
            UpdateText();
        }
        else if (internalPointCount > currentTargetPoints)
        {
            internalPointCount = currentTargetPoints;
            doubleSpeed = false;
            tripleSpeed = false;
            asap = false;
        }
        else
        {
            doubleSpeed = false;
            tripleSpeed = false;
            asap = false;
        }
    }

    void UpdatePoints(int pointsGained, Vector2Int range)
    {
        currentTargetPoints = range.y;
        if (currentTargetPoints - pointDifToGoDoubleSpeed > internalPointCount) doubleSpeed = true;
        if (currentTargetPoints - pointDifToGoTripleSpeed > internalPointCount) tripleSpeed = true;
        if (currentTargetPoints - pointDifToGoASAP > internalPointCount) asap = true;
    }

    void UpdateText()
    {
        scoreText.SetText(internalPointCount.ToString());
    }
}
