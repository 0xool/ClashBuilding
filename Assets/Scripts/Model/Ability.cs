// Create ability class for casting spells in game

class Ability {
    public string name;
    public int damage;
    public int manaCost;
    public int cooldown;
    public int currentCooldown;
    public bool isReady;

    public Ability(string name, int damage, int manaCost, int cooldown) {
        this.name = name;
        this.damage = damage;
        this.manaCost = manaCost;
        this.cooldown = cooldown;
        this.currentCooldown = 0;
        this.isReady = true;
    }

    public void Update() {
        if (currentCooldown > 0) {
            currentCooldown--;
        } else {
            isReady = true;
        }
    }

    public void Cast(string playerTag) {
        if (isReady) {
            isReady = false;
            currentCooldown = cooldown;
        }
    }
}