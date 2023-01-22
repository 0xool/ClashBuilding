using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIManager : MonoBehaviour
{
    public GameObject rightIcon;
    public GameObject leftIcon;
    public GameObject centerIcon;
    private GameObject unit;
    private bool isUpgradeable = false;

    void Start()
    {        
        if(GameManager.instance.IsPlayerOne()) this.transform.parent.transform.eulerAngles = new Vector3(0, 180, 0);
        if(GameManager.instance.IsPlayerTwo()) this.transform.parent.transform.eulerAngles = new Vector3(0, 90, 0);
    }

    public void SetUnit(GameObject unit) {
        this.unit = unit;
        this.isUpgradeable = this.unit.GetComponent<IUpgradeable>() != null;
        if(!this.isUpgradeable){
            HideMiddleBtn();
        }   
    }

    public void OnRightBtnClockRotate() {
        TouchManager.instance.SetIsUIBlocked();
        this.unit.transform.Rotate(new Vector3(0, 90, 0));
    }

    public void OnLeftBtnClockSell() {
        this.unit.GetComponentInParent<ISellable>().Sell();
    }

    public void RemoveUI() {
        Destroy(this.transform.parent.gameObject);
    }

    public void OnMiddleBtnUpgradeClicked() {
        if(!isUpgradeable) return;
        this.unit.GetComponent<IUpgradeable>().Upgrade();
    }

    private void HideMiddleBtn() {
        Destroy(centerIcon);
    }
}
