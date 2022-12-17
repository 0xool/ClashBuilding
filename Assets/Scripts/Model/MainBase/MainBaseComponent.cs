using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MainBaseComponent: MonoBehaviour {


    public int HP;
    public const string unitName = "MainBase";
    public int attackDamage = 100;
    public int attackRange = 100;
    public GameObject mainBaseGun;

    private List<GameObject> inRagneOffesiveUnitList;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        inRagneOffesiveUnitList = new List<GameObject>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
    }

    void AttackUnitInSight() {
        if(inRagneOffesiveUnitList.Count > 0){
            // Attack the very first unit in range until dead.
            var firstUnitInRange = inRagneOffesiveUnitList[0];
            
        }
    }

}
