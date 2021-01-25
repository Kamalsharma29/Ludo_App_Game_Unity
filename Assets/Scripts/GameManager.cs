using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour {
    public GameObject slotPrefab;

    public GameObject playerPiecePrefab;

    public List<int> movesList;

    public List<PieceType> pieceTurn;

    public int[,] path;
    public List<Slot> slotList;

    public Vector3 MovePiece(PlayerPiece pp, int numToMove) {
        int index = MapImaginaryToReal(pp);
        index += numToMove;
        if (index > 51) {
            index -= 51;
        }

        pp.MoveForward(numToMove);

        return slotList[index].loc;
    }

    void printPath() {
        for (int i = 0; i < path.GetLength(0); i++) {
            print($"{path[i, 0]}:{path[i, 1]}");
        }
    }

    public int MapImaginaryToReal(PlayerPiece pp) {
        var curLoc = pp.location;
        for (int i = 0; i < path.GetLength(0); i++) {
            if (path[i, 0] == curLoc[0]) {
                if (path[i, 1] == curLoc[1]) {
                    return i;
                }
            }
        }

        return -1;
    }


    void Start() {
        pieceTurn = new List<PieceType>() { PieceType.P1, PieceType.P2, PieceType.P3, PieceType.P4 };
        slotList = new List<Slot>();


        InitializeInThisOrder();

        // var x = GameObject.Find("Piece_11");
        // print(x.GetComponent<PieceTapped>().piece);



    }

    void InitializeInThisOrder() {

        InitializePath();
        CreateAllPathSlots();
        CreatePlayerPieces();
    }

    void InitializePath() {
        path = new int[Constants.MAX_PATH, 2];
        for (int i = 1; i <= 4; i++) {
            for (int j = 1; j <= path.GetLength(0) / 4; j++) {
                var index = (i - 1) * path.GetLength(0) / 4 + j - 1; // 0-51
                path[index, 0] = i;
                path[index, 1] = j;
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

        x = -30;
        z = -60;
        for (int i = 1; i <= 4; i++, x -= 10) {
            CreatePlayerPiece($"{pieceTxt}1{i}", new Vector3(x, y, z), darkGreen);
        }

        x = -60;
        z = 30;
        for (int i = 1; i <= 4; i++, z += 10) {
            CreatePlayerPiece($"{pieceTxt}2{i}", new Vector3(x, y, z), darkBlue);
        }

        x = 30;
        z = 60;
        for (int i = 1; i <= 4; i++, x += 10) {
            CreatePlayerPiece($"{pieceTxt}3{i}", new Vector3(x, y, z), darkRed);
        }

        x = 60;
        z = -30;
        for (int i = 1; i <= 4; i++, z -= 10) {
            CreatePlayerPiece($"{pieceTxt}4{i}", new Vector3(x, y, z), darkYellow);
        }

    }

    private void CreatePlayerPiece(string name, Vector3 pos, Material mat) {
        Transform parent = GameObject.Find("Pieces").transform;
        GameObject newPiece = Instantiate(playerPiecePrefab, pos, transform.rotation, parent);
        newPiece.GetComponent<Renderer>().material = mat;
        newPiece.name = name;
        newPiece.AddComponent<PieceTapped>();
    }


    void CreateSlot(int index, Vector3 pos, bool addSlot = true, Material mat = null) {
        var slotText = "Slot_";
        Transform parent = GameObject.Find("Slots").transform;
        GameObject newSlot = Instantiate(slotPrefab, pos, transform.rotation, parent);
        if (mat != null)
            newSlot.GetComponent<Renderer>().material = mat;
        newSlot.name = slotText += $"{path[index, 0]}:{path[index, 1]}";
        if (addSlot) {
            Slot other = new Slot(pos);
            slotList.Add(other);
        }
        // newSlot.GetComponent<EachSlot>().slot = slot;
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
                CreateSlot(index, new Vector3(x, y, z - 10 * i), green);
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
        for (int i = 0; i < 5; i++) {
            float newX = x, newZ = z - 10 * i;
            // Slot slot = new Slot(new int[] { (int)newX, (int)newZ });
            CreateSlot(index, new Vector3(x, y, z - 10 * i), false, green);
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
                CreateSlot(index, new Vector3(newX, y, newZ), green);
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
            if (i == 4) {
                CreateSlot(index, new Vector3(x - 10 * i, y, z), blue);
            } else {
                CreateSlot(index, new Vector3(x - 10 * i, y, z));
            }
            ++index;
        }
        // Middle Column
        x = -20f;
        z = 0f;
        CreateSlot(index, new Vector3(x - 50, y, z));
        for (int i = 0; i < 5; i++) {
            CreateSlot(index, new Vector3(x - 10 * i, y, z), false, blue);
        }
        ++index;

        // Left column
        x = -20f;
        z = 10f;
        for (int i = 5; i >= 0; i--) {
            if (i == 3) {
                CreateSlot(index, new Vector3(x - 10 * i, y, z), blue);
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
                CreateSlot(index, new Vector3(x, y, z + 10 * i), red);
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
        for (int i = 0; i < 5; i++) {
            CreateSlot(index, new Vector3(x, y, z + 10 * i), false, red);
        }
        ++index;

        // Right Column (inverted)
        x = 10f;
        y = 0.5f;
        z = 20f;
        for (int i = 5; i >= 0; i--) {
            if (i == 4) {
                CreateSlot(index, new Vector3(x, y, z + 10 * i), red);
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
                CreateSlot(index, new Vector3(x + 10 * i, y, z), yellow);
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
        for (int i = 0; i < 5; i++) {
            CreateSlot(index, new Vector3(x + 10 * i, y, z), false, yellow);
        }
        ++index;

        // Right Column
        x = 20f;
        y = 0.5f;
        z = -10f;
        for (int i = 5; i >= 0; i--) {
            if (i == 4) {
                CreateSlot(index, new Vector3(x + 10 * i, y, z), yellow);

            } else {
                CreateSlot(index, new Vector3(x + 10 * i, y, z));
            }
            ++index;
        }
    }

}
