using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RemoveFromTarget(GameObject target);
public interface IUnit {
    public int GetHP();
    public int GetCost();
    public void AttackEnemy(GameObject enemy);
    public void InflictDamage(int bulletDamage);
    public int GetReloadTime();
    public void RemoveEnemyAsTarget(GameObject enemy);
    public void IsBeingDestroyed();

    public void AppendRemoveTargetDelegation( RemoveFromTarget removeFromTarget);
}