using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Cannon cannon;
    public Transform spawnPoint;
    private bool _shootingDirection;
    
    public void SpawnCannon()
    {
        var instantiate = Instantiate(this.cannon, spawnPoint.position, Quaternion.identity);
        instantiate.SetSpeed( _shootingDirection ? spawnPoint.right : spawnPoint.right * -1);
}

    
    public void ChangeShootingDirection()
    {
        _shootingDirection = !_shootingDirection;
    }
}
