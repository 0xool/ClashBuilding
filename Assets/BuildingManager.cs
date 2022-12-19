using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject unit;
    public float spawnInterval = 5.0f;
    private float timer = 0;
    public float offsetSpawn = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetHP() {
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > spawnInterval){
            var newUnit = Instantiate(unit, new Vector3(this.transform.position.x - offsetSpawn,this.transform.position.y - (this.transform.localScale.x/2 - unit.transform.localScale.x/2),this.transform.position.z), this.transform.rotation);
            newUnit.tag = this.gameObject.tag;
            timer = 0;
        }else{
            timer += Time.deltaTime;
        }
    }
}
