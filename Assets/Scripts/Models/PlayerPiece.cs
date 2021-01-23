public class PlayerPiece {
    private PieceType pieceType;
    private int pieceId;
    public int[] location { get; private set; }

    public PlayerPiece(PieceType pieceType, int pieceId, int[] location) {
        this.pieceType = pieceType;
        this.pieceId = pieceId;
        this.location = location;
    }

    public bool isAtHome() {
        return this.location[1] == 0;
    }
    public bool isRunComplete() {
        return this.location[1] == Constants.WINNING_ID;
    }
}