using UnityEngine;

public class DBGameManager : MonoBehaviour
{
    public enum EVFXID
    {
        BulletHit
    }
    public static DBGameManager instance;
    [SerializeField] GameObject[] vfxPrefabs;

    [field: SerializeField] public CameraController cameraController { get; private set; }


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    public static void SpawnVFX(EVFXID id, Vector3 position, Quaternion rotation)
    {
        Instantiate(instance.vfxPrefabs[(int)id], position, rotation);
    }
}
