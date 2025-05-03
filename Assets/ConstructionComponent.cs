using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionComponent : MonoBehaviour
{
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

    public bool _inPlayerZone = false;
    public bool inPlayerZone{
        get{
            return _inPlayerZone;
        }
        set{
            _inPlayerZone = value;
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
    [SerializeField]
    private bool buildingConstructed = false;
    private int collisionNumber = 0;
    private int refineryContact = 0;
    public Material CanConstructMaterial;
    public Material CanNotConstructMaterial;
    private BuildingType buildingType;
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = CanNotConstructMaterial;
        buildingType = this.transform.parent.GetComponent<IConstructable>().GetBuildingType();
    }

    private void ChangeConstructionPlaneMaterial() {
        this.gameObject.GetComponent<MeshRenderer>().material = CanConstruct() ? CanConstructMaterial : CanNotConstructMaterial;
    }

    public bool CanConstruct() {
        return canConstructWithoutCollision && inConstructZone && inPlayerZone && !buildingConstructed;
    }

    public void EnableConstructionMode() {
        if (buildingConstructed){
            DisableConstructionMode();
            return;
        }

        EnableMeshComponents();
    }

    public void DisableConstructionMode() {
        DisableMeshComponents();
        buildingConstructed = true;
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

            if(GameManager.instance.IsPlayerOne()){
                if(collider.tag == Utilities.PlayerOneZoneTag) inPlayerZone = false;
            }

            if(GameManager.instance.IsPlayerTwo()){
                if(collider.tag == Utilities.PlayerTwoZoneTag) inPlayerZone = false;
            }

            if(this.buildingType == BuildingType.REFINERY){
                if(collider.CompareTag("Resource")){
                    refineryContact--;
                    if(refineryContact == 0)
                        inConstructZone = false;
                }
                return;
            }
            if(collider.CompareTag(Utilities.GetLeftZoneTag(this.buildingType)) || collider.CompareTag(Utilities.GetRightZoneTag(this.buildingType))){
                inConstructZone = false;
                return;
            }

            if(collider.CompareTag(GameManager.instance.GetEnemyTagString()) || (collider.GetComponent<SpawnBehaviour>() != null) || collider.CompareTag(Utilities.ObstacleTag)){
                collisionNumber--;
                if(collisionNumber == 0) canConstructWithoutCollision = true;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider != this.gameObject){

            if(GameManager.instance.IsPlayerOne()){
                if(collider.tag == Utilities.PlayerOneZoneTag) inPlayerZone = true;
            }

            if(GameManager.instance.IsPlayerTwo()){
                if(collider.tag == Utilities.PlayerTwoZoneTag) inPlayerZone = true;
            }

            if(this.buildingType == BuildingType.REFINERY){
                if(collider.CompareTag("Resource")){
                    refineryContact++;
                    inConstructZone = true;
                }
                return;
            }

            if(collider.CompareTag(Utilities.GetLeftZoneTag(this.buildingType)) || collider.CompareTag(Utilities.GetRightZoneTag(this.buildingType))){
                inConstructZone = true;
                return;
            }

            if(collider.CompareTag(GameManager.instance.GetEnemyTagString()) || (collider.GetComponent<SpawnBehaviour>() != null) || collider.CompareTag(Utilities.ObstacleTag)){
                collisionNumber++;
                canConstructWithoutCollision = false;
            }
        }
    }
}
