using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public enum TouchState {
        NORMAL,
        CONSTRUCTION,
        UNITSELECT,
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
    private Vector3 cameraPivotDirection;
    // Start is called before the first frame update
    void Awake() {
        touchState = TouchState.NORMAL;  
        cameraPivotDirection = (GameManager.instance.IsPlayerTwo()) ? new Vector3(0, 1, 1) : new Vector3(1, -1, 0);  
        cameraPivotDirection *= cameraMovmentSpeed; 
        Debug.Log(cameraPivotDirection);
    }
    void Start()
    {
        this.selectableMask =  LayerMask.GetMask("Unit");
        this.unitMenuHandler = GameObject.FindGameObjectWithTag("UnitMenuHandler").GetComponent<UnitMenuHandler>();
    }

    public void SetTouchState(TouchState touchState){
        this.touchState = touchState;
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
                        this.selectedUnit = hit.transform.gameObject.GetComponent<ISelectable>();
                        if(selectedUnit != null){
                            this.touchState = TouchState.UNITSELECT;
                            this.selectedUnit.SelectBuilding();
                            this.unitMenuHandler.AnimateInMenu();
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

            case TouchState.UNITSELECT:

                if(Input.GetMouseButtonDown(0)){
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 1000.0f, selectableMask))
                    {
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
                    Camera.main.transform.position -= cameraPivotDirection;
                }

                if(Screen.width - touchPos.x < widthMovmentLimit ){
                    // go right
                    Camera.main.transform.position += cameraPivotDirection;
                }

                if(touchPos.y < heighthMovmentLimit ){
                    // go up
                    Camera.main.transform.position -= cameraPivotDirection;
                }

                if(Screen.height - touchPos.y < heighthMovmentLimit ){
                    // go down
                    Camera.main.transform.position += cameraPivotDirection;
                }
                break;
            
            default:

                break;

        }


    }
}
