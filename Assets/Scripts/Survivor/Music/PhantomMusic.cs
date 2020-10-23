using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomMusic : MonoBehaviour
{

    [SerializeField]
    private AudioSource phantomAmbientMusic;

    [SerializeField]
    private AudioSource phantomCloseMusic;

    [SerializeField]
    private float phantomAmbientMusicDistance;

    [SerializeField]
    private float phantomCloseMusicDistance;

    private Transform position;

    void Start()
    {
        position = GetComponent<Transform>();
        StartCoroutine(Detect());
    }


    private IEnumerator Detect()
    {
        bool phantomClose, phantomFar;

        while (true)
        {
            phantomClose = Music.ShouldPlayMusic(position, phantomCloseMusicDistance, "Phantom");
            phantomFar = Music.ShouldPlayMusic(position, phantomAmbientMusicDistance, "Phantom");

            if (phantomFar && !phantomClose)
            {
                if (phantomCloseMusic.isPlaying)
                {
                    phantomCloseMusic.Stop();
                }

                phantomAmbientMusic.Play();
            }

            else if (phantomClose && phantomFar)
            {
                if (phantomAmbientMusic.isPlaying)
                {
                    phantomAmbientMusic.Stop();
                }

            }

            else
            {
                if (phantomAmbientMusic.isPlaying)
                {
                    phantomAmbientMusic.Stop();
                }

                if (phantomCloseMusic.isPlaying)
                {
                    phantomCloseMusic.Stop();
                }
            }
        }

	yield return new WaitForSeconds(1);
    }
}
