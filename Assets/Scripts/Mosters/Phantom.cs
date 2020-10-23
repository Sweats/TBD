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
	if (Keybinds.GetKey(Action.MoveForward))
	{

	}

	else if (Keybinds.GetKey(Action.MoveLeft))
	{

	}

	else if (Keybinds.GetKey(Action.MoveRight))
	{

	}

	else if (Keybinds.GetKey(Action.MoveBack))
	{

	}

	else if (Keybinds.GetKey(Action.Attack))
	{
		OnAttack();
	}

    }


    private void OnAttack()
    {

    }
}
