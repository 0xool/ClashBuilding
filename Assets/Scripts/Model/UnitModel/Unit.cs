using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Unit
{
    public Unit(string unitName, int movementSpeed, int hp, UnitState unitState, ZONE zone) {
        this.unitName = unitName;
        this.movementSpeed = movementSpeed;
        this.hp = hp;

        this.unitState = unitState;
        this.zone = zone;
    }

    public string unitName;
    public UnitState unitState = UnitState.MOVING;
    public int movementSpeed;
    public int hp;
    public ZONE zone;
}