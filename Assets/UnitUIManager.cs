using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIManager : MonoBehaviour
{
    public GameObject rightIcon;
    public GameObject leftIcon;
    public GameObject centerIcon;
    private GameObject unit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUnit(GameObject unit) {
        this.unit = unit;   
    }

    public void OnRightBtnClockRotate() {
        this.unit.transform.Rotate(new Vector3(0, 90, 0));
    }

    public void OnLeftBtnClockSell() {
        this.unit.GetComponentInParent<ISellable>().Sell();
    }
}
