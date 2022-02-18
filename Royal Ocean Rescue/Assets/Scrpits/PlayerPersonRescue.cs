using UnityEngine;

public class PlayerPersonRescue : MonoBehaviour
{
    /// <summary>
    /// This Script controls the creation of rowboat and which boat they will follow upon instantiation 
    /// </summary>
    
    //======== Spawning 
    private GameObject _rowBoat;                //The Game Object of the Row Boat prefab 
    private GameObject _boatHolderGameObject;   //The Game Object where the rowboats will spawn into 
    
    //========= Rears 
    private Transform _mainBoatRear;            //The players rear
    private Transform _rear;                    //The rear this boat will follow 
    
    //========= Misc
    private Rigidbody2D _rigidbody2D;           //Rigidbody
    public AudioSource audioSource;             //Audio that plays when it's spawned 
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //Connects all the components 
    private void Awake()
    {
        //Bring it in from Resources folder 
        _rowBoat =  Resources.Load("PreFabs/RowBoat") as GameObject;
        //Get from the object 
        _boatHolderGameObject = transform.Find($"Boats").gameObject;
        _mainBoatRear = GameObject.Find($"Ship_With_Respawn").transform.Find($"Ship").transform.Find($"Rear");
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //Checks if the player hit a person if they did de-spawn the person and spawn in a rowboat that will follow the line 
    private void OnTriggerExit2D(Collider2D hitBox)
    {
        if (!hitBox.CompareTag($"Person")) return;
        audioSource.Play();
        //Check if we have any rowboats, if we don't connect to follow player if we do follow the last rowboat in _boatHolderGameObject
        _rear = _boatHolderGameObject.transform.childCount == 0 ? _mainBoatRear : 
            _boatHolderGameObject.transform.GetChild(_boatHolderGameObject.transform.childCount - 1).gameObject.GetComponent<RowBoat>().GetRear();
        
        //Create the row boat 
        var instantiate = Instantiate(_rowBoat, hitBox.transform.position, Quaternion.identity);
        
        //Give the rowboat data so it can know what to follow and how to update if something changes 
        instantiate.GetComponent<RowBoat>().SetPreFabData(_rear, transform, _rigidbody2D,
            _boatHolderGameObject.transform.childCount, _boatHolderGameObject);
        
        //Attach it to the _boatHolderGameObject 
        instantiate.transform.SetParent(_boatHolderGameObject.transform);
        
        //Destroy the original person game object
        Destroy(hitBox.gameObject);
    }
}
