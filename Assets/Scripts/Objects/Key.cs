using UnityEngine;

public enum KeyType: byte
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
    private KeyType type;

    public Key(string keyName, int mask, KeyType type)
    {
        this.keyName = keyName;
        this.mask = mask;
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

    public KeyType Type()
    {
        return type;
    }
}
