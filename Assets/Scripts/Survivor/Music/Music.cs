using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// NOTE:Handles the music for the survivors.
public class Music : MonoBehaviour
{
    [SerializeField]
    private AudioSource lurkerCloserMusic;

    [SerializeField]
    private AudioSource lurkerCloseMusic;

    [SerializeField]
    private AudioSource[] lurkerPassedThroughSurviorSounds;

    [SerializeField]
    private float lurkerCloseMusicDistance;

    [SerializeField]
    private float lurkerCloserMusicDistance;

    [SerializeField]
    private float lurkerGhostTouchingDistance;

    [SerializeField]
    private AudioSource phantomCloseMusic;

    [SerializeField]
    private AudioSource phantomCloserMusic;

    [SerializeField]
    private float phantomCloseMusicDistance;

    [SerializeField]
    private float phantomCloserMusicDistance;

    [SerializeField]
    private AudioSource maryCloseMusic;

    [SerializeField]
    private float maryCloseMusicDistance;

    [SerializeField]
    private AudioSource fallenCloseMusic;

    [SerializeField]
    private AudioSource fallenCloseSound;

    [SerializeField]
    private float fallenAmbientMusicDistance;

    [SerializeField]
    private float fallenCloseMusicDistance;

    private IEnumerator phantomDetectionRoutine;

    private IEnumerator lurkerDetectionRoutine;

    private IEnumerator maryDetectionRoutine;

    private IEnumerator fallenDetectionRoutine;

    private Transform survivorPosition;

    private int monsterID;

    private bool dead;

    private bool matchOver;

    private bool lurkerGhostTouchedRecently;


    void Start()
    {
        survivorPosition = GetComponent<Transform>();
        phantomDetectionRoutine = PhantomDetectionRoutine();
        fallenDetectionRoutine = FallenDetectionRoutine();
        lurkerDetectionRoutine = LurkerDetectionRoutine();
        maryDetectionRoutine = MaryDetectionRoutine();

    }

    private bool Detect(Transform position, float distance, string monsterTag)
    {
        bool found = false;
        //RaycastHit[] objectsHit = Physics.SphereCastAll(survivorPosition.position, distance, survivorPosition.forward, distance);

        RaycastHit[] objectsHit = Physics.SphereCastAll(position.position, distance, position.forward, distance);

        for (var i = 0; i < objectsHit.Length; i++)
        {
            GameObject hitGameObject = objectsHit[i].collider.gameObject;

            string tag = hitGameObject.tag;

            if (hitGameObject.CompareTag(monsterTag))
            {
                found = true;
                break;
            }
        }


        return found;
    }

    private IEnumerator PhantomDetectionRoutine()
    {
        while (true)
        {
            if (matchOver || dead)
            {
                StopAllMusic();
                yield break;
            }

            bool phantomClose = Detect(survivorPosition, phantomCloseMusicDistance, Tags.PHANTOM);
            bool phantomCloser = Detect(survivorPosition, phantomCloserMusicDistance, Tags.PHANTOM);
            UpdatePhantomMusic(phantomClose, phantomCloser);
            yield return new WaitForSeconds(2f);
        }
    }


    private void UpdatePhantomMusic(bool phantomClose, bool phantomCloser)
    {
        if (phantomClose && phantomCloser)
        {
            if (phantomCloseMusic.isPlaying)
            {
                phantomCloseMusic.Stop();
                phantomCloserMusic.Play();
            }

        }

        else if (phantomClose && !phantomCloser)
        {
            if (phantomCloserMusic.isPlaying)
            {
                phantomCloseMusic.Stop();
                phantomCloseMusic.Play();
            }
        }

        else
        {
            if (phantomCloseMusic.isPlaying)
            {
                phantomCloseMusic.Stop();
            }
        }
    }


    private IEnumerator LurkerDetectionRoutine()
    {
        while (true)
        {
            if (matchOver || dead)
            {
                StopAllMusic();
                yield break;
            }

            bool lurkerClose = Detect(survivorPosition, lurkerCloseMusicDistance, Tags.LURKER);
            bool lurkerCloser = Detect(survivorPosition, lurkerCloserMusicDistance, Tags.LURKER);
            bool lurkerGhostTouching = Detect(survivorPosition, lurkerGhostTouchingDistance, Tags.LURKER);
            UpdateLurkerMusic(lurkerClose, lurkerCloser, lurkerGhostTouching);
            yield return new WaitForSeconds(2f);
        }
    }

    private void UpdateLurkerMusic(bool lurkerClose, bool lurkerCloser, bool lurkerGhostTouching)
    {
        if (lurkerClose && !lurkerCloser)
        {
            if (lurkerCloserMusic.isPlaying)
            {
                lurkerCloserMusic.Stop();
            }

            lurkerCloseMusic.Play();
        }

        else if (!lurkerClose && lurkerCloser)
        {
            if (lurkerCloseMusic.isPlaying)
            {
                lurkerCloseMusic.Stop();
            }

            lurkerCloserMusic.Play();
        }

        else
        {
            if (lurkerCloseMusic.isPlaying)
            {
                lurkerCloseMusic.Stop();
            }

            if (lurkerCloserMusic.isPlaying)
            {
                lurkerCloserMusic.Stop();
            }
        }

        if (lurkerGhostTouching && !lurkerGhostTouchedRecently)
        {
            StartCoroutine(LurkerTouchSoundsCooldown());
            int randomSoundIndex = Random.Range(0, lurkerPassedThroughSurviorSounds.Length);
            lurkerPassedThroughSurviorSounds[randomSoundIndex].Play();
        }
    }


    private IEnumerator LurkerTouchSoundsCooldown()
    {
        lurkerGhostTouchedRecently = true;
        yield return new WaitForSeconds(5f);
        lurkerGhostTouchedRecently = false;
    }

    //TODO: Make an event that will call this at some point.
    private void OnMonsterSpawnInStage(int monster)
    {
        monsterID = monster;

        switch (monsterID)
        {
            case 0:
                StartCoroutine(lurkerDetectionRoutine);
                break;
            case 1:
                StartCoroutine(phantomDetectionRoutine);
                break;
            case 2:
                StartCoroutine(maryDetectionRoutine);
                break;
            case 3:
                StartCoroutine(fallenDetectionRoutine);
                break;
            default:
                break;
        }
    }

    //TODO. Get around to doing this.
    private IEnumerator FallenDetectionRoutine()
    {
        while (true)
        {
            if (matchOver || dead)
            {
                StopAllMusic();
                yield break;
            }

            bool fallenClose = Detect(survivorPosition, fallenCloseMusicDistance, Tags.FALLEN);
            UpdateFallenMusic(fallenClose);
        }

        yield return new WaitForSeconds(2f);
    }

    private void UpdateFallenMusic(bool fallenClose)
    {

    }

    //TODO. Get around to doing this.
    private IEnumerator MaryDetectionRoutine()
    {
        while (true)
        {
            if (matchOver || dead)
            {
                StopAllMusic();
                yield break;
            }

            bool maryClose = Detect(survivorPosition, maryCloseMusicDistance, Tags.MARY);
            UpdateMaryMusic(maryClose);
            yield return new WaitForSeconds(2f);
        }


    }

    private void UpdateMaryMusic(bool maryClose)
    {
        if (!maryClose)
        {
            if (maryCloseMusic.isPlaying)
            {
                maryCloseMusic.Stop();
            }
        }

        else
        {
            maryCloseMusic.Play();
        }
    }

    private void StopAllMusic()
    {
        if (maryCloseMusic.isPlaying)
        {
            maryCloseMusic.Stop();
        }

        if (lurkerCloserMusic.isPlaying)
        {
            lurkerCloserMusic.Stop();
        }

        if (lurkerCloseMusic.isPlaying)
        {
            lurkerCloseMusic.Stop();
        }

        if (phantomCloseMusic.isPlaying)
        {
            phantomCloseMusic.Stop();
        }

        if (phantomCloserMusic.isPlaying)
        {
            phantomCloserMusic.Stop();
        }

        if (fallenCloseMusic.isPlaying)
        {
            fallenCloseMusic.Stop();
        }

        if (fallenCloseSound.isPlaying)
        {
            fallenCloseSound.Stop();
        }
    }

    public void MarkDead()
    {
        dead = true;
    }

    public void MarkGameOver()
    {
        matchOver = true;
    }
}
