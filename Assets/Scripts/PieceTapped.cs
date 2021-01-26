using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PieceTapped : MonoBehaviour {
    [SerializeField]
    public PlayerPiece piece;
    private GameManager gameManager;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        var digits = gameObject.name.Split('_')[1];
        var pt = (PieceType)int.Parse(digits[0].ToString());
        int id = int.Parse(digits[1].ToString());
        var loc = new int[2] { int.Parse(digits[0].ToString()), 0 };
        piece = new PlayerPiece(pt, id, loc);
    }



    void OnMouseDown() {

        gameManager.MovePiece(piece, transform);
    }

}