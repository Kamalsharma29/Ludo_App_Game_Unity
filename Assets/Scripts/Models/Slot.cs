using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slot {
    // public int[] loc { get; private set; }
    public Vector3 loc;
    public string name { get; private set; }
    public bool isHomeStop;
    public bool isOtherStop;
    private bool isEnd;
    private List<PlayerPiece> playerPieceList;
    public Slot(Vector3 loc, string name, bool isHomeStop = false, bool isOtherStop = false, bool isEnd = false) {
        this.loc = loc;
        this.name = name;
        this.isHomeStop = isHomeStop;
        this.isOtherStop = isOtherStop;
        this.isEnd = isEnd;
        this.playerPieceList = new List<PlayerPiece>();
    }

    public void AddPiece(PlayerPiece pp) {
        playerPieceList.Add(pp);
    }
    public void RemovePiece(PlayerPiece pp) {
        playerPieceList.Remove(pp);
    }

    // public int GetForeignIndex(PieceType pt) {
    //     for (int i = 0; i < playerPieceList.Count; i++) {
    //         if (playerPieceList[i].pieceType != pt) {
    //             return i;
    //         }
    //     }

    //     return -1;
    // }

    public PlayerPiece GetForeignPresent(PieceType pt) {
        for (int i = 0; i < playerPieceList.Count; i++) {
            if (playerPieceList[i].pieceType != pt) {
                return playerPieceList[i];
            }
        }
        return null;
    }

    public int piecesCount => playerPieceList.Count;


    public bool isStop() {
        return this.isHomeStop || this.isOtherStop;
    }

}