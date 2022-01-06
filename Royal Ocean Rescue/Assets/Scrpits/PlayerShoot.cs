using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    public Cannon cannon;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;
    private bool _shootingDirection;
    public GameObject original;
    public Image left;
    public Image right;

    private void Awake()
    {
        SetColor();
    }

    public void SpawnCannon()
    {
        if (original.transform.childCount != 0) return;
        var instantiate = Instantiate(cannon, _shootingDirection ? spawnPointRight.position : spawnPointLeft.position, Quaternion.identity);
        instantiate.transform.SetParent(original.transform);
        //Make sure it moves in the direction that the ship is turned towards 
        instantiate.SetPosition(_shootingDirection
            ? new Vector2(Mathf.Cos((Mathf.PI / 180) * transform.rotation.eulerAngles.z),
                Mathf.Sin((Mathf.PI / 180) * transform.rotation.eulerAngles.z))
            : new Vector2(-Mathf.Cos((Mathf.PI / 180) * transform.rotation.eulerAngles.z),
                -Mathf.Sin((Mathf.PI / 180) * transform.rotation.eulerAngles.z)));
        //Make sure it moves as fast as the ship was moving
        instantiate.GetComponent<Rigidbody2D>().velocity = transform.GetComponent<Rigidbody2D>().velocity;
    }

    
    public void ChangeShootingDirection()
    {
        _shootingDirection = !_shootingDirection;
        SetColor();
    }

    private void SetColor()
    {
        if (_shootingDirection)
        {
            left.color = Color.gray;
            right.color = Color.white;
        }
        else
        {
            left.color = Color.white;
            right.color = Color.gray;
        }
    }
}
