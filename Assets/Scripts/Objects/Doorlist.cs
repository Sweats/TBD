using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorlist : MonoBehaviour
{

    // this object is used only to store a list of doors in a spot.
    [SerializeField]
    private Door[] doors;

    // not sure if we will need this
    public Door[] GetDoorsInStage()
    {
        return doors;
    }
}
