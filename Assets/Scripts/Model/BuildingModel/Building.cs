

public class Building {

    public Building(string name, int hp, BuildingType bt, int damage, int spawnCost) {
        this.name = name;
        this.hp = hp;
        this.buildingType = bt;

        this.damage = damage;
        this.spawnCost = spawnCost;
    }

    public Building(string name, int hp, BuildingType bt, int damage) {
        this.name = name;
        this.hp = hp;
        this.buildingType = bt;

        this.damage = damage;
        
    }
    
    public string name;
    public int hp;
    public int damage;
    public BuildingType buildingType;
    public int spawnCost;
}