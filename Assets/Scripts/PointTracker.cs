using System;

public static class PointTracker
{
    static int points;
    static float? pointMult = null;
    static int pointsPerBoat = 100;

    // Passes the amount of points gained
    public static event Action<int> OnPointsGained;

    public static void ResetPoints() { points = 0; }
    public static void ResetMult() { pointMult = null; }
    public static void GainPoints() 
    {
        if (pointMult == null) pointMult = 0;
        else pointMult += .1f;
        int pointsGained = (int)(pointsPerBoat * pointMult); 
		points += pointsGained;
        OnPointsGained?.Invoke(pointsGained);
    }
    public static int GetPoints() { return points; }
}
