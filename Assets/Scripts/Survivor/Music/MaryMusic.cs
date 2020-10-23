using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaryMusic : MonoBehaviour
{
    private Transform survivorPosition;

    [SerializeField]
    private AudioSource maryCloseMusic;

    [SerializeField]
    private AudioSource maryFarMusic;

    [SerializeField]
    private float maryCloseMusicDistance;

    [SerializeField]
    private float maryFarMusicDistance;

    void Start()
    {
        survivorPosition = GetComponent<Transform>();
        StartCoroutine(Detect());
    }


    private IEnumerator Detect()
    {
        while (true)
        {
            bool maryClose, maryFar;

            maryClose = Music.ShouldPlayMusic(survivorPosition, maryCloseMusicDistance, "Mary");
            maryFar = Music.ShouldPlayMusic(survivorPosition, maryFarMusicDistance, "Mary");

            if (maryClose && !maryFar)
            {
                if (maryFarMusic.isPlaying)
                {
                    maryFarMusic.Stop();
                }

                maryCloseMusic.Play();

            }

            else if (!maryClose && maryFar)
            {
                if (maryCloseMusic.isPlaying)
                {
                    maryCloseMusic.Stop();
                }

                maryFarMusic.Play();
            }

            else
            {
                if (maryCloseMusic.isPlaying)
                {
                    maryCloseMusic.Stop();
                }

                if (maryFarMusic.isPlaying)
                {
                    maryFarMusic.Stop();
                }
            }
        }

	yield return new WaitForSeconds(0.5f);
    }

    // TODO: Have an event call this when everyone spawns in the stage at the same time.
    private void OnMarySpawnedInStage()
    {

    }
}
