using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerShoot _playerShoot;
    private TalkController _talkController;
    public int gameState = 0;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerShoot = GetComponent<PlayerShoot>();
        _talkController = GetComponent<TalkController>();

    }

    // Update is called once per frame
    private void Update()
    {

        switch (gameState)
        {
            case 0:
            {
                var inputVector = Vector2.zero;

                inputVector.x = Input.GetAxis("Horizontal");
                inputVector.y = Input.GetAxis("Vertical");
        
                _playerController.SetInput(inputVector);

                if (Input.GetButtonDown("Fire1"))
                {
                    _playerShoot.SpawnCannon();
                }
        
                if(Input.GetButtonDown("Fire2")){_playerShoot.ChangeShootingDirection();}
                break;
            }
            case 1:
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    _talkController.PlayAudio();
                    if (_talkController.Done())
                    {
                        EndDialogue();
                        break;
                    }
                    _talkController.NextSentence();
                }
                
                break;
            }
        }
    }

    public void EndDialogue()
    {
        gameState = 0;
        _talkController.SetCanvas(false);
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (!hitBox.CompareTag($"TalkZone")) return;
        var inputVector = Vector2.zero;
        _playerController.SetInput(inputVector);
        _talkController.LoadText(hitBox.GetComponent<TalkZone>().sentences, hitBox.GetComponent<TalkZone>().sprite);
        hitBox.GetComponent<BoxCollider2D>().enabled = false;
        _talkController.SetCanvas(true);
        _talkController.NextSentence();
        gameState = 1;
    }
}
