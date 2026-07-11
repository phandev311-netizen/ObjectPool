using UnityEngine;

public class Object : MonoBehaviour
{
    private float _lifeTime = 1f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
