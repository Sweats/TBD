using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handles the music for the survivors. Probably gonna be a lot of stuff in here.
//
public class Music : MonoBehaviour
{

    private LurkerMusic lurkerMusic;

    private PhantomMusic phantomMusic;

    private MaryMusic maryMusic;

    private FallenMusic fallenMusic;


    void Start()
    {
        EventManager.monsterSpawnedInStageEvent.AddListener(OnMonsterSpawnInStage);
    }

    private void OnMonsterSpawnInStage(int monster)
    {
        switch (monster)
        {
            case 0:
                lurkerMusic.Begin();
                break;
            case 1:
                phantomMusic.Begin();
                break;
            case 2:
                maryMusic.Begin();
                break;
            case 3:
                fallenMusic.Begin();
                break;
            default:
                break;
        }
    }

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
