using Mirror;
using UnityEngine;

public class TankBullet : NetworkBehaviour
{
    [field: SerializeField] public float speed { private set; get; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        GameplaySession.SpawnVFX(DBGameManager.EVFXID.BulletHit, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
