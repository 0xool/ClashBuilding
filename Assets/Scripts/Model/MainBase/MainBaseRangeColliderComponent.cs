using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class MainBaseColliderComponent: MonoBehaviour{

    public MainBaseComponent MainBase;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        MainBase = this.GetComponentInParent<MainBaseComponent>();
    }
}