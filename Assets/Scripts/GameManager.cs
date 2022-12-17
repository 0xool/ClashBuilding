using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModel;

public class GameManager : MonoBehaviour
{
    private Player playerOne;
    private Player playerTwo;
    //============================================================================================================
    private void SetTestPLayers()
    { 
        playerOne.name = "Tom";
        playerTwo.name = "Jack";
    }
    //============================================================================================================
    // Start is called before the first frame update
    void Awake()
    {
        playerOne = new Player();
        playerTwo = new Player();
    }
    void Start()
    {
        SetTestPLayers();
    }
    // Update is called once per frame
    void Update()
    {
        if(playerOne.mainBaseHp == 0){
            // Player One Loses
        }

        if(playerTwo.mainBaseHp == 0){
            // Player Two Loses
        }
    }
}
