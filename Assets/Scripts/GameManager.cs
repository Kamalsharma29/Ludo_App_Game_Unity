using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject slot1Prefab;
    public GameObject slot2Prefab;
    public GameObject slot3Prefab;
    public GameObject slot4Prefab;
    // Start is called before the first frame update
    void Start()
    {
        CreatePath1Slots();
        CreatePath2Slots();
        CreatePath3Slots();
        CreatePath4Slots();
    }

    void CreateSlot(GameObject prefab, Vector3 pos) {

        GameObject newObject = Instantiate(prefab, pos, transform.rotation);
        // newObject.transform.parent = GameObject.Find("Home").transform;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
