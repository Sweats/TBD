using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
//using System.Math;

public class SoundUI : MonoBehaviour
{

    [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField]
    private Slider musicVolumeSlider;

    [SerializeField]
    private Slider gameEffectsSlider;

    [SerializeField]
    private Slider voiceChatVolumeSlider;

    [SerializeField]
    private Slider voiceChatMonsterPitchSlider;

    [SerializeField]
    private UnityEngine.UIElements.DropdownMenu captureModeDropdown;

    [SerializeField]
    private Slider voiceActivationLevelSlider;

    [SerializeField]
    private Canvas soundMenu;

    [SerializeField]
    private OptionsUI optionsUI;

    [SerializeField]
    private Color buttonColor;


    [SerializeField]
    private float masterVolumeDefault;
    [SerializeField]
    private float soundEffectsVolumeDefault;
    [SerializeField]
    private float musicVolumeDefault;
    [SerializeField]
    private float voiceChatVolumeDefault;
    [SerializeField]
    private float monsterPitchVolumeDefault;
    [SerializeField]
    private float voiceActivationLevelDefault;


    private float soundEffectVolume;
    private float musicVolume;
    private float voiceChatVolume;
    private float monsterVoicePitchVolume;
    private float voiceActivationLevel;
    private float masterVolume;


    private const string MASTER_VOLUME_KEY_STRING = "master_volume";
    private const string SOUND_EFFECT_VOLUME_KEY_STRING = "sound_effect_volume";
    private const string GAME_MUSIC_VOLUME_KEY_STRING = "music_volume";
    private const string VOICE_CHAT_VOLUME_KEY_STRING = "voice_chat_volume";
    private const string MONSTER_VOICE_PITCH_VOLUME_KEY_STRING = "monster_voice_pitch_volume";
    private const string VOICE_ACTIVATION_LEVEL_KEY_STRING = "voice_activation_level";

    public static string MASTER_SOUND_MIXER_STRING = "Master";
    public static string EFFECTS_SOUND_MIXER_STRING = "Effects";
    public static string MUSIC_SOUND_MIXER_STRING = "Music";
    public static string MONSTER_PITCH_MIXER_STRING = "Monster Pitch";
    public static string VOICE_ACTIVATION_MIXER_STRING = "Voice Activation Level";
    public static string VOICE_MIXER_STRING = "Voice";

    public static string INSANITY_MIXER_STRING = "Insanity";


    public AudioMixer audioMixer;

    private void Start()
    {
        this.enabled = false;
        LoadSoundsFromConfig();
    }


    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            Hide();
            optionsUI.Show();
        }
    }

    public void Show()
    {
        this.enabled = true;
        soundMenu.enabled = true;

    }



    public void Hide()
    {
        SaveSoundConfig();
        soundMenu.enabled = false;
        this.enabled = false;

    }

    public void OnSoundBackButtonClicked()
    {
        Hide();
        optionsUI.Show();
    }


    public void OnMasterVolumeSliderChanged(float value)
    {
        masterVolume = value;
        audioMixer.SetFloat(MASTER_SOUND_MIXER_STRING, Mathf.Log(masterVolume) * 20);
    }


    private void LoadSoundsFromConfig()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY_STRING, masterVolumeDefault); 
        soundEffectVolume = PlayerPrefs.GetFloat(SOUND_EFFECT_VOLUME_KEY_STRING, soundEffectsVolumeDefault);
        musicVolume = PlayerPrefs.GetFloat(GAME_MUSIC_VOLUME_KEY_STRING, musicVolumeDefault);
        voiceChatVolume = PlayerPrefs.GetFloat(VOICE_CHAT_VOLUME_KEY_STRING, voiceChatVolumeDefault);
        voiceActivationLevel = PlayerPrefs.GetFloat(VOICE_ACTIVATION_LEVEL_KEY_STRING, voiceActivationLevelDefault);
        monsterVoicePitchVolume = PlayerPrefs.GetFloat(MONSTER_VOICE_PITCH_VOLUME_KEY_STRING, monsterPitchVolumeDefault);
        
        audioMixer.SetFloat(MASTER_SOUND_MIXER_STRING, Mathf.Log(masterVolume) * 20);
        audioMixer.SetFloat(EFFECTS_SOUND_MIXER_STRING, Mathf.Log(soundEffectVolume) * 20);
        audioMixer.SetFloat(MUSIC_SOUND_MIXER_STRING, Mathf.Log(musicVolume) * 20);
        audioMixer.SetFloat(VOICE_MIXER_STRING, Mathf.Log(voiceChatVolume) * 20);
        audioMixer.SetFloat(VOICE_ACTIVATION_MIXER_STRING, Mathf.Log(voiceActivationLevel) * 20);
        audioMixer.SetFloat(MONSTER_PITCH_MIXER_STRING, Mathf.Log(monsterVoicePitchVolume) * 20);
        audioMixer.SetFloat(INSANITY_MIXER_STRING, Mathf.Log(soundEffectVolume) * 20);

//        gameSounds.SetVolume(soundEffectVolume, SoundType.Effects);
//        gameSounds.SetVolume(musicVolume, SoundType.Music);
//        gameSounds.SetVolume(voiceChatVolume, SoundType.VoiceChat);
//        gameSounds.SetVolume(voiceActivationLevel, SoundType.VoiceActivation);
//        gameSounds.SetVolume(monsterVoicePitchVolume, SoundType.MonsterPitch);
//
        UpdateUI();
    }

    private void UpdateUI()
    {
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        gameEffectsSlider.value = soundEffectVolume;
        voiceActivationLevelSlider.value = voiceActivationLevel;
        voiceChatVolumeSlider.value = voiceChatVolume;
        voiceChatMonsterPitchSlider.value = monsterVoicePitchVolume;
    }

    public void OnGameEffectsVolumeSliderChanged(float value)
    {
        soundEffectVolume = value;
        audioMixer.SetFloat(EFFECTS_SOUND_MIXER_STRING, Mathf.Log(soundEffectVolume) * 20);
        audioMixer.SetFloat(INSANITY_MIXER_STRING, Mathf.Log(soundEffectVolume) * 20);
        //gameSounds.SetVolume(soundEffectVolume, SoundType.Effects);
    }


    public void OnGameMusicVolumeSliderChanged(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat(MUSIC_SOUND_MIXER_STRING, Mathf.Log(musicVolume) * 20);
        //gameSounds.SetVolume(musicVolume, SoundType.Music);
    }

    public void OnMonsterVoicePitchVolumeChanged(float value)
    {
        monsterVoicePitchVolume = value;
        audioMixer.SetFloat(MONSTER_PITCH_MIXER_STRING, Mathf.Log(monsterVoicePitchVolume) * 20);
        //gameSounds.SetVolume(monsterVoicePitchVolume, SoundType.MonsterPitch);

    }


    public void OnVoiceChatVolumeChanged(float value)
    {
        voiceChatVolume = value;
        audioMixer.SetFloat(VOICE_MIXER_STRING, Mathf.Log(voiceChatVolume) * 20);
        //gameSounds.SetVolume(voiceChatVolume, SoundType.VoiceChat);

    }

    public void OnCaptureModeFieldChanged()
    {

    }

    public void OnVoiceActivationLevelChanged(float value)
    {
        voiceActivationLevel = value;
        audioMixer.SetFloat(VOICE_ACTIVATION_MIXER_STRING, Mathf.Log(voiceActivationLevel) * 20);
        //gameSounds.SetVolume(voiceActivationLevel, SoundType.VoiceActivation);
    }

    public void OnDefaultsButtonClicked()
    {
        masterVolume = masterVolumeDefault;
        voiceChatVolume = voiceChatVolumeDefault;
        musicVolume = musicVolumeDefault;
        monsterVoicePitchVolume = monsterPitchVolumeDefault;
        voiceActivationLevel = voiceActivationLevelDefault;
        soundEffectVolume = soundEffectsVolumeDefault;

        audioMixer.SetFloat(MASTER_SOUND_MIXER_STRING, Mathf.Log(masterVolume) * 20);
        audioMixer.SetFloat(EFFECTS_SOUND_MIXER_STRING, Mathf.Log(soundEffectVolume) * 20);
        audioMixer.SetFloat(MUSIC_SOUND_MIXER_STRING, Mathf.Log(musicVolume) * 20);
        audioMixer.SetFloat(VOICE_MIXER_STRING, Mathf.Log(voiceChatVolume) * 20);
        audioMixer.SetFloat(VOICE_ACTIVATION_MIXER_STRING, Mathf.Log(voiceActivationLevel) * 20);
        audioMixer.SetFloat(MONSTER_PITCH_MIXER_STRING, Mathf.Log(monsterVoicePitchVolume) * 20);
        audioMixer.SetFloat(INSANITY_MIXER_STRING, Mathf.Log(soundEffectVolume) * 20);


        //gameSounds.SetVolume(soundEffectVolume, SoundType.Effects);
        //gameSounds.SetVolume(musicVolume, SoundType.Music);
        //gameSounds.SetVolume(voiceChatVolume, SoundType.VoiceChat);
        //gameSounds.SetVolume(voiceActivationLevel, SoundType.VoiceActivation);

        UpdateUI();
    }

    private void SaveSoundConfig()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY_STRING, masterVolume);
        PlayerPrefs.SetFloat(SOUND_EFFECT_VOLUME_KEY_STRING, soundEffectVolume);
        PlayerPrefs.SetFloat(GAME_MUSIC_VOLUME_KEY_STRING, musicVolume);
        PlayerPrefs.SetFloat(VOICE_CHAT_VOLUME_KEY_STRING, voiceChatVolume);
        PlayerPrefs.SetFloat(MONSTER_VOICE_PITCH_VOLUME_KEY_STRING, monsterVoicePitchVolume);
        PlayerPrefs.SetFloat(VOICE_ACTIVATION_LEVEL_KEY_STRING, voiceActivationLevel);
        PlayerPrefs.Save();

    }


}

