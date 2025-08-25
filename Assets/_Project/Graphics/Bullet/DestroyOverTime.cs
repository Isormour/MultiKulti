using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 1f;
    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }
}
