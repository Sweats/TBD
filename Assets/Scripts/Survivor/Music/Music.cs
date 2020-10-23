using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the music for the survivors. Probably gonna be a lot of stuff in here.
//
public class Music : MonoBehaviour
{
    [SerializeField]
    private AudioSource fallenAmbientMusic;

    [SerializeField]
    private AudioSource fallenCloseMusic;

    [SerializeField]
    private AudioSource fallenAmbientMusicDistance;

    [SerializeField]
    private AudioSource fallenCloseMusicDistance;

    public static bool ShouldPlayMusic(Transform position, float distance, string monsterTag)
    {
        bool found = false;
        //RaycastHit[] objectsHit = Physics.SphereCastAll(survivorPosition.position, distance, survivorPosition.forward, distance);

	RaycastHit[] objectsHit = Physics.SphereCastAll(position.position, distance, position.forward, distance);

        for (var i = 0; i < objectsHit.Length; i++)
        {
            GameObject hitGameObject = objectsHit[i].collider.gameObject;

            string tag = hitGameObject.tag;

            if (tag == monsterTag)
            {
                found = true;
                break;
            }
        }


        return found;
    }
}
