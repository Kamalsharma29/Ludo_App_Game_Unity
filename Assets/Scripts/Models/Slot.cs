public class Slot {
    public int[] loc { get; private set; }
    private bool isHomeStop;
    private bool isOtherStop;
    private bool isEnd;
    // private List<PlayerPiece> playerPieceList;

    public Slot(int[] loc, bool isHomeStop = false, bool isOtherStop = false, bool isEnd = false) {
        this.loc = loc.Length != 2 ? new int[2] { -1, -1 } : loc;
    }

    public bool isStop() {
        return this.isHomeStop || this.isOtherStop;
    }

}