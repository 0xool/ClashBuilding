using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _canConstruct = true;
    public bool canConstruct{
        get{
            return _canConstruct;
        }
        set{
            _canConstruct = value;
            ChangeConstructionPlaneMaterial();
        }
    }
    private int collisionNumber = 0;
    public Material CanConstructMaterial;
    public Material CanNotConstructMaterial;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ChangeConstructionPlaneMaterial() {
        if(canConstruct){
            this.gameObject.GetComponent<MeshRenderer>().material = CanConstructMaterial;
        }else{
            this.gameObject.GetComponent<MeshRenderer>().material = CanNotConstructMaterial;
        }
    }

    public bool CanConstruct() {
        return canConstruct;
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider != this.gameObject){

            if(collider.CompareTag(Utilities.GetEnemyTag(this.transform.parent.tag)) || (collider.GetComponent<SpawnBehaviour>() != null)){
                collisionNumber++;
                canConstruct = false;
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if(collider != this.gameObject){
            if(collider.CompareTag(Utilities.GetEnemyTag(this.transform.parent.tag)) || (collider.GetComponent<SpawnBehaviour>() != null)){
                collisionNumber--;
                if(collisionNumber == 0) canConstruct = true;
            }
        }
    }
}
