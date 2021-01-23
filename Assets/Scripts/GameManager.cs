using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject slotPrefab;
    public GameObject slot1Prefab;
    public GameObject slot2Prefab;
    public GameObject slot3Prefab;
    public GameObject slot4Prefab;

    public GameObject playerPiece;

    // Start is called before the first frame update
    void Start() {
        CreateAllPathSlots();
        CreatePlayerPieces();

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
        GameObject newPiece = Instantiate(playerPiece, pos, transform.rotation, parent);
        newPiece.GetComponent<Renderer>().material = mat;
        newPiece.name = name;
    }

    void Update() {

    }

    void CreateSlot(GameObject prefab, Vector3 pos) {
        Transform parent = GameObject.Find("Slots").transform;
        GameObject newObject = Instantiate(prefab, pos, transform.rotation, parent);
    }

    void CreateAllPathSlots() {
        CreatePath1Slots();
        CreatePath2Slots();
        CreatePath3Slots();
        CreatePath4Slots();
    }

    void CreatePath1Slots() {
        float x, y, z;
        x = -10f;
        y = 0.5f;
        z = -20f;
        // Left column
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x, y, z - 10 * i));
        }
        // Middle Column
        x = 0f;
        y = 0.5f;
        z = -20f;
        for (int i = 0; i < 5; i++) {
            CreateSlot(slot1Prefab, new Vector3(x, y, z - 10 * i));
        }
        CreateSlot(slotPrefab, new Vector3(x, y, z - 50));
        // Right Column
        x = 10f;
        y = 0.5f;
        z = -20f;
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x, y, z - 10 * i));
        }
    }

    void CreatePath2Slots() {
        float x, y, z;
        y = 0.5f;

        x = -20f;
        z = 10f;
        // Left column
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x - 10 * i, y, z));
        }
        // Middle Column
        x = -20f;
        z = 0f;
        for (int i = 0; i < 5; i++) {
            CreateSlot(slot2Prefab, new Vector3(x - 10 * i, y, z));
        }
        CreateSlot(slotPrefab, new Vector3(x - 50, y, z));
        // Right Column
        x = -20f;
        z = -10f;
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x - 10 * i, y, z));
        }
    }

    void CreatePath3Slots() {
        float x, y, z;
        x = -10f;
        y = 0.5f;
        z = 20f;
        // Left column
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x, y, z + 10 * i));
        }
        // Middle Column
        x = 0f;
        y = 0.5f;
        z = 20f;
        for (int i = 0; i < 5; i++) {
            CreateSlot(slot3Prefab, new Vector3(x, y, z + 10 * i));
        }
        CreateSlot(slotPrefab, new Vector3(x, y, z + 50));
        // Right Column
        x = 10f;
        y = 0.5f;
        z = 20f;
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x, y, z + 10 * i));
        }
    }

    void CreatePath4Slots() {
        float x, y, z;
        x = 20f;
        y = 0.5f;
        z = 10f;
        // Left column
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x + 10 * i, y, z));
        }
        // Middle Column
        x = 20f;
        y = 0.5f;
        z = 0f;
        for (int i = 0; i < 5; i++) {
            CreateSlot(slot4Prefab, new Vector3(x + 10 * i, y, z));
        }
        CreateSlot(slotPrefab, new Vector3(x + 50, y, z));
        // Right Column
        x = 20f;
        y = 0.5f;
        z = -10f;
        for (int i = 0; i < 6; i++) {
            CreateSlot(slotPrefab, new Vector3(x + 10 * i, y, z));
        }
    }

}
