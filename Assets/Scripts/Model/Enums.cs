


public enum ZONE
{
    LEFTZONE,
    RIGHTZONE
}

public enum BuildingType
{
    HQ,
    DEFENSE,
    SPAWNER,
    REFINERY
}

public enum BuildingState
{
    IDLE,
    DYING
}

public enum UnitState {
    MOVING,
    ATTACKING,
    DYING
}

public enum BuildingMode{
    CONSTRUCTION,
    SPAWNING,
    ATTACKING,
    DESTRUCTION,
    IDLE,
}

public static class UnitMovmentLocation
{
    public static readonly string ZonePlayerTwoLeft = "PlayerTwo_Zone_Left";
    public static readonly string ZonePlayerTwoRight = "PlayerTwo_Zone_Right";
    public static readonly string ZonePlayerTwoHQ = "PlayerTwo_Zone_HQ";
    public static readonly string ZonePlayerOneRight = "PlayerOne_Zone_Right";
    public static readonly string ZonePlayerOneLeft = "PlayerOne_Zone_Left";
    public static readonly string ZonePlayerOneHQ = "PlayerOne_Zone_HQ";
}