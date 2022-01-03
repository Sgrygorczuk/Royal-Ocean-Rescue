using UnityEngine;

public class PersonRescue : MonoBehaviour
{
    public RowBoat _rowBoat;
    public GameObject _original;
    public Transform mainBoatRear;
    private Transform _rear;
    private Rigidbody2D _rigidbody2D;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (hitBox.CompareTag($"Person"))
        {
            _rear = _original.transform.childCount == 0 ? mainBoatRear : _original.transform.GetChild(_original.transform.childCount - 1).gameObject.GetComponent<RowBoat>().ownRear;
            var instantiate = Instantiate(_rowBoat, hitBox.transform.position, Quaternion.identity);
            instantiate.SetPreFabData(_rear, transform, _rigidbody2D, _original.transform.childCount, _original);
            instantiate.transform.SetParent(_original.transform);
            Destroy(hitBox.gameObject);
        }
    }
}
