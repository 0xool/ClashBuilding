using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMenuHandler : MonoBehaviour
{
    public GameObject unitMenuPrefab;
    private TouchManager touchManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSelectUnitMenuItems(GameObject[] units){
        Transform contentPanel;
        GameObject menuItem;
        foreach (var unit in units)
        {
            contentPanel = this.transform.GetChild(0).GetChild(0);
            menuItem = Instantiate(unitMenuPrefab, this.transform.position, this.transform.rotation, contentPanel);
            menuItem.GetComponent<UnitMenuItem>().SetUnit(unit);
        }

            contentPanel = this.transform.GetChild(0).GetChild(0);
            menuItem = Instantiate(unitMenuPrefab, this.transform.position, this.transform.rotation, contentPanel);
            menuItem.GetComponent<UnitMenuItem>().SetUnit(null);
    }

    public void AnimateInMenu() {
        LeanTween.moveX(this.GetComponent<RectTransform>(), 0 , 0.25f).setEaseLinear();
    }

    public void AnimateOutMenu() {
        LeanTween.moveX(this.GetComponent<RectTransform>(), -125 , 0.25f).setEaseLinear();
    }

     

}
