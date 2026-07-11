using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [Header("Khởi tạo sẵn")]
    [SerializeField] private int _initAmount = 10;

    [SerializeField] private List<GameObject> _preloadPrefabs;

    private Dictionary<GameObject, ObjectPool> _pools =
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

        foreach (GameObject prefab in _preloadPrefabs)
        {
            if (!_pools.ContainsKey(prefab))
            {
                _pools.Add(prefab,
                    new ObjectPool(prefab, _initAmount, transform));
            }
        }
    }

    private ObjectPool GetPool(GameObject prefab)
    {
        if (!_pools.TryGetValue(prefab, out ObjectPool pool))
        {
            pool = new ObjectPool(prefab, _initAmount, transform);
            _pools.Add(prefab, pool);
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
