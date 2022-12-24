using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;


public class BuildingMenuItem : 
  MonoBehaviour, 
  IBeginDragHandler, 
  IDragHandler,
  IEndDragHandler
{
    private bool isBuilding = false;
    public GameObject buildPrefab;
    private GameObject build;
    private TouchManager touchManager;

    void Start() {
        this.touchManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TouchManager>();
    }

    void Update() {
        if(isBuilding){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask =  LayerMask.GetMask("MainPlane");

        if (Physics.Raycast(ray, out hit, 1000.0f, mask))
        {
            if(hit.transform.gameObject.tag == "MainPlane"){    
                this.build.transform.position = new Vector3(hit.point.x, hit.point.y + this.build.transform.localScale.y / 2, hit.point.z);
            }
        }
        }    
    }

     public void OnBeginDrag(PointerEventData data)
    {
        isBuilding = true;
        this.build = Instantiate(buildPrefab, new Vector3(-100, 0, -100), Quaternion.identity);            
        this.build.transform.position += new Vector3(0, this.build.transform.localScale.y / 2, 0);
        touchManager.SetTouchState(TouchManager.TouchState.CONSTRUCTION);
    }

    public void OnDrag(PointerEventData data)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IConstructable sb = this.build.GetComponent<IConstructable>();
        touchManager.SetTouchState(TouchManager.TouchState.NORMAL);
        sb.Build();
        isBuilding = false;
    }

}