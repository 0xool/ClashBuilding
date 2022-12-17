

public class Building {

    public Building(string name, int hp, BuildingType bt, int damage) {
        this.name = name;
        this.hp = hp;
        this.buildingType = bt;

        this.damage = damage;
    }
    string name;
    int hp;
    int damage;
    BuildingType buildingType;
}