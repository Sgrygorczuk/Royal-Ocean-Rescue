using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitTimed : MonoBehaviour
{
    
    private float _spawnTime = 4;   //The timer
    public float startSpawn = 4;   //Max time the timer can be a
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (_spawnTime <= 0)
        {
            SceneManager.LoadScene($"MainMenu");
        }
        else
        {
            _spawnTime -= Time.deltaTime;
        }
        
    }
    
}
