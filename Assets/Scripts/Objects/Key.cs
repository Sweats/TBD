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
    private string keyName;

    [SerializeField]
    private int mask;

    [SerializeField]
    private int pathID;

    [SerializeField]
    private KeyType type;

    public Key(string keyName, int mask, int pathID, KeyType type)
    {
        this.keyName = keyName;
        this.mask = mask;
        this.pathID = pathID;
        this.type = type;
    }

    public Key()
    {

    }

    public string Name()
    {
        return keyName;
    }

    public int Mask()
    {
        return mask;
    }

    public int PathID()
    {
        return pathID;
    }

    public KeyType Type()
    {
        return type;
    }
}
