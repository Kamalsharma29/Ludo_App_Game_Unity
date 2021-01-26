using UnityEngine;

public class PlayerPiece {
    public PieceType pieceType { get; private set; }
    public int pieceId { get; private set; }
    public int[] location { get; private set; }

    public PlayerPiece(PieceType pieceType, int pieceId, int[] location) {
        this.pieceType = pieceType;
        this.pieceId = pieceId;
        this.location = location;
    }

    public void MoveForward(int numToMove) {
        if (CanMoveToEndCol(numToMove)) {
            numToMove = numToMove - (8 - location[1]);
            location[1] = 100;
            // numToMove -= finalPosition;
        }

        if (IsAtEndColumn()) {
            if (location[1] + numToMove == Constants.WINNING_ID) { // win
                location[1] += numToMove;
            } else if (location[1] + numToMove < Constants.WINNING_ID) { // move
                location[1] += numToMove;
            } else { // location[1] + numToMove > 105 /// can't move
                // do nothing
            }
        } else {
            if (numToMove + location[1] > 13)
                location[0] += 1;
            if (location[0] > 4)
                location[0] = 1;
            location[1] += numToMove;
            if (location[1] > 13)
                location[1] -= 13;
        }
    }

    public void GoHome() {
        location[0] = (int)pieceType;
        location[1] = 0;
    }

    public bool CanMoveToEndCol(int numToMove) =>
        !IsAtHome() && location[0] == (int)pieceType && location[1] <= 7 && location[1] + numToMove > 7;

    public bool IsAtEndColumn() => location[1] >= 100 && location[1] <= 104;

    public bool IsAtHome() => this.location[1] == 0;

    public bool IsRunComplete() => this.location[1] == Constants.WINNING_ID;


    public override string ToString() =>
        $"{this.pieceType.ToString()}-{this.pieceId} {this.location[0]}:{this.location[1]}";

}