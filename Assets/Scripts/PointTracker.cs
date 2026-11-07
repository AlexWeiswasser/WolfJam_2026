using System;
using UnityEngine;

public static class PointTracker
{
    static int points;
    static float? pointMult = null;
    static int pointsPerBoat = 100;

    // Passes the amount of points gained and what the point value should start and end at. (eg. gained 100 points, go from 200 to 300)
    public static event Action<int, Vector2Int> OnPointsGained;

    public static void ResetPoints() { points = 0; }
    public static void ResetMult() { pointMult = null; }
    public static void GainPoints() 
    {
        if (pointMult == null) pointMult = 0.1f;
        else pointMult += .1f;
        int pointsGained = (int)(pointsPerBoat * pointMult); 
        int prevPoints = points;
		points += pointsGained;
        OnPointsGained?.Invoke(pointsGained, new Vector2Int(prevPoints, points));
    }
    public static int GetPoints() => points; 
}

public static class ReputationTracker
{
    static int reputation = 100;
    static int reputationGain = 5;
    static int reputationLoss = 20;
    public static void ResetReputation() { reputation = 100; }
    public static event Action<int, Vector2Int> OnReputationChanged;

    public static void AddReputation()
    {
        int prevReputation = reputation;
        reputation += reputationGain;
        OnReputationChanged?.Invoke(reputationGain, new Vector2Int(prevReputation, reputation));
    }

    public static void LoseReputation()
    {
        int prevReputation = reputation;
        reputation -= reputationLoss;
        OnReputationChanged?.Invoke(-reputationLoss, new Vector2Int(prevReputation, reputation));

    }
    public static int GetReputation() => reputation;
}
