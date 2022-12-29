using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMenuItem : MonoBehaviour
{
    private GameObject unit;
    public void OnUnitClicked() {
        TouchManager touchManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchManager>();
        touchManager.selectedUnit.SelectUnit(unit);
    }

    public void SetUnit(GameObject unit) {
        this.unit = unit;
    }


}
