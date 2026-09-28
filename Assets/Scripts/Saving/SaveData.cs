using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public List<DirtSaveData> dirtData;
    public List<PlaceableSaveData> placeableData;
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
    public int day;
    public int hour;
    public int minute;
}
