using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{

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
    private AudioSource walkingSound;

    [SerializeField]
    private AudioSource sprintingSound;

    [SerializeField]
    private AudioSource flashlightToggleSound;

    [SerializeField]
    private AudioSource trapSound;

    private AudioSource[] soundEffects;

    [SerializeField]
    private AudioSource ambience;

    [SerializeField]
    private Canvas soundMenu;

    [SerializeField]
    private Color buttonColor;


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


    private const string SOUND_EFFECT_VOLUME_KEY_STRING = "sound_effect_volume";
    private const string GAME_MUSIC_VOLUME_KEY_STRING = "music_volume";
    private const string VOICE_CHAT_VOLUME_KEY_STRING = "voice_chat_volume";
    private const string MONSTER_VOICE_PITCH_VOLUME_KEY_STRING = "monster_voice_pitch_volume";
    private const string VOICE_ACTIVATION_LEVEL_KEY_STRING = "voice_activation_level";

    void Start()
    {
        soundEffects = new AudioSource[]
        {
                walkingSound,
                sprintingSound,
                flashlightToggleSound,
                trapSound,
        };

        LoadSoundsFromConfig();
    }



    public void Show()
    {
        soundMenu.enabled = true;

    }


    public void Hide()
    {
        SaveSoundConfig();
        soundMenu.enabled = false;

    }


    public void OnMasterVolumeSliderChanged(float value)
    {

    }


    private void LoadSoundsFromConfig()
    {
        soundEffectVolume = PlayerPrefs.GetFloat(SOUND_EFFECT_VOLUME_KEY_STRING, 1.0f);
        musicVolume = PlayerPrefs.GetFloat(GAME_MUSIC_VOLUME_KEY_STRING, 1.0f);
        voiceChatVolume = PlayerPrefs.GetFloat(VOICE_CHAT_VOLUME_KEY_STRING, 1.0f);
        voiceActivationLevel = PlayerPrefs.GetFloat(VOICE_ACTIVATION_LEVEL_KEY_STRING, 0.5f);
        monsterVoicePitchVolume = PlayerPrefs.GetFloat(MONSTER_VOICE_PITCH_VOLUME_KEY_STRING, 0.5f);

        foreach (AudioSource sound in soundEffects)
        {
            sound.volume = soundEffectVolume;
        }

        ambience.volume = musicVolume;

        UpdateUI();

    }

    private void UpdateUI()
    {
        musicVolumeSlider.value = musicVolume;
        gameEffectsSlider.value = soundEffectVolume;
        voiceActivationLevelSlider.value = voiceActivationLevel;
        voiceChatVolumeSlider.value = voiceChatVolume;
        voiceChatMonsterPitchSlider.value = monsterVoicePitchVolume;
    }




    public void OnGameEffectsVolumeSliderChanged(float value)
    {
        soundEffectVolume = value;
        for (var i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].volume = value;
        }
    }


    public void OnGameMusicVolumeSliderChanged(float value)
    {
        musicVolume = value;
        ambience.volume = value;

    }

    public void OnMonsterVoicePitchVolumeChanged(float value)
    {
        monsterVoicePitchVolume = value;

    }


    public void OnVoiceChatVolumeChanged(float value)
    {
        voiceChatVolume = value;

    }

    public void OnCaptureModeFieldChanged()
    {

    }
    public void OnVoiceActivationLevelChanged(float value)
    {
        voiceActivationLevel = value;
    }

    public void OnMoveEnterButton(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();
        text.color = buttonColor;

    }

    public void OnMouseLeftButton(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();
        text.color = Color.white;

    }

    public void OnDefaultsButtonClicked()
    {
        voiceChatVolume = voiceChatVolumeDefault;
        musicVolume = musicVolumeDefault;
        monsterVoicePitchVolume = monsterPitchVolumeDefault;
        voiceActivationLevel = voiceActivationLevelDefault;
        soundEffectVolume = soundEffectsVolumeDefault;

        for (var i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].volume = soundEffectsVolumeDefault;
        }

        UpdateUI();
    }

    private void SaveSoundConfig()
    {
        PlayerPrefs.SetFloat(SOUND_EFFECT_VOLUME_KEY_STRING, soundEffectVolume);
        PlayerPrefs.SetFloat(GAME_MUSIC_VOLUME_KEY_STRING, musicVolume);
        PlayerPrefs.SetFloat(VOICE_CHAT_VOLUME_KEY_STRING, voiceChatVolume);
        PlayerPrefs.SetFloat(MONSTER_VOICE_PITCH_VOLUME_KEY_STRING, monsterVoicePitchVolume);
        PlayerPrefs.SetFloat(VOICE_ACTIVATION_LEVEL_KEY_STRING, voiceActivationLevel);
        PlayerPrefs.Save();

    }
}

