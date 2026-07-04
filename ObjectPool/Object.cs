using UnityEngine;

public class Object : MonoBehaviour
{
    private float lifeTime = 1f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
