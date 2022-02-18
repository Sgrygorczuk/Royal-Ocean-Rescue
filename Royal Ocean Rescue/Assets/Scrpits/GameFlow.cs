using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    //==== Control Scripts 
    private PlayerInput _playerInput;
    private TalkController _talkController;
    private Animator _fadeAnimator;      //Allows us to fade in and out 

    //===== Game Flow 
    private enum GameState             
    {
        Start,            //Loads in the scene and set up the first talking section 
        PlayerMoving,     //The player is in control of moving 
        TalkTime,         //Talking sections 
        End,              //The end cut scene plays, and we exit to credits scene 
    }
    
    private GameState _currentState = GameState.Start;      //Keeps track of what state we're currently in
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        _fadeAnimator = GameObject.Find($"Fade Canvas").GetComponent<Animator>();
        _playerInput = GameObject.Find($"Ship_With_Respawn").transform.Find($"Ship").GetComponent<PlayerInput>();
        _talkController = GameObject.Find($"Dialogue_Canvas").GetComponent<TalkController>();
    }

    // Update is called once per frame
    public void Update()
    {
        //Checks if the player is playing a desktop version if they are enables them to use ESC to quit out of the game 
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        //Checks what state the game is currently in and updates it 
        switch (_currentState)
        {
            case GameState.Start:
            {
                _currentState = GameState.TalkTime;
                StartCoroutine(StartGame());
                break;
            }
            case GameState.PlayerMoving:
            {
                _playerInput.PlayerUpdate();
                break;
            }
            case GameState.TalkTime:
            {
                _talkController.TalkUpdate();
                break;
            }
            case GameState.End:
            {
                break;
            }
        }
    }

    //Starts the game by fading from dark and starting the conversation 
    private IEnumerator StartGame()
    {
        _fadeAnimator.Play("ScreenFade");
        yield return new WaitForSeconds(1);
        _talkController.NextSentence();
    }
    
    //Exits talking and goes back to the player 
    public void EndDialogue()
    {
        _currentState = GameState.PlayerMoving;
    }

    public void EndGame()
    {
        _currentState = GameState.End;
        StartCoroutine(Wipe());   
    }
    
    //Start the talking section 
    public void EnterDialogue(string[] sentences, Sprite sprite)
    {
        _talkController.LoadText(sentences, sprite);
        _talkController.SetCanvas(true);
        _talkController.NextSentence();
        _currentState = GameState.TalkTime;
    }

    //Performs the animations and once it fade out we go to the next screen 
    private IEnumerator Wipe()
    {
        _fadeAnimator.Play("ScreenFadeIn");
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene($"TBC");
    }
}
