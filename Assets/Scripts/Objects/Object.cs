using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Object : MonoBehaviour
{

    [SerializeField]
    private bool physicsEnabled;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float mass;

    private bool grabbed;

    public enum ObjectType
    {
        None = 0,
        Key,
        Door,
        Misc
    }
    

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (gravity > 0)
        {
            rigidbody.useGravity = true;
        }
        
    }
    public void Grab()
    {
        grabbed = true;

    }

    public void Drop()
    {
        grabbed = false;
    }


    public void Handle()
    {

    }
    
}

