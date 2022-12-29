using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Unit
{
    public Unit(string unitName, float movementSpeed, int hp, UnitState unitState, ZONE zone, int damage, int cost) {
        this.unitName = unitName;
        this.movementSpeed = movementSpeed;
        this.hp = hp;

        this.unitState = unitState;
        this.zone = zone;
        this.damage = damage;
        
        this.cost = cost;
    }

    public int damage;
    public string unitName;
    public UnitState unitState = UnitState.MOVING;
    public float movementSpeed;
    public int hp;
    public ZONE zone;
    public int cost;
}