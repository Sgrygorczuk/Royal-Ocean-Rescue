using UnityEngine;

public class TalkZone : MonoBehaviour
{
    public Sprite sprite;
    public string[] sentences;


    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (!hitBox.CompareTag($"Ship")) return;
        GetComponent<AudioSource>().Play();
    }
}
