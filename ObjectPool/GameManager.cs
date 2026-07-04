using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn(prefabA);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Spawn(prefabB);
        }
    }

    private void Spawn(GameObject prefab)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;

        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        GameObject obj = PoolManager.Instance.GetObject(prefab);
        obj.transform.position = worldPos;
        obj.transform.rotation = Quaternion.identity;
    }
}