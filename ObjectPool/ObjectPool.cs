using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject _prefab;
    private List<GameObject> _objects = new List<GameObject>();
    private Transform _parent;

    public ObjectPool(GameObject prefab, int initAmount, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initAmount; i++)
        {
            CreateInstance();
        }
    }

    private GameObject CreateInstance()
    {
        GameObject obj = Object.Instantiate(_prefab, _parent);
        obj.SetActive(false);
        _objects.Add(obj);
        return obj;
    }

    public T GetObject<T>() where T : MonoBehaviour
    {
        foreach (GameObject obj in _objects)
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
        foreach (GameObject obj in _objects)
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
