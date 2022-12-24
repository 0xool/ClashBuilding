using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    public GameObject menuPrefab;
    private List<MenuBuildingModel> menuItems;
    void Awake() {
        menuItems = new List<MenuBuildingModel>();
        MenuBuildingModel infantryBuilding = new MenuBuildingModel("Infantry Building", 700);
        MenuBuildingModel turrentBuilding = new MenuBuildingModel("Turrent", 400);
        menuItems.Add(infantryBuilding);     
        menuItems.Add(turrentBuilding);
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in menuItems)
        {        
            BuildingMenuItem menuItem = Instantiate(menuPrefab, Vector3.zero, this.transform.rotation, this.transform).GetComponent<BuildingMenuItem>();
            menuItem.GetComponentInChildren<TMP_Text>().text = item.cost.ToString();
            menuItem.buildPrefab = item.GetPrefab();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class MenuBuildingModel{
    public string name;
    public int cost;
    private GameObject prefab;


    public MenuBuildingModel(string name, int cost) {
        this.name = name;
        this.prefab = (GameObject) Resources.Load("Prefab/" + name);
        this.cost = cost;  
    }

    public GameObject GetPrefab() {
        return this.prefab;
    }
}
