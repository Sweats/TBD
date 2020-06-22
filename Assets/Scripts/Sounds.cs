using UnityEngine;

public enum SoundType
{
    Music,
    Effects,
    VoiceChat,
    VoiceActivation,
    MonsterPitch,

}

public class Sounds : MonoBehaviour
{

    [SerializeField]
    private Sound[] sounds;

    public void SetVolume(float volume, SoundType type)
    {
        for (var i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];

            if (sound.soundtype == type)
            {
                sound.SetVolume(volume);
            }
        }
    }
}
