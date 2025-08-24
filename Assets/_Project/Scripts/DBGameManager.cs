using UnityEngine;

public class DBGameManager : MonoBehaviour
{
    public static DBGameManager instance;
    [field: SerializeField] public CameraController cameraController { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
