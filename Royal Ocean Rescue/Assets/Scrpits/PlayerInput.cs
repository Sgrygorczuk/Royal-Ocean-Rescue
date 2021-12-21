using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerShoot _playerShoot;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShoot = GetComponent<PlayerShoot>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        var inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        
        _playerController.SetInput(inputVector);

        if (Input.GetButtonDown("Fire1")) { _playerShoot.SpawnCannon(); }
        
        if(Input.GetButtonDown("Fire2")){_playerShoot.ChangeShootingDirection();}
    }
}
