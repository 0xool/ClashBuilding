using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;


public class DragUIItem : 
  MonoBehaviour, 
  IBeginDragHandler, 
  IDragHandler,
  IEndDragHandler
{
    private bool isBuilding = false;
    public GameObject buildPrefab;
    private GameObject build;

    void Update() {
        if(isBuilding){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask =  LayerMask.GetMask("MainPlane");

        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            if(hit.transform.gameObject.tag == "MainPlane"){    
                Debug.Log(hit.point);        
                this.build.transform.position = new Vector3(hit.point.x, hit.point.y + this.build.transform.localScale.y / 2, hit.point.z);
            }
        }
        }    
    }

     public void OnBeginDrag(PointerEventData data)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(
        Input.mousePosition);
        isBuilding = true;
        int mask =  LayerMask.GetMask("MainPlane");

        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            Debug.Log("WTF");
            if(hit.transform.gameObject.tag == "MainPlane"){
                Vector3 worldPoint = hit.point;
                this.build = Instantiate(buildPrefab, worldPoint, Quaternion.identity);            
                this.build.transform.position += new Vector3(0, this.build.transform.localScale.y / 2, 0);
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SpawnBehaviour sb = this.build.GetComponent<SpawnBehaviour>();
        sb.Build();
        isBuilding = false;
    }

}