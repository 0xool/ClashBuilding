using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _inConstructZone = false;
    public bool inConstructZone{
        get{
            return _inConstructZone;
        }
        set{
            _inConstructZone = value;
            ChangeConstructionPlaneMaterial();
        }
    }
    public bool _canConstructWithoutCollision = true;
    public bool canConstructWithoutCollision{
        get{
            return _canConstructWithoutCollision;
        }
        set{
            _canConstructWithoutCollision = value;
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
        if(canConstructWithoutCollision && inConstructZone){
            this.gameObject.GetComponent<MeshRenderer>().material = CanConstructMaterial;
        }else{
            this.gameObject.GetComponent<MeshRenderer>().material = CanNotConstructMaterial;
        }
    }

    public bool CanConstruct() {
        return canConstructWithoutCollision && inConstructZone;
    }


    void OnTriggerEnter(Collider collider)
    {
        if(collider != this.gameObject){

            if(collider.CompareTag(Utilities.GetLeftFriendlyZoneTag()) || collider.CompareTag(Utilities.GetRightFriendlyZoneTag())){
                inConstructZone = true;
                return;
            }

            if(collider.CompareTag(Utilities.GetEnemyTag(this.transform.parent.tag)) || (collider.GetComponent<SpawnBehaviour>() != null)){
                collisionNumber++;
                canConstructWithoutCollision = false;
            }
        }
    }

    public void EnableConstructionMode() {
        EnableMeshComponents();
    }

    public void DisableConstructionMode() {
        DisableMeshComponents();
    }

    private void EnableMeshComponents() {
        this.gameObject.GetComponent<MeshCollider>().enabled = true;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void DisableMeshComponents(){ 
        this.gameObject.GetComponent<MeshCollider>().enabled = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerExit(Collider collider) {
        if(collider != this.gameObject){

            if(collider.CompareTag(Utilities.GetLeftFriendlyZoneTag()) || collider.CompareTag(Utilities.GetRightFriendlyZoneTag())){
                inConstructZone = false;
                return;
            }

            if(collider.CompareTag(Utilities.GetEnemyTag(this.transform.parent.tag)) || (collider.GetComponent<SpawnBehaviour>() != null)){
                collisionNumber--;
                if(collisionNumber == 0) canConstructWithoutCollision = true;
            }
        }
    }
}
