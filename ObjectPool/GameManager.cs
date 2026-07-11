using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefabA;
    [SerializeField] private GameObject _prefabB;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn(_prefabA);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Spawn(_prefabB);
        }
    }

    private void Spawn(GameObject prefab)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;

        Vector3 worldPos = _cam.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        GameObject obj = PoolManager.Instance.GetObject(prefab);
        obj.transform.position = worldPos;
        obj.transform.rotation = Quaternion.identity;
    }
}
