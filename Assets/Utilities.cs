using UnityEngine;
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
    public static int SellRatio = 2;
    public static string PlayerOneZoneTag = "PlayerOneZone";
    public static string PlayerTwoZoneTag = "PlayerTwoZone";
    public static string ObstacleTag = "Obstacle";
    private static string InGameMenuUiFilePath = "Prefab/UI/UnitInGameMenu";
    private static GameObject GetGameObjectFromFile(string path) {
        return (GameObject) Resources.Load(path);
    }
    public static GameObject GetInGameMenuUIGameObject() {
        return GetGameObjectFromFile(InGameMenuUiFilePath);
    }
}
