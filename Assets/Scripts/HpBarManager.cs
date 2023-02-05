using UnityEngine;
using UnityEngine.UI;



public class HpBarManager : MonoBehaviour {
    private GameObject unit;
    private void Start() {
        if (GameManager.instance.IsPlayerOne()) {
            this.transform.eulerAngles = new Vector3(0, 45, 0);
        }
        if (GameManager.instance.IsPlayerTwo()) {
            this.transform.eulerAngles = new Vector3(0, -45, 0);
        }

    }

    public void SetUnit(GameObject unit) {
        this.unit = unit;
        this.transform.localScale = new Vector3(this.unit.transform.localScale.y, this.unit.transform.localScale.y, this.unit.transform.localScale.z) ;
        this.transform.GetComponent<RectTransform>().anchoredPosition3D -= new Vector3(this.unit.transform.localScale.y / 2, 0, this.unit.transform.localScale.y / 2);
    }


    private void Update() {
        if (unit != null) {
            this.transform.position = unit.transform.position;
            if (GameManager.instance.IsPlayerOne()){
                this.transform.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(1, 0, 1);
            }else{
                this.transform.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(-1, 0, 1);
            }
        }
    }

}