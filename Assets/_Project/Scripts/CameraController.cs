using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform targetToFollow;
    [SerializeField] float followMoveSpeed;
    [SerializeField] float followRotSpeed;
    [SerializeField] Camera cam;
    [SerializeField] float maxOrtographicSize;
    [SerializeField] float maxHeight;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }
    public void SetTarget(Transform target)
    {
        targetToFollow = target;
    }
    void FollowTarget()
    {
        if (!targetToFollow) return;
        Vector3 pos = Vector3.Lerp(this.transform.position, targetToFollow.position, Time.deltaTime * followMoveSpeed);
        Quaternion rot = Quaternion.Lerp(this.transform.rotation, targetToFollow.rotation, Time.deltaTime * followRotSpeed);
        transform.SetPositionAndRotation(pos, rot);
        cam.orthographicSize = (transform.position.y / maxHeight) * maxOrtographicSize;
    }
}
