using UnityEngine;

    public enum KeyType
    {
        Rusty = 0,
        Metal,
        Old,
        Silver,
        Code,
        Hammer,
        Crowbar
    }

 [System.Serializable]
public class Key
{
    public string keyName = "Rusty Key";
    public int mask;
    public KeyType type;

    public int group;

    [SerializeField]
    private AudioSource pickupSound;
    public Texture textureIcon;
    
    public Key(Key key)
    {
        this.keyName = key.keyName;
        this.mask = key.mask;
        this.type = key.type;
        this.group = key.group;
        this.pickupSound = key.pickupSound;
        this.textureIcon = key.textureIcon;
    }


    public void PlayPickupSound()
    {
        pickupSound.Play();
    }
}

