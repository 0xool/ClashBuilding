


public enum ZONE
{
    LEFTZONE,
    RIGHTZONE
}

public enum BuildingType
{
    HQ,
    DEFENSE,
    SPAWNER
}

public enum UnitState {
    MOVING,
    ATTACKING
}

public static class UnitMovmentLocation
{
    public static readonly string ZoneEnemyLeft = "Enemy_Zone_Left";
    public static readonly string ZoneEnemyRight = "Enemy_Zone_Right";
    public static readonly string ZoneEnemyHQ = "Enemy_Zone_HQ";
    public static readonly string ZoneFriendlyRight = "Friendly_Zone_Right";
    public static readonly string ZoneFriendlyLeft = "Friendly_Zone_Left";
    public static readonly string ZoneFriendlyHQ = "Friendly_Zone_HQ";
}