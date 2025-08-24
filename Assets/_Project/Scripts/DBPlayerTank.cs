using Mirror;
using UnityEngine;

public class DBPlayerTank : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    new bool isLocalPlayer = false;
    [SerializeField] Transform CameraPivot;

    public override void OnStartAuthority()
    {
        isLocalPlayer = true;
        DBGameManager.instance.cameraController.SetTarget(CameraPivot);

    }
    void Update()
    {
        if (isLocalPlayer)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        }
    }
}
