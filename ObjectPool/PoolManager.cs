using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Khởi tạo sẵn")]
    [SerializeField] private int initAmount = 10;

    [SerializeField] private List<GameObject> preloadPrefabs;

    private Dictionary<GameObject, ObjectPool> pools =
        new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (GameObject prefab in preloadPrefabs)
        {
            if (!pools.ContainsKey(prefab))
            {
                pools.Add(prefab,
                    new ObjectPool(prefab, initAmount, transform));
            }
        }
    }

    private ObjectPool GetPool(GameObject prefab)
    {
        if (!pools.TryGetValue(prefab, out ObjectPool pool))
        {
            pool = new ObjectPool(prefab, initAmount, transform);
            pools.Add(prefab, pool);
        }

        return pool;
    }

    public T GetObject<T>(GameObject prefab) where T : MonoBehaviour
    {
        return GetPool(prefab).GetObject<T>();
    }

    public GameObject GetObject(GameObject prefab)
    {
        return GetPool(prefab).GetObject();
    }
}