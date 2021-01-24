using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTapped : MonoBehaviour {
    private GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        
        // if (Input.GetMouseButtonDown(0)) {
        // }
    }

    void OnMouseDown() {
        
            gameManager.lastDiceNum = Random.Range(1, 7);
            print(gameManager.lastDiceNum);
    }

}