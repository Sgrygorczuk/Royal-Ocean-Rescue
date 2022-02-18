using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //============ Other Player Scripts 
    private PlayerController _playerController; //Controls the player movement 
    private PlayerShoot _playerShoot;           //Controls player shooting 

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //Connect to the scrpits 
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShoot = GetComponent<PlayerShoot>();
    }

    // Checks for player actions 
    public void PlayerUpdate()
    {
        //Set the speed to zero 
        var inputVector = Vector2.zero;

        //Check for player actions 
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        
        //Update those speed 
        _playerController.SetInput(inputVector);

        //If player shoots shoot 
        if (Input.GetButtonDown("Fire1")) { _playerShoot.SpawnCannon(); }
        
        //Changes player shooting side 
        if(Input.GetButtonDown("Fire2")){_playerShoot.ChangeShootingDirection();}
    }
    
    //Checks if player has collided with a Talk Zone, if so stops the boat and gives the controls over to it 
    private void OnTriggerEnter2D(Collider2D hitBox)
    {

        if (hitBox.CompareTag($"Exit"))
        {
            GameObject.Find($"Main_Camera").GetComponent<GameFlow>().EndGame();
        }

        if (!hitBox.CompareTag($"TalkZone")) return;
        //Stop Moving 
        var inputVector = Vector2.zero;
        _playerController.SetInput(inputVector);
        hitBox.GetComponent<BoxCollider2D>().enabled = false;
        
        //Start the talking 
        GameObject.Find($"Main_Camera").GetComponent<GameFlow>().EnterDialogue(
            hitBox.GetComponent<TalkZone>().sentences, hitBox.GetComponent<TalkZone>().sprite);
    }
    
}
