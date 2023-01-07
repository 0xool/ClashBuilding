using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : SingletonBehaviour<TouchManager>
{
    public enum TouchState {
        NORMAL,
        CONSTRUCTION,
        UNITSELECTED,
    }
    private Vector3 touchPos;
    private Vector3 secondTouchPos;
    private Vector3 direction;
    private TouchState touchState;
    public float cameraMovmentSpeed = 0.1f;
    private RaycastHit hit;
    private Ray ray;
    private int selectableMask;
    public ISelectable selectedUnit;
    private UnitMenuHandler unitMenuHandler;
    private Vector3 cameraPivotDirectionVertical;
    private Vector3 cameraPivotDirectionHorizontal;
    private bool isUiBlocked = false;
    private GameObject cameraPivot;
    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        touchState = TouchState.NORMAL;  
        cameraPivotDirectionVertical = (GameManager.instance.IsPlayerTwo()) ? new Vector3(1, 0, 1) : new Vector3(-1, 0, -1);  
        cameraPivotDirectionVertical *= cameraMovmentSpeed;

        cameraPivotDirectionHorizontal = (GameManager.instance.IsPlayerTwo()) ? new Vector3(1, 0, -1) : new Vector3(-1, 0, 1);  
        cameraPivotDirectionHorizontal *= cameraMovmentSpeed; 

        cameraPivot = this.transform.parent.gameObject;
    }
    void Start()
    {
        this.selectableMask =  LayerMask.GetMask("Unit");
        this.unitMenuHandler = GameObject.FindGameObjectWithTag("UnitMenuHandler").GetComponent<UnitMenuHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (touchState)
        {
            case TouchState.NORMAL:
                if (Input.GetMouseButtonDown(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 1000.0f, selectableMask))
                    {
                        if(!hit.transform.gameObject.CompareTag(GameManager.instance.currentPlayer)) break;
                        this.selectedUnit = hit.transform.gameObject.GetComponent<ISelectable>();                        
                        if(selectedUnit != null){
                            this.touchState = TouchState.UNITSELECTED;
                            if (this.selectedUnit.SelectBuildingWithMenu()) this.unitMenuHandler.AnimateInMenu();
                            return;
                        }
                    }

                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                if(Input.GetMouseButton(0)){
                    direction = touchPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;
                }

                break;

            case TouchState.UNITSELECTED:

                if(Input.GetMouseButtonDown(0)){
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if(isUiBlocked) {
                        isUiBlocked = !isUiBlocked;
                        return;
                    }
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 1000.0f, selectableMask))
                    {
                        if(hit.transform.gameObject.layer == 5) break;
                        ISelectable selectableUnit = hit.transform.gameObject.GetComponent<ISelectable>();
                        if(selectableUnit == selectedUnit){
                            return;
                        }
                    }

                    this.touchState = TouchState.NORMAL;
                    this.selectedUnit.UnSelectBuilding();
                    foreach (Transform child in this.unitMenuHandler.transform.GetChild(0).GetChild(0)) {
                        GameObject.Destroy(child.gameObject);
                    }
                    this.selectedUnit = null;
                    this.unitMenuHandler.AnimateOutMenu();

                    return;
                }

                if(Input.GetMouseButton(0)){
                    direction = touchPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Camera.main.transform.position += direction;
                }

                break;

            case TouchState.CONSTRUCTION:
                float widthMovmentLimit = 0.2f * Screen.width;
                float heighthMovmentLimit = 0.2f * Screen.height;
                touchPos = Input.mousePosition;
                if(touchPos.x < widthMovmentLimit ){
                    // go left
                    cameraPivot.transform.position -= cameraPivotDirectionHorizontal;
                }

                if(Screen.width - touchPos.x < widthMovmentLimit ){
                    // go right
                    cameraPivot.transform.position += cameraPivotDirectionHorizontal;
                }

                if(touchPos.y < heighthMovmentLimit ){
                    // go up
                    cameraPivot.transform.position -= cameraPivotDirectionVertical;
                }

                if(Screen.height - touchPos.y < heighthMovmentLimit ){
                    // go down
                    cameraPivot.transform.position += cameraPivotDirectionVertical;
                }
                break;
            
            default:

                break;

        }
    }

    public void SetTouchState(TouchState touchState){
        this.touchState = touchState;
    }

    public void SetIsUIBlocked() {
        isUiBlocked = true;
    }
}
