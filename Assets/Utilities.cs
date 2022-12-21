
public static class Utilities {
    public static string GetEnemyTag(string tag) {
        return (tag == "Player1") ? "Player2" : "Player1";
    }

    public static string GetLeftFriendlyZoneTag() {
        return "ZoneLeftFriendly";
    }

        public static string GetRightFriendlyZoneTag() {
        return "ZoneRightFriendly";
    }
        public static string GetRightEnemyZoneTag() {
        return "ZoneRightEnemy";
    }

        public static string GetLeftEnemyZoneTag() {
        return "ZoneLeftEnemy";
    }
}
