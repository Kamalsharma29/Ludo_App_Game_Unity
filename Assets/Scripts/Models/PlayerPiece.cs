using UnityEngine;

public class PlayerPiece {
    public PieceType pieceType;
    public int pieceId;
    public int[] location { get; private set; }

    public PlayerPiece(PieceType pieceType, int pieceId, int[] location) {
        this.pieceType = pieceType;
        this.pieceId = pieceId;
        this.location = location;
    }

    public void MoveForward(int num) {
        if (num + location[1] > 13)
            location[0] += 1;
        if (location[0] > 4)
            location[0] = 1;
        location[1] += num;
        if (location[1] > 13) 
            location[1] -= 13;
    }

    public void GoHome() {
        switch (pieceType) {
            case PieceType.P1:
            location[0] = 1;
            break;
            case PieceType.P2:
            location[0] = 2;
            break;
            case PieceType.P3:
            location[0] = 3;
            break;
            case PieceType.P4:
            location[0] = 4;
            break;
        }
        location[1] = 0;
    }

    public bool isAtHome() {
        return this.location[1] == 0;
    }
    public bool isRunComplete() {
        return this.location[1] == Constants.WINNING_ID;
    }

    public override string ToString() {
        return $"{this.pieceType.ToString()}-{this.pieceId} {this.location[0]}:{this.location[1]}";
    }
}