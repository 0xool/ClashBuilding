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

    }

    public void GameOver(string player){
        if(player == "Player1"){
            Debug.Log("Game Over for Player1 and Player2 won.");
        }else if (player == "Player2"){
            Debug.Log("Game Over for Player2 and Player1 won.");
        }
    }
}
