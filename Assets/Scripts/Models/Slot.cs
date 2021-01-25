using System.Collections.Generic;
using UnityEngine;

public class Slot {
    // public int[] loc { get; private set; }
    public Vector3 loc;
    public bool isHomeStop;
    public bool isOtherStop;
    private bool isEnd;
    private List<PlayerPiece> playerPieceList;

    public int piecesCount => playerPieceList.Count;

    public Slot(Vector3 loc, bool isHomeStop = false, bool isOtherStop = false, bool isEnd = false) {
        this.loc = loc;
        this.playerPieceList = new List<PlayerPiece>();
    }

    public bool isStop() {
        return this.isHomeStop || this.isOtherStop;
    }

}