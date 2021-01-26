using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class MoveFace : MonoBehaviour {
    private GameManager gameManager;
    private TMPro.TextMeshPro textComp;

    public int move;
    public bool selected;
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        move = gameManager.selectedMove;
        textComp = gameObject.GetComponent<TextMeshPro>();
        textComp.text = move.ToString();
    }

    void Update() {
        if (!selected) textComp.color = Color.red;

    }

    void OnMouseDown() {
        textComp.color = Color.green;
        selected = true;
        gameManager.MoveFaceSelected(move, gameObject);
    }
}
