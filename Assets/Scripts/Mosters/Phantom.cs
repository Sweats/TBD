using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    [SerializeField]
    private float speed;


    [SerializeField]
    private float attackDistance;

    private CharacterController phantomController;

    [SerializeField]
    private AudioSource ambientMusic;

    [SerializeField]
    private AudioSource attacSound;




    void Start()
    {
	    phantomController = GetComponent<CharacterController>();

    }

    void Update()
    {
	if (Keybinds.GetKey(Actions.MoveForward))
	{

	}

	else if (Keybinds.GetKey(Actions.MoveLeft))
	{

	}

	else if (Keybinds.GetKey(Actions.MoveRight))
	{

	}

	else if (Keybinds.GetKey(Actions.MoveBack))
	{

	}

	else if (Keybinds.GetKey(Actions.Attack))
	{
		OnAttack();
	}

    }


    private void OnAttack()
    {

    }
}
