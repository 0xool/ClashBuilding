using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateInMenu() {
        LeanTween.moveX(this.GetComponent<RectTransform>(), 0 , 0.25f).setEaseLinear();
    }

    public void AnimateOutMenu() {
        LeanTween.moveX(this.GetComponent<RectTransform>(), -125 , 0.25f).setEaseLinear();
    }
}
