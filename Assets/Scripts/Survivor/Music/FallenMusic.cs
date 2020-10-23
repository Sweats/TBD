using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenMusic : MonoBehaviour
{
    [SerializeField]
    private AudioSource fallenAmbientMusic;

    [SerializeField]
    private AudioSource fallenCloseMusic;

    [SerializeField]
    private float fallenAmbientMusicDistance;

    [SerializeField]
    private float fallenCloseMusicDistance;

    private Transform survivorPosition;

    void Start()
    {
	    survivorPosition = GetComponent<Transform>();

    }

    public void Detect()
    {
	    StartCoroutine(Detect());
    }

    private IEnumerator Detect()
    {
	    bool fallenClose = Music.ShouldPlayMusic()

    }

    private void 

}
