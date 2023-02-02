

public class Building {
    public Building(BuildingType bt, int damage, int spawnCost, int constructionCost) {
        this.buildingType = bt;

        this.damage = damage;
        this.spawnCost = spawnCost;
        this.buildingState = BuildingState.IDLE;

        this.constructionCost = constructionCost;
    }

    public Building(BuildingType bt, int damage, int constructionCost) {        
        this.buildingType = bt;

        this.damage = damage;
        this.buildingState = BuildingState.IDLE;
        this.constructionCost = constructionCost;
    }
    public int damage;
    public BuildingType buildingType;
    public BuildingState buildingState;
    public int spawnCost;
    public int constructionCost;
    public int upgradeCost;
}