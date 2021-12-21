using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonRescue : MonoBehaviour
{
    public RowBoat _rowBoat;
    public Person _person;
    public Transform mainBoatRear;
    private List<RowBoat> _boats;
    private Transform _rear;
    private Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boats = new List<RowBoat>();
    }

    private void Update()
    {
        for (var i = 0; i < _boats.Count; i++)
        {
            if (_boats[i].health == 0)
            {
                UpdateChain(i);
            }
        }
    }

    private void UpdateChain(int i)
    {
        if (i == 0 && _boats.Count == 1)
        {
            //var instantiate = Instantiate(_person, _boats[i].transform.position, Quaternion.identity);
            Destroy(_boats[i]);   
        }
    }

    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Person"))
        {
            _rear = _boats.Count == 0 ? mainBoatRear : _boats[_boats.Count - 1].ownRear;
            _rowBoat.SetPreFabData(_rear, transform, _rigidbody2D);
            var instantiate = Instantiate(_rowBoat, hitBox.transform.position, Quaternion.identity);
            _boats.Add(instantiate);
            Destroy(hitBox.gameObject);
        }
    }
}
