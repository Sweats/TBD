using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Audio;

// will be used to show spectating players the insaniy effect that the player is currently seeing.
//public class InsanityEffectEvent : UnityEvent<InsanityEffect> { }

public class InsanityEffect : MonoBehaviour
{
    public bool insanityEnabled;
    public float insanityNeededToStart;

    public float timerDischargeRate;

    public float timer = 100;

    public AudioSource insanitySoundEffect;
    public InsanityEffectEvent insanityEffectEvent;

    public AudioMixer audioMixer;

    public void OnGammaInsanityEffectTriggered()
    {

    }


    public void OnDeafInsanityEffectTriggered()
    {
        StartCoroutine(AdjustVolume());
    }

    private IEnumerator AdjustVolume()
    {
        float oldSoundEffectVolume, oldMusicVolume, oldVoiceVolume;
        audioMixer.GetFloat(SoundUI.EFFECTS_SOUND_MIXER_STRING, out oldSoundEffectVolume);
        audioMixer.GetFloat(SoundUI.MUSIC_SOUND_MIXER_STRING, out oldMusicVolume);
        audioMixer.GetFloat(SoundUI.VOICE_MIXER_STRING, out oldVoiceVolume);

        audioMixer.SetFloat(SoundUI.EFFECTS_SOUND_MIXER_STRING, Mathf.Log(0.001f) * 20);
        audioMixer.SetFloat(SoundUI.MUSIC_SOUND_MIXER_STRING, Mathf.Log(0.001f) * 20);
        audioMixer.SetFloat(SoundUI.VOICE_MIXER_STRING, Mathf.Log(0.001f) * 20);

        insanitySoundEffect.Play();

        yield return new WaitForSeconds(5f);

        audioMixer.SetFloat(SoundUI.EFFECTS_SOUND_MIXER_STRING, Mathf.Log(oldSoundEffectVolume) * 20);
        audioMixer.SetFloat(SoundUI.MUSIC_SOUND_MIXER_STRING, Mathf.Log(oldMusicVolume) * 20);
        audioMixer.SetFloat(SoundUI.VOICE_MIXER_STRING, Mathf.Log(oldVoiceVolume) * 20);
    }

//
//    private IEnumerator AdjustGamma()
//    {
//
//    }
//    


    public void OnFlashlightFlickerInsanityEffectTriggered()
    {

    }


    public void OnBlackAndWhiteInsanityEffectTriggered()
    {

    }


    public void OnFakeTrapInsanityEffectTriggered()
    {

    }


    public void OnJumpScareInsanityEffectTriggered()
    {

    }

}