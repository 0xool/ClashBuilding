using UnityEngine;

public interface IUnit {
    public int GetHP();
    public void AttackEnemy(GameObject enemy);
    public void InflictDamage(int bulletDamage);
    public int GetReloadTime();
    public void RemoveEnemyAsTarget(GameObject enemy);
}