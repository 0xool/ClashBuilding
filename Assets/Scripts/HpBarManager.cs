using UnityEngine;
using UnityEngine.UI;



public class HpBarManager : MonoBehaviour {

    private void Start() {
        if (GameManager.instance.IsPlayerOne()) {
            this.transform.eulerAngles = new Vector3(0, 45, 0);
            this.transform.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(1, 0, 1);
        }
        if (GameManager.instance.IsPlayerTwo()) {
            this.transform.parent.transform.eulerAngles = new Vector3(0, -45, 0);
            this.transform.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(1, 0, 1);
        }
    }

}