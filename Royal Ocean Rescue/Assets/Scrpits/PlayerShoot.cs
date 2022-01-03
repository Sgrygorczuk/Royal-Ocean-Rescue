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
        var instantiate = Instantiate(cannon, spawnPointRight.parent.position, Quaternion.identity);
        instantiate.SetPosition(_shootingDirection ? spawnPointRight : spawnPointLeft);
        instantiate.transform.SetParent(original.transform);
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
