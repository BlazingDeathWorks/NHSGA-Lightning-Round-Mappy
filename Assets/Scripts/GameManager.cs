using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ASSUMING THAT ALL ITEMS ARE NAMED DIFFERENTLY
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private List<GameObject> itemObjects = new List<GameObject>();
    private static bool s_firstTime = true;
    private static int s_collectedCount;
    private ItemGameData data;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        data = new ItemGameData();

        if (s_firstTime)
        {
            Debug.Log("First Time This Scene");
            s_firstTime = false;
            //Clean everything that is saved
            data = BinarySaveSystem.LoadSystem<ItemGameData>(Application.persistentDataPath + "/itemData.bin");
            data.ItemObjectID.Clear();
            BinarySaveSystem.SaveSystem(data, Application.persistentDataPath + "/itemData.bin");
            return;
        }

        //If we are reloading the same level...
        //Load Binary Save System String Array
        //Compare to itemObject reference and destroy the ones that are in both lists
        data = BinarySaveSystem.LoadSystem<ItemGameData>(Application.persistentDataPath + "/itemData.bin");
        Debug.Log("WTF");
        for (int i = 0; i < data.ItemObjectID.Count; i++)
        {
            for (int j = 0; j < itemObjects.Count; j++)
            {
                if (j < 0) j = 0;
                if (data.ItemObjectID[i] == itemObjects[j].name)
                {
                    GameObject item = itemObjects[j];
                    itemObjects.Remove(item);
                    Destroy(item);
                    j--;
                }
            }
        }
    }

    //Before destroy, each item calls this method to let system know not to spawn it in the case of a reload
    public void RegisterItemAsCollected(GameObject item)
    {
        //Save itemObjects into Serializable class and request save through BinarySaveSystem
        data.ItemObjectID.Add(item.name);
        BinarySaveSystem.SaveSystem(data, Application.persistentDataPath + "/itemData.bin");
        Debug.Log("Should have saved: " + item.name);
    }

    //TODO: CALL THIS WHEN GOING TO THE NEXT LEVEL
    private void ResetFirstTime()
    {
        s_firstTime = true;
    }

    public void AddToCollectedCount()
    {
        s_collectedCount++;
        if (s_collectedCount >= 10)
        {
            //Go to next level
            s_collectedCount = 0;
            ResetFirstTime();
            SceneController.Instance.NextScene();
        }
    }
}

[Serializable]
public class ItemGameData
{
    public List<string> ItemObjectID = new List<string>();
}