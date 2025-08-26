using Mirror;
using UnityEngine;

public class DBPlayerTank : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    new bool isLocalPlayer = false;
    [SerializeField] Transform CameraPivot;
    [SerializeField] Transform TankTurret;
    [SerializeField] float turretRotSpeed = 45.0f;
    [SerializeField] GameObject bulletPrefab;
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
            AimTurret();
            SyncShoot();
        }
    }

    private void SyncShoot()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        CmdShoot();
    }
    [Command]
    private void CmdShoot()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, TankTurret.position + TankTurret.forward * 2f, TankTurret.rotation);
        NetworkServer.Spawn(bulletObject);
    }

    private void AimTurret()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = DBGameManager.instance.cameraController.cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 targetDir = hitPoint - TankTurret.position;
            Quaternion lookRotation = Quaternion.LookRotation(targetDir);
            Vector3 rotation = Quaternion.Lerp(TankTurret.rotation, lookRotation, Time.deltaTime * turretRotSpeed).eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0f, rotation.y, 0f);

            TankTurret.rotation = targetRotation;
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
