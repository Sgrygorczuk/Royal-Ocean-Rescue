using System;
using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    public Cannon cannon;

    private bool _playerInRange = false;
    private Transform _shipTransform;

    private float _spawnTime = 2;   //The timer
    public float startSpawn = 2;   //Max time the timer can be a
    
    private void Start()
    {
        _shipTransform = GameObject.Find("Ship").transform;
    }

    private void Update()
    {
        if (!_playerInRange) return;
        var difference = _shipTransform.position - transform.position;
        var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            
        if (_spawnTime <= 0)
        {
            var instantiate = Instantiate(cannon, transform.position, Quaternion.identity);
            //Make sure it moves in the direction that the ship is turned towards 
            instantiate.SetPosition( _shipTransform.transform.position - transform.position);
            instantiate.speed = 1;
            //Resets the spawn timer 
            _spawnTime = startSpawn;
        }
        else
        {
            //Updates the spawn timer 
            _spawnTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Ship"))
        {
            _playerInRange = false;
        }
    }
}
