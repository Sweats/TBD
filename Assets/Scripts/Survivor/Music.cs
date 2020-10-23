using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the music for the survivors. Probably gonna be a lot of stuff in here.
//
public class Music : MonoBehaviour
{
    [Serializable]
    private AudioSource lurkerAmbientMusic;

    private Transform survivorPosition;

    [SerializeField]
    private AudioSource lurkerCloseMusic;


    private bool lurkerAround;

    [SerializeField]
    private float lurkerAmbientMusicDistance;

    [SerializeField]
    private float lurkerCloseMusicDistance;

    [SerializeField]
    private AudioSource phantomAmbientMusic;


    [SerializeField]
    private AudioSource phantomCloseMusic;

    [SerializeField]
    private float phantomAmbientMusicDistance;

    [SerializeField]
    private float phantomCloseMusicDistance;

    [SerializeField]
    private AudioSource fallenAmbientMusic;

    [SerializeField]
    private AudioSource fallenCloseMusic;

    [SerializeField]
    private AudioSource fallenAmbientMusicDistance;

    [SerializeField]
    private AudioSource fallenCloseMusicDistance;



    void Start()
    {
        survivorPosition = GetComponent<Transform>();

    }



    private bool IsPhantomClose()
    {

    }


    private bool IsPhantomFar()
    {

    }

    private IEnumerable DetectPhantom()
    {
        while (true)
        {

        }

    }

    private IEnumerable DetectMary()
    {

    }

    private IEnumerable DetectFallen()
    {

    }
}
