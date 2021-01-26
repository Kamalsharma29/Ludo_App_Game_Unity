using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour {
    public GameObject slotPrefab;
    public GameObject moveFacePrefab;
    public GameObject playerPiecePrefab;
    public List<GameObject> moveFacesList;
    public List<int> movesList;
    public List<PieceType> pieceTurn;
    public List<Slot> slotList;
    public List<Slot> endSlotList;
    public List<PlayerPiece> playerPieces;
    public List<PlayerPiece> finishedPieces;
    public List<Vector3> winnerLocation;
    private List<Vector3> homePosition;
    private int numOfPlayers = 4;
    public int selectedMove;
    public int selectedMoveFaceGameObjectIndex;
    public int[,] imaginaryPath;

    void Start() {
        pieceTurn = new List<PieceType>();
        for (int i = 1; i <= numOfPlayers; i++) {
            pieceTurn.Add((PieceType)i);
        }
        slotList = new List<Slot>();
        endSlotList = new List<Slot>();
        homePosition = new List<Vector3>();
        finishedPieces = new List<PlayerPiece>();
        moveFacesList = new List<GameObject>();
        InitializeWinnerLocation();

        InitializeInThisOrder();


    }

    void Update() {


    }

    public void DiceRolled(int move) {
        selectedMove = move;
        movesList.Add(move);

        var moveFacePos = new Vector3(-80, 0, -65 + (movesList.Count - 1) * 10);
        var parent = GameObject.Find("MoveFaces").transform;
        var newMoveFace = Instantiate(moveFacePrefab, moveFacePos, Quaternion.AngleAxis(90, Vector3.right), parent);
        newMoveFace.name = $"MoveFace_{move}";
        moveFacesList.Add(newMoveFace);
    }

    public void MoveFaceSelected(int move, GameObject go) {
        if (selectedMoveFaceGameObjectIndex > -1)
            moveFacesList[selectedMoveFaceGameObjectIndex].GetComponent<MoveFace>().selected = false;
        selectedMove = move;
        selectedMoveFaceGameObjectIndex = moveFacesList.IndexOf(go);
    }

    public void AfterMoveDone(int move) {
        // remove from movesList
        movesList.Remove(move);

        // move all pieces down by z
        for (int i = selectedMoveFaceGameObjectIndex + 1; i < moveFacesList.Count; i++) {
            var cur = moveFacesList[i];
            var moveDistanceZ = -10;
            // cur.transform.Translate(new Vector3(0, 0, moveDistanceZ));
            cur.transform.position = 
                new Vector3(cur.transform.position.x, 
                    cur.transform.position.y, 
                    cur.transform.position.z + moveDistanceZ);
        }
        // destroy gameobject
        Destroy(moveFacesList[selectedMoveFaceGameObjectIndex]);
        // remove gameobject from list
        moveFacesList.RemoveAt(selectedMoveFaceGameObjectIndex);
        // select new move from bottom
        selectedMoveFaceGameObjectIndex = movesList.Count() - 1;
        // reset selectedMove
        if (movesList.Count > 0)
            selectedMove = moveFacesList[selectedMoveFaceGameObjectIndex].GetComponent<MoveFace>().move;
    }

    void InitializeWinnerLocation() {
        var y = 10f;
        var space = 6;
        winnerLocation = new List<Vector3>(){
            new Vector3(-space, y, -10),  new Vector3(0, y, -10),  new Vector3(space, y, -10),  new Vector3(0, y + 5, -6),
            new Vector3(-10, y, space),  new Vector3(-10, y, 0),  new Vector3(-10, y, -space),  new Vector3(-6, y + 5, 0),
            new Vector3(space, y, 10),  new Vector3(0, y, 10),  new Vector3(-space, y, 10),  new Vector3(0, y + 5, 6),
            new Vector3(10, y, space),  new Vector3(10, y, 0),  new Vector3(10, y, -space),  new Vector3(6, y + 5, 0),
        };
    }

    void InitializeInThisOrder() {

        InitializePath();
        CreateAllPathSlots();
        CreatePlayerPieces();

    }

    public void MovePiece(PlayerPiece pp, Transform tr) {
        if (pp.IsRunComplete()) return;


        int slotIndex = GetSlotIndex(pp.location);
        if (pp.IsAtHome()) {
            if (selectedMove != 6) return;
            Vector3 target = new Vector3(-1, -1, -1);
            switch (pp.pieceType) {
                case PieceType.P1:
                    target = GameObject.Find("Slot_1:9").transform.position;
                    slotIndex = 8;
                    break;
                case PieceType.P2:
                    target = GameObject.Find("Slot_2:9").transform.position;
                    slotIndex = 21;
                    break;
                case PieceType.P3:
                    target = GameObject.Find("Slot_3:9").transform.position;
                    slotIndex = 34;
                    break;
                case PieceType.P4:
                    target = GameObject.Find("Slot_4:9").transform.position;
                    slotIndex = 47;
                    break;
            }
            var initialImaginaryLoc = 9; // Dont' change the 9. This is the initial imaginary loc.
            pp.MoveForward(initialImaginaryLoc);
            slotList[slotIndex].AddPiece(pp);
            tr.Translate(target - tr.position);
        } else if (pp.IsAtEndColumn()) {
            // slotIndex == -1
            var curPos = pp.location[1];
            if (curPos + selectedMove == Constants.WINNING_ID) { // win
                var startingIndex = ((int)pp.pieceType - 1) * 5;
                var finalIndex = startingIndex + curPos - 100;
                endSlotList[finalIndex].RemovePiece(pp);
                curPos += selectedMove;
                finalIndex = startingIndex + curPos - 100;

                pp.MoveForward(selectedMove);
                startingIndex = ((int)pp.pieceType - 1) * 4;
                finalIndex = startingIndex + finishedPieces.Where(x => x.pieceType == pp.pieceType).Count();
                var target = winnerLocation[finalIndex];
                finishedPieces.Add(pp);
                tr.Translate(target - tr.position);

            } else if (curPos + selectedMove < Constants.WINNING_ID) { // move
                var startingIndex = ((int)pp.pieceType - 1) * 5;
                var finalIndex = startingIndex + curPos - 100;
                endSlotList[finalIndex].RemovePiece(pp);
                curPos += selectedMove;
                finalIndex = startingIndex + curPos - 100;

                pp.MoveForward(selectedMove);
                endSlotList[finalIndex].AddPiece(pp);
                var target = endSlotList[finalIndex].loc;
                tr.Translate(target - tr.position);
            } else { // indexLoc + selectedMove > Constants.WINNING_ID /// do nothing

            }
        } else if (pp.CanMoveToEndCol(selectedMove)) { // cant win on 6 from here
            // remove old piece
            GetSlot(slotIndex).RemovePiece(pp);

            var curPos = pp.location[1];
            var startingIndex = ((int)pp.pieceType - 1) * 5;


            var endLocation = selectedMove - (8 - curPos);
            var finalIndex = startingIndex + endLocation;

            pp.MoveForward(selectedMove);
            if (curPos == 7 && endLocation == 5) {
                startingIndex = ((int)pp.pieceType - 1) * 4;
                finalIndex = startingIndex + finishedPieces.Where(x => x.pieceType == pp.pieceType).Count();
                var target = winnerLocation[finalIndex];
                finishedPieces.Add(pp);
                tr.Translate(target - tr.position);
            } else {
                endSlotList[finalIndex].AddPiece(pp);
                var target = endSlotList[finalIndex].loc;
                tr.Translate(target - tr.position);
            }

        } else {
            GetSlot(slotIndex).RemovePiece(pp);
            slotIndex += selectedMove;
            if (slotIndex > 51) {
                slotIndex -= 52;
            }

            pp.MoveForward(selectedMove);
            GetSlot(slotIndex).AddPiece(pp);
            var target = GetSlot(slotIndex).loc;
            tr.Translate(target - tr.position);

            // check to kill piece
            if (!GetSlot(slotIndex).isStop()) {
                var presentPiece = GetSlot(slotIndex).GetForeignPresent(pp.pieceType);
                if (presentPiece != null) {
                    KillPiece(presentPiece, slotIndex);
                }
            }
        }
        AfterMoveDone(selectedMove);

    }

    public void KillPiece(PlayerPiece pp, int index) {
        // remove from list
        GetSlot(index).RemovePiece(pp);
        // set location to x:0
        pp.GoHome();
        // get transform
        var tr = GameObject.Find($"Piece_{(int)pp.pieceType}{pp.pieceId}").transform;

        // find where the home position is
        var homePositionIndex = GetHomePositionIndex(pp);
        var target = homePosition[homePositionIndex];
        // move piece home
        tr.Translate(target - tr.position);
    }

    // For initial pieces at home with loc = 0
    public int GetHomePositionIndex(PlayerPiece pp) {
        var homePositionIndex = ((int)pp.pieceType - 1) * 4;
        homePositionIndex += pp.pieceId - 1;
        return homePositionIndex;
    }

    public Slot GetSlot(int index) {
        return slotList[index];
    }

    void printPath() {
        for (int i = 0; i < imaginaryPath.GetLength(0); i++) {
            print($"{imaginaryPath[i, 0]}:{imaginaryPath[i, 1]}");
        }
    }

    public int GetSlotIndex(int[] location) {
        var curLoc = location;
        for (int i = 0; i < imaginaryPath.GetLength(0); i++) {
            if (imaginaryPath[i, 0] == curLoc[0]) {
                if (imaginaryPath[i, 1] == curLoc[1]) {
                    return i;
                }
            }
        }
        return -1;
    }

    void InitializePath() {
        imaginaryPath = new int[Constants.MAX_PATH, 2];
        for (int i = 1; i <= 4; i++) {
            for (int j = 1; j <= imaginaryPath.GetLength(0) / 4; j++) {
                var index = (i - 1) * imaginaryPath.GetLength(0) / 4 + j - 1; // 0-51
                imaginaryPath[index, 0] = i;
                imaginaryPath[index, 1] = j;
            }
        }


    }

    void CreatePlayerPieces() {

        Material darkGreen = Resources.Load("Materials/GreenDarkMat", typeof(Material)) as Material;
        Material darkBlue = Resources.Load("Materials/BlueDarkMat", typeof(Material)) as Material;
        Material darkRed = Resources.Load("Materials/RedDarkMat", typeof(Material)) as Material;
        Material darkYellow = Resources.Load("Materials/YellowDarkMat", typeof(Material)) as Material;

        float x, y, z;
        y = 2f;
        string pieceTxt = "Piece_";
        Vector3 vec;

        x = -30;
        z = -60;
        for (int i = 1; i <= 4; i++, x -= 10) {
            vec = new Vector3(x, y, z);
            homePosition.Add(vec);
            CreatePlayerPiece($"{pieceTxt}1{i}", vec, darkGreen);
        }

        x = -60;
        z = 30;
        for (int i = 1; i <= 4; i++, z += 10) {
            vec = new Vector3(x, y, z);
            homePosition.Add(vec);
            CreatePlayerPiece($"{pieceTxt}2{i}", vec, darkBlue);
        }

        x = 30;
        z = 60;
        for (int i = 1; i <= 4; i++, x += 10) {
            vec = new Vector3(x, y, z);
            homePosition.Add(vec);
            CreatePlayerPiece($"{pieceTxt}3{i}", vec, darkRed);
        }

        x = 60;
        z = -30;
        for (int i = 1; i <= 4; i++, z -= 10) {
            vec = new Vector3(x, y, z);
            homePosition.Add(vec);
            CreatePlayerPiece($"{pieceTxt}4{i}", vec, darkYellow);
        }

    }

    private void CreatePlayerPiece(string name, Vector3 pos, Material mat) {
        Transform parent = GameObject.Find("Pieces").transform;
        GameObject newPiece = Instantiate(playerPiecePrefab, pos, transform.rotation, parent);
        newPiece.GetComponent<Renderer>().material = mat;
        newPiece.name = name;
        newPiece.AddComponent<PieceTapped>();
    }


    void CreateSlot(int index, Vector3 pos, Material mat = null, bool isHomeStop = false, bool isOtherStop = false) {
        var slotText = "Slot_";
        Transform parent = GameObject.Find("Slots").transform;
        GameObject newSlotObject = Instantiate(slotPrefab, pos, transform.rotation, parent);
        if (mat != null)
            newSlotObject.GetComponent<Renderer>().material = mat;
        if (index < 100) {
            slotText += $"{imaginaryPath[index, 0]}:{imaginaryPath[index, 1]}";
            Slot normalSlot = new Slot(pos, slotText, isHomeStop, isOtherStop);
            slotList.Add(normalSlot);
        } else {
            // index = 304
            var second = index % 100; // 4
            var id = (index - second) / 100; // 3
            slotText += $"{id}:{second + 100}";
            Slot endSlot = new Slot(pos, slotText, false, false, isEnd: true);
            endSlotList.Add(endSlot);
        }
        newSlotObject.name = slotText;
    }

    void CreateAllPathSlots() {
        int index = 0;
        CreatePath1Slots(ref index);
        CreatePath2Slots(ref index);
        CreatePath3Slots(ref index);
        CreatePath4Slots(ref index);
    }

    void CreatePath1Slots(ref int index) {
        Material green = Resources.Load("Materials/GreenMat", typeof(Material)) as Material;
        float x = -1, y = -1, z = -1;
        y = 0.5f;

        // for (int pathBlock = 1; pathBlock <= 4; pathBlock++) {
        //     Material mat = Resources.Load("Materials/DefaultMat", typeof(Material)) as Material;

        //     switch (pathBlock) {
        //         case 1:
        //             x = -10;
        //             z = -20f;
        //             mat = Resources.Load("Materials/GreenMat", typeof(Material)) as Material;
        //             break;
        //         case 2:
        //             x = -20f;
        //             z = 10f;
        //             mat = Resources.Load("Materials/BlueMat", typeof(Material)) as Material;
        //             break;
        //         case 3:
        //             x = -10f;
        //             z = 20f;
        //             mat = Resources.Load("Materials/RedMat", typeof(Material)) as Material;
        //             break;

        //         case 4:
        //             x = 20f;
        //             z = 10f;
        //             mat = Resources.Load("Materials/YellowMat", typeof(Material)) as Material;
        //             break;
        //         default:
        //             break;
        //     }

        // Right Column
        x = 10f;
        y = 0.5f;
        z = -20f;
        for (int i = 0; i < 6; i++) {
            float newX = x, newZ = z - 10 * i;
            // Slot slot = new Slot(new int[] { (int)newX, (int)newZ });
            if (i == 3) {
                // slot.isOtherStop = true;
                CreateSlot(index, new Vector3(x, y, z - 10 * i), green, isOtherStop: true);
            } else {
                CreateSlot(index, new Vector3(x, y, z - 10 * i));
            }
            ++index;
        }
        // Middle Column
        x = 0f;
        y = 0.5f;
        z = -20f;
        CreateSlot(index, new Vector3(x, y, z - 50));
        var middleIndex = 100;
        for (int i = 4; i >= 0; i--) {
            float newX = x, newZ = z - 10 * i;
            // Slot slot = new Slot(new int[] { (int)newX, (int)newZ });
            CreateSlot(middleIndex++, new Vector3(x, y, z - 10 * i), green);
        }
        ++index;

        // Left column
        x = -10;
        z = -20f;
        for (int i = 5; i >= 0; i--) {
            float newX = x, newZ = z - 10 * i;
            // Slot slot = new Slot(new int[] { (int)newX, (int)newZ });
            if (i == 4) {
                // slot.isHomeStop = true;
                CreateSlot(index, new Vector3(newX, y, newZ), green, isHomeStop: true);
            } else {
                CreateSlot(index, new Vector3(newX, y, newZ));
            }
            ++index;
        }


    }

    void CreatePath2Slots(ref int index) {
        Material blue = Resources.Load("Materials/BlueMat", typeof(Material)) as Material;
        float x, y, z;
        y = 0.5f;



        // Right Column
        x = -20f;
        z = -10f;
        for (int i = 0; i < 6; i++) {
            if (i == 3) {
                CreateSlot(index, new Vector3(x - 10 * i, y, z), blue, isOtherStop: true);
            } else {
                CreateSlot(index, new Vector3(x - 10 * i, y, z));
            }
            ++index;
        }
        // Middle Column
        x = -20f;
        z = 0f;
        CreateSlot(index, new Vector3(x - 50, y, z));
        var middleIndex = 200;
        for (int i = 4; i >= 0; i--) {
            CreateSlot(middleIndex++, new Vector3(x - 10 * i, y, z), blue);
        }
        ++index;

        // Left column
        x = -20f;
        z = 10f;
        for (int i = 5; i >= 0; i--) {
            if (i == 4) {
                CreateSlot(index, new Vector3(x - 10 * i, y, z), blue, isHomeStop: true);
            } else {
                CreateSlot(index, new Vector3(x - 10 * i, y, z));
            }
            ++index;
        }
    }

    void CreatePath3Slots(ref int index) {
        Material red = Resources.Load("Materials/RedMat", typeof(Material)) as Material;
        float x, y, z;
        y = 0.5f;

        // Left column (inverted)
        x = -10f;
        z = 20f;
        for (int i = 0; i < 6; i++) {
            if (i == 3) {
                CreateSlot(index, new Vector3(x, y, z + 10 * i), red, isOtherStop: true);
            } else {
                CreateSlot(index, new Vector3(x, y, z + 10 * i));
            }
            ++index;
        }

        // Middle Column
        x = 0f;
        y = 0.5f;
        z = 20f;
        CreateSlot(index, new Vector3(x, y, z + 50));
        var middleIndex = 300;
        for (int i = 4; i >= 0; i--) {
            CreateSlot(middleIndex++, new Vector3(x, y, z + 10 * i), red);
        }
        ++index;

        // Right Column (inverted)
        x = 10f;
        y = 0.5f;
        z = 20f;
        for (int i = 5; i >= 0; i--) {
            if (i == 4) {
                CreateSlot(index, new Vector3(x, y, z + 10 * i), red, isHomeStop: true);
            } else {
                CreateSlot(index, new Vector3(x, y, z + 10 * i));
            }
            ++index;
        }
    }

    void CreatePath4Slots(ref int index) {
        Material yellow = Resources.Load("Materials/YellowMat", typeof(Material)) as Material;
        float x, y, z;
        y = 0.5f;

        // Left column
        x = 20f;
        z = 10f;
        for (int i = 0; i < 6; i++) {
            if (i == 3) {
                CreateSlot(index, new Vector3(x + 10 * i, y, z), yellow, isOtherStop: true);
            } else {
                CreateSlot(index, new Vector3(x + 10 * i, y, z));

            }
            ++index;
        }

        // Middle Column
        x = 20f;
        y = 0.5f;
        z = 0f;
        CreateSlot(index, new Vector3(x + 50, y, z));
        var middleIndex = 400;
        for (int i = 4; i >= 0; i--) {
            CreateSlot(middleIndex++, new Vector3(x + 10 * i, y, z), yellow);
        }
        ++index;

        // Right Column
        x = 20f;
        y = 0.5f;
        z = -10f;
        for (int i = 5; i >= 0; i--) {
            if (i == 4) {
                CreateSlot(index, new Vector3(x + 10 * i, y, z), yellow, isHomeStop: true);

            } else {
                CreateSlot(index, new Vector3(x + 10 * i, y, z));
            }
            ++index;
        }
    }

}
