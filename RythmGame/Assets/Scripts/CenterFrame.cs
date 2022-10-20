using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    AudioSource myAudio;
    bool musicStart = false;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note") && !musicStart)//첫 노트에서만 실행되도록 함

        {
            myAudio.Play();
            musicStart = true;
        }
    }
}
