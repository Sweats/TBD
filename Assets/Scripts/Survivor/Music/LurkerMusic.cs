using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurkerMusic : MonoBehaviour
{
    private Transform surviorPosition;

    private bool lurkerAround;

    [SerializeField]
    private AudioSource lurkerAmbientMusic;

    [SerializeField]
    private AudioSource lurkerCloseMusic;

    [SerializeField]
    private float lurkerAmbientMusicDistance;

    [SerializeField]
    private float lurkerCloseMusicDistance;

    void Start()
    {
        surviorPosition = GetComponent<Transform>();
        EventManager.lurkerChangedFormEvent.AddListener(OnLurkerFormChanged);
    }

    private void OnLurkerFormChanged(bool ghostForm)
    {
        if (!ghostForm)
        {
            lurkerAround = true;
            StartCoroutine(Detect());
        }

        else
        {
            lurkerAround = false;
        }
    }

    private IEnumerator Detect()
    {
        bool lurkerClose, lurkerFar;

        // TODO: Maybe we only want to do a distance calculation instead of a ray cast.

        while (true)
        {
            if (!lurkerAround)
            {
                yield break;
            }

            lurkerClose = Music.ShouldPlayMusic(surviorPosition, lurkerCloseMusicDistance, "Lurker");
            lurkerFar = Music.ShouldPlayMusic(surviorPosition, lurkerAmbientMusicDistance, "Lurker");

            if (lurkerClose && !lurkerFar)
            {
                if (lurkerAmbientMusic.isPlaying)
                {
                    lurkerAmbientMusic.Stop();
                }

                lurkerCloseMusic.Play();
            }

            else if (!lurkerClose && lurkerFar)
            {
                if (lurkerCloseMusic.isPlaying)
                {
                    lurkerCloseMusic.Stop();
                }

                lurkerAmbientMusic.Play();
            }

            else
            {
                if (lurkerCloseMusic.isPlaying)
                {
                    lurkerCloseMusic.Stop();

                }

                if (lurkerAmbientMusic.isPlaying)
                {
                    lurkerAmbientMusic.Stop();
                }
            }

            yield return new WaitForSeconds(1);
        }
    }


    // TODO: Have an event call this when everyone spawns in the stage at the same time.
    private void OnLurkerSpawnedInStage()
    {

    }

}
