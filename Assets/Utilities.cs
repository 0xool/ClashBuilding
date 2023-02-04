using System;
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
    private static string PrefabPath = "Prefab/";
    private static string UIPrefabPath = "Prefab/UI/";
    public static int SellRatio = 2;
    public static string PlayerOneZoneTag = "PlayerOneZone";
    public static string PlayerTwoZoneTag = "PlayerTwoZone";
    public static string ObstacleTag = "Obstacle";
    private static string InGameMenuUiFilePath = UIPrefabPath + "UnitInGameMenu";
    private static string InHpBarFilePath = UIPrefabPath + "HpBarCanvas";    
    private static Func<string, string> GetConstructionGameObjectPath = (constructionName) => {return PrefabPath + constructionName;};
    private static Func<string, string> GetUIGameObjectPath = (constructionName) => {return PrefabPath + constructionName;};

    private static GameObject GetGameObjectFromFile(string path) {
        return (GameObject) Resources.Load(path);
    }
    public static GameObject GetInGameMenuUIGameObject() {
        return GetGameObjectFromFile(InGameMenuUiFilePath);
    }
    public static GameObject GetHpBarUIGameObject() {
        return GetGameObjectFromFile(InHpBarFilePath);
    }
    public static GameObject GetConstructionGameObject(string constructionName){
        return GetGameObjectFromFile(GetConstructionGameObjectPath(constructionName));
    }

    public static GameObject GetUnitGameObject(string unitName){
        return GetGameObjectFromFile(GetConstructionGameObjectPath(unitName));
    }
}
