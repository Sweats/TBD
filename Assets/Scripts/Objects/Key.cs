using UnityEngine;

[System.Serializable]
public class Key
{
    public enum KeyType : int
    {
        Rusty = 0,
        Metal,
        Old,
        Silver,
        Code,
        Hammer,
        Crowbar
    }
    public string name = "Rusty Key";
    public int mask;

    public KeyType type;

    public int group;
    public AudioSource pickupSound;
    public Texture textureIcon;
    
    public Key(Key key)
    {
        this.name = key.name;
        this.mask = key.mask;
        this.type = key.type;
        this.group = key.group;
        this.pickupSound = key.pickupSound;
        this.textureIcon = key.textureIcon;
    }
}

