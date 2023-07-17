using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HierarchyManager : MonoBehaviour
{
    public static HierarchyManager Instance { get; private set; }
    [SerializeField] private string[] hierarchyObjectKeys;
    [SerializeField] [field: FormerlySerializedAs("hierachyObjects")] private GameObject[] hierarchyObjects;
    private Dictionary<string, GameObject> hierarchyDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //Create Dictionary
        if (hierarchyObjects.Length != hierarchyObjectKeys.Length) return;
        for (int i = 0; i < hierarchyObjects.Length; i++)
        {
            hierarchyDict.Add(hierarchyObjectKeys[i], hierarchyObjects[i]);
        }
    }

    public GameObject GetHierarchyObject(string key)
    {
        return hierarchyDict[key];
    }
}
