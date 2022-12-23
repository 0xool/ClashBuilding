

public class Building {
    public Building(string name, int hp, BuildingType bt, int damage, int spawnCost, int constructionCost) {
        this.name = name;
        this.hp = hp;
        this.buildingType = bt;

        this.damage = damage;
        this.spawnCost = spawnCost;
        this.buildingState = BuildingState.IDLE;

        this.constructionCost = constructionCost;
    }

    public Building(string name, int hp, BuildingType bt, int damage, int constructionCost) {
        this.name = name;
        this.hp = hp;
        this.buildingType = bt;

        this.damage = damage;
        this.buildingState = BuildingState.IDLE;
        this.constructionCost = constructionCost;
    }
    
    public string name;
    public int hp;
    public int damage;
    public BuildingType buildingType;
    public BuildingState buildingState;
    public int spawnCost;
    public int constructionCost;
}