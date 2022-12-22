using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public enum TouchState {
        NORMAL,
        CONSTRUCTION,
    }
    private Vector3 touchPos;
    private Vector3 secondTouchPos;
    private Vector3 direction;
    private TouchState touchState;
    public float cameraMovmentSpeed = 0.1f;
    // Start is called before the first frame update
    void Awake() {
        touchState = TouchState.NORMAL;    
    }
    void Start()
    {

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
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                    Debug.Log("WTF");
                    Debug.Log(direction);
                    Debug.Log(Input.mousePosition);
                    direction = new Vector3(cameraMovmentSpeed, 0, 0);
                    Camera.main.transform.position -= direction;
                }

                if(Screen.width - touchPos.x < widthMovmentLimit ){
                    // go right
                    Camera.main.transform.position += new Vector3(cameraMovmentSpeed, 0, 0);
                }

                if(touchPos.y < heighthMovmentLimit ){
                    // go up
                    Camera.main.transform.position -= new Vector3(0, cameraMovmentSpeed, 0);
                }

                if(Screen.height - touchPos.y < heighthMovmentLimit ){
                    // go down
                    Camera.main.transform.position += new Vector3(0, cameraMovmentSpeed, 0);
                }
                break;
            
            default:

                break;

        }


    }
}
