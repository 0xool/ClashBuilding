using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarterManager : MonoBehaviour
{
    private Building buidlingModel;
    public int hp = 500;
    // Start is called before the first frame update
    void Start()
    {
        buidlingModel = new Building(this.name, this.hp, BuildingType.HQ, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
