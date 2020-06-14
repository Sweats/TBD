using UnityEngine;

[System.Serializable]
public class Key
{
    public string name = "Rusty Key";
    public int mask;

    public int Id;

    public int group;
    public AudioSource pickupSound;
    public Texture textureIcon;
    
    public Key(Key key)
    {
        this.name = key.name;
        this.mask = key.mask;
        this.Id = key.Id;
        this.group = key.group;
        this.pickupSound = key.pickupSound;
        this.textureIcon = key.textureIcon;
    }
}

