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
    [SerializeField]
    private string keyName = "Rusty Key";

    [SerializeField]
    private int mask;

    [SerializeField]
    private KeyType type;

    [SerializeField]
    private int pathID;

    private Texture keyIcon;


    public Key(Key key)
    {
        this.keyName = key.keyName;
        this.mask = key.mask;
        this.type = key.type;
        this.pathID = key.pathID;
    }

    public int Mask()
    {
        return mask;

    }


    public void SetTexture(Texture texture)
    {
	    this.keyIcon = texture;
    }

    public string Name()
    {
	    return keyName;
    }


    public int PathID()
    {
	    return pathID;
    }


    public  KeyType Type()
    {
	    return type;
    }


    public Texture Texture()
    {
	    return keyIcon;
    }
}

