using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitMenuItem : MonoBehaviour
{
    private GameObject unit;
    public GameObject unitCost;
    public GameObject unitName;
    public GameObject StopSpawn;
    private IUnit unitModel;
    public void OnUnitClicked() {
        TouchManager touchManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchManager>();
        touchManager.selectedUnit.SelectUnit(unit);
    }

    public void SetUnit(GameObject unit) {
        this.unit = unit;
        SetupUnitIcon();
    }

    public void SetupUnitIcon(){
        if(this.unit == null){
            StopSpawn.GetComponent<TMP_Text>().enabled = true;
            unitName.GetComponent<TMP_Text>().enabled = false;
            unitCost.GetComponent<TMP_Text>().enabled = false;
            this.GetComponent<Image>().color = new Color(241.0f/255.0f, 101.0f/255.0f, 101.0f/255.0f, 1.0f);
            return;
        }

        unitModel = unit.GetComponent<IUnit>();
        StopSpawn.GetComponent<TMP_Text>().enabled = false;
        unitCost.GetComponent<TMP_Text>().text = unitModel.GetCost().ToString();
        unitName.GetComponent<TMP_Text>().text = unit.name;

    }


}
