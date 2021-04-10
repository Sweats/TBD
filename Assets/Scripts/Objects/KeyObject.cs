using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Mirror;


public class KeyObject : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    private string keyName;

    [SerializeField]
    [SyncVar]
    private int mask;

    [SerializeField]
    [SyncVar]
    private KeyType type;

    [SerializeField]
    [SyncVar]
    private int pathID;

    [SyncVar]
    private bool pickedUp = false;

    [SerializeField]
    private Color groupColor;

    [SerializeField]
    private float timeBetweenGlows = 45;
    [SerializeField]
    private float glowTimerLength = 115;

    [SerializeField]
    private float maxTimerForGlow = 100;

    [SerializeField]
    private float minTimer = 0;

    [SerializeField]
    private float noGlowTimer;

    [SerializeField]
    private float glowTimer;


    [SerializeField]
    private AudioSource pickupSound;

    [SerializeField]
    private Texture keyIcon;

    [SerializeField]
    // this is the color that will appear around the key mesh to let the player know they can pick it up
    private Color glowOutlineColor = Color.red;

    [SerializeField]
    private Color glowColor = Color.white;

    [SerializeField]
    private Renderer keyRenderer;

    [SerializeField]
    private MeshCollider keyCollider;


    [Client]
    private void Start()
    {
        noGlowTimer = maxTimerForGlow;
        keyRenderer.material.SetColor("_Color", glowOutlineColor);
    }

    [Client]
    private void Update()
    {
        noGlowTimer -= Time.deltaTime * timeBetweenGlows;

        if (noGlowTimer <= minTimer)
        {
            Glow();

            glowTimer -= Time.deltaTime * glowTimerLength;
            noGlowTimer = 0;

            if (glowTimer <= minTimer)
            {
                UnGlow();
                noGlowTimer = maxTimerForGlow;
                glowTimer = maxTimerForGlow;
            }
        }

    }

    public KeyObject(string keyName, int mask, int pathID, KeyType type)
    {
        this.keyName = keyName;
        this.mask = mask;
        this.pathID = pathID;
        this.type = type;
    }

    public KeyObject()
    {

    }

    private void Glow()
    {
        keyRenderer.material.SetColor("_Color", glowColor);

    }

    private void UnGlow()
    {
        keyRenderer.material.SetColor("_Color", glowOutlineColor);

    }

    public void Pickup()
    {
        pickedUp = true;
        Hide();
    }

    public void PlayPickupSound()
    {
        pickupSound.Play();
    }

    public void Hide()
    {
        keyRenderer.enabled = false;
        keyCollider.enabled = false;
    }

    public void Show()
    {
        if (!pickedUp)
        {
            keyRenderer.enabled = true;
            keyCollider.enabled = true;
        }
    }

    public void SetName(string name)
    {
        this.keyName = name;

    }

    public void SetMask(int mask)
    {
        this.mask = mask;
    }

    public void SetType(KeyType type)
    {
        this.type = type;
    }

    public Texture Texture()
    {
        return keyIcon;
    }

    public int Mask()
    {
        return mask;
    }

    public int PathID()
    {
        return pathID;
    }

    public string Name()
    {
        return keyName;
    }

    public KeyType Type()
    {
        return type;
    }

    [Command(requiresAuthority=false)]
    public void CmdPlayerClickedOnKey(NetworkConnectionToClient sender = null)
    {
        StartCoroutine(PlayerClickedOnKeyRoutine(sender));
    }

    [Server]
    private IEnumerator PlayerClickedOnKeyRoutine(NetworkConnectionToClient sender)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();
        var keys = survivor.Items();
        bool found = false;

        for (var i = 0; i < keys.Count; i++)
        {
            Key keyInInventory = keys[i];

            if (mask == keyInInventory.Mask())
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            TargetPlayerAlreadyHasKey();
            yield break;
        }
        
        Key key = new Key(keyName, mask, pathID, type);
        keys.Add(key);

        PlayerPickedUpKeyMessage playerPickedUpKeyMessage = new PlayerPickedUpKeyMessage
        {
            playerName = survivor.Name(),
            keyName = key.Name()
        };

        RpcPlayerPickedUpKeyMessage(playerPickedUpKeyMessage);

        //NOTE: We need to skip a frame or else the RPC will never get sent! 
        //Good thing that I was able to look at the mirror examples on github...

        yield return null;
        NetworkServer.Destroy(this.gameObject);

    }

    [ClientRpc]
    private void RpcPlayerPickedUpKeyMessage(PlayerPickedUpKeyMessage serverMessage)
    {
        EventManager.playerPickedUpKeyEvent.Invoke(serverMessage.playerName, serverMessage.keyName);
    }

    [TargetRpc]
    private void TargetPlayerAlreadyHasKey()
    {
        EventManager.survivorAlreadyHasKeyEvent.Invoke();
    }
}

