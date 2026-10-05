using System.Collections.Generic;
using UnityEngine;


public class GenerateScript : MonoBehaviour
{
    [SerializeField] private GameObject testPrefab;
    [SerializeField] private List<GameObject> spawnItemList;
    [SerializeField] private List<GameObject> spawnAreaList;

    [SerializeField] private int itemSpawnAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private void OnEnable()
    {   
        TimeManager.onDayChanged += wipeItems;
    }

    private void OnDisable()
    {   
        TimeManager.onDayChanged -= wipeItems;
    }

    void generateItems()
    {
      
       for (int i = 0; i < itemSpawnAmount; i++)
        {
            GameObject spawnArea = spawnAreaList[Random.Range(0, spawnAreaList.Count)];
            GameObject spawnItem = spawnItemList[Random.Range(0, spawnItemList.Count)];

            if(spawnArea.transform.childCount == 0)
            {
                Instantiate(spawnItem, spawnArea.transform.position, Quaternion.identity, spawnArea.transform);
            }
            else
            {
                i--;
            }
        }
    }

    void wipeItems()
    {
        foreach(GameObject spawnArea in spawnAreaList)
        {
            if(spawnArea.transform.childCount != 0)
            {
                Destroy(spawnArea.transform.GetChild(0).gameObject);
            }
        }
        generateItems();
    }

}
