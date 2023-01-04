
public static class Utilities {
    public static string GetLeftZoneTag(BuildingType buildingType) {
        if(GameManager.instance.IsPlayerTwo())
            return (buildingType == BuildingType.SPAWNER) ? "ZoneLeftFriendly" : "ZoneFriendlyDefence";
        else
            return (buildingType == BuildingType.SPAWNER) ? "ZoneLeftEnemy" : "ZoneEnemyDefence";
    }

    public static string GetRightZoneTag(BuildingType buildingType) {
        if(GameManager.instance.IsPlayerTwo())
            return (buildingType == BuildingType.SPAWNER) ? "ZoneRightFriendly" : "ZoneFriendlyDefence";
        else
            return (buildingType == BuildingType.SPAWNER) ? "ZoneRightEnemy" : "ZoneEnemyDefence";
    }

    public static string PlayerOneZoneTag = "PlayerOneZone";
    public static string PlayerTwoZoneTag = "PlayerTwoZone";
    public static string ObstacleTag = "Obstacle";
}
