
public static class Utilities {
    public static string GetEnemyTag(string tag) {
        return (tag == "Player1") ? "Player2" : "Player1";
    }

    public static string GetLeftFriendlyZoneTag(BuildingType buildingType) {
        return (buildingType == BuildingType.SPAWNER) ? "ZoneLeftFriendly" : "ZoneLeftFriendlyDefence";
    }

        public static string GetRightFriendlyZoneTag(BuildingType buildingType) {
        return (buildingType == BuildingType.SPAWNER) ? "ZoneRightFriendly" : "ZoneRightFriendlyDefence";
    }
        public static string GetRightEnemyZoneTag(BuildingType buildingType) {
        return (buildingType == BuildingType.SPAWNER) ? "ZoneRightEnemy" : "";
    }

        public static string GetLeftEnemyZoneTag(BuildingType buildingType) {
        return (buildingType == BuildingType.SPAWNER) ? "ZoneLeftEnemy" : "";
    }
}
