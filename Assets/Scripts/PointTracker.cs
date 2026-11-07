public static class PointTracker
{
    static int points;
    static float? pointMult = null;
    static int pointsPerBoat = 100;

    public static void ResetPoints() { points = 0; }
    public static void ResetMult() { pointMult = null; }
    public static void GainPoints() 
    {
        if (pointMult == null) pointMult = 0;
        else pointMult += .1f;
		points += (int)(pointsPerBoat * pointMult); 
    }
    public static int GetPoints() { return points; }
}
