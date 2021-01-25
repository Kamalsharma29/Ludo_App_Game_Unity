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


        // print(piece);
        if (piece.isAtHome()) {
            Vector3 target = new Vector3(-1, -1, -1);
            switch (piece.pieceType) {
                case PieceType.P1:
                    target = GameObject.Find("Slot_1:9").transform.position;
                    break;
                case PieceType.P2:
                    target = GameObject.Find("Slot_2:9").transform.position;
                    break;
                case PieceType.P3:
                    target = GameObject.Find("Slot_3:9").transform.position;
                    break;
                case PieceType.P4:
                    target = GameObject.Find("Slot_4:9").transform.position;
                    break;
            }
            piece.MoveForward(9);
            transform.Translate(target - transform.position);
        } else {
            var target = gameManager.MovePiece(piece, 10);
            transform.Translate(target - transform.position);
        }
    }

}