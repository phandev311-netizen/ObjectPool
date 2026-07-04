using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private List<GameObject> objects = new List<GameObject>();
    private Transform parent;

    public ObjectPool(GameObject prefab, int initAmount, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initAmount; i++)
        {
            CreateInstance();
        }
    }

    private GameObject CreateInstance()
    {
        GameObject obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);
        objects.Add(obj);
        return obj;
    }

    public T GetObject<T>() where T : MonoBehaviour
    {
        foreach (GameObject obj in objects)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj.GetComponent<T>();
            }
        }

        GameObject newObj = CreateInstance();
        newObj.SetActive(true);

        return newObj.GetComponent<T>();
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in objects)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = CreateInstance();
        newObj.SetActive(true);

        return newObj;
    }
}