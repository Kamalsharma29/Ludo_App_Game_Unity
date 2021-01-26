using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        var move = Random.Range(1, 7);
        if (gameObject.name == "6Dice") move = 6;
        gameManager.DiceRolled(move);
    }

}