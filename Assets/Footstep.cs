using UnityEngine;

public class Footstep : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform spherePosition;

    [SerializeField]
    private AudioSource defaultWalkingSound;

    [SerializeField]
    private AudioSource defaultSprintingSound;

    private const string FLOOR_TAG = "floor";

    void Start()
    {
        spherePosition = GetComponent<Transform>();
    }

    public void OnSurvivorMove(bool sprinting)
    {
        Debug.Log($"Survivor is moving. Sprint value = {sprinting}!");

        if (sprinting)
        {
            if (defaultWalkingSound.isPlaying)
            {
                defaultWalkingSound.Stop();
            }

            defaultSprintingSound.Play();
        }


        else
        {
            if (defaultSprintingSound.isPlaying)
            {
                defaultSprintingSound.Stop();
            }

            defaultWalkingSound.Play();
        }

    }

    public void OnSurvivorStopMoving()
    {
        if (defaultSprintingSound.isPlaying)
        {
            defaultSprintingSound.Stop();

        }

        if (defaultWalkingSound.isPlaying)
        {
            defaultWalkingSound.Stop();
        }

    }


    // TO DO: Give a tiny bit of a delay to this function because we don't need to spam it every frame.
//    private void HandleFootstepSound(bool sprinting)
//    {
//        RaycastHit hit;
//
//        if (Physics.SphereCast(spherePosition.position, 0.3f, spherePosition.forward, out hit, 0.3f))
//        {
//            Collider collider = hit.collider;
//
//            if (collider.tag == FLOOR_TAG)
//            {
//                // Get the floor here
//                if (sprinting)
//                {
//                    AudioSource walkingSound = collider.GetComponent<AudioSource>();
//
//                }
//
//                else
//                {
//                    AudioSource walkingSound = collider.GetComponent<AudioSource>();
//
//                }
//            }
//
//            else
//            {
//                if (sprinting)
//                {
//                    if (defaultWalkingSound.isPlaying)
//                    {
//                        defaultWalkingSound.Stop();
//
//                    }
//
//                    defaultSprintingSound.Play();
//
//                }
//
//                else
//                {
//                    if (defaultSprintingSound.isPlaying)
//                    {
//                        defaultSprintingSound.Stop();
//                    }
//
//                    defaultWalkingSound.Play();
//                }
//            }
//        
//        }
//
    //}
}
