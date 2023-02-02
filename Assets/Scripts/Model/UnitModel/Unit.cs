using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Unit
{
    public Unit(float movementSpeed, UnitState unitState, ZONE zone, int damage, int cost) {
        this.movementSpeed = movementSpeed;

        this.unitState = unitState;
        this.zone = zone;
        this.damage = damage;
        
        this.cost = cost;
    }

    public int damage;
    public UnitState unitState = UnitState.MOVING;
    public float movementSpeed;
    public ZONE zone;
    public int cost;
}

