using UnityEngine;

[System.Serializable]
public class Sound
{

    [SerializeField]
    private AudioSource sound;
    public SoundType soundtype;
    public void SetVolume(float volume)
    {
        sound.volume = volume;

    }

// Will be used for the monsters
    public void Enable()
    {
        sound.enabled = true;

    }


// Will be used for the monsters
    public void Disable()
    {
        sound.enabled = false;
    }
}