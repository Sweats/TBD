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

    [SerializeField]
    private Sounds gameSounds;

    void Start()
    {
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
        soundEffectVolume = PlayerPrefs.GetFloat(SOUND_EFFECT_VOLUME_KEY_STRING, soundEffectsVolumeDefault);
        musicVolume = PlayerPrefs.GetFloat(GAME_MUSIC_VOLUME_KEY_STRING, musicVolumeDefault);
        voiceChatVolume = PlayerPrefs.GetFloat(VOICE_CHAT_VOLUME_KEY_STRING, voiceChatVolumeDefault);
        voiceActivationLevel = PlayerPrefs.GetFloat(VOICE_ACTIVATION_LEVEL_KEY_STRING, voiceActivationLevelDefault);
        monsterVoicePitchVolume = PlayerPrefs.GetFloat(MONSTER_VOICE_PITCH_VOLUME_KEY_STRING, monsterPitchVolumeDefault);

        gameSounds.SetVolume(soundEffectVolume, SoundType.Effects);
        gameSounds.SetVolume(musicVolume, SoundType.Music);
        gameSounds.SetVolume(voiceChatVolume, SoundType.VoiceChat);
        gameSounds.SetVolume(voiceActivationLevel, SoundType.VoiceActivation);
        gameSounds.SetVolume(monsterVoicePitchVolume, SoundType.MonsterPitch);

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
        gameSounds.SetVolume(soundEffectVolume, SoundType.Effects);
    }


    public void OnGameMusicVolumeSliderChanged(float value)
    {
        musicVolume = value;
        gameSounds.SetVolume(musicVolume, SoundType.Music);
    }

    public void OnMonsterVoicePitchVolumeChanged(float value)
    {
        monsterVoicePitchVolume = value;
        gameSounds.SetVolume(monsterVoicePitchVolume, SoundType.MonsterPitch);

    }


    public void OnVoiceChatVolumeChanged(float value)
    {
        voiceChatVolume = value;
        gameSounds.SetVolume(voiceChatVolume, SoundType.VoiceChat);

    }

    public void OnCaptureModeFieldChanged()
    {

    }
    public void OnVoiceActivationLevelChanged(float value)
    {
        voiceActivationLevel = value;
        gameSounds.SetVolume(voiceActivationLevel, SoundType.VoiceActivation);
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

        gameSounds.SetVolume(soundEffectVolume, SoundType.Effects);
        gameSounds.SetVolume(musicVolume, SoundType.Music);
        gameSounds.SetVolume(voiceChatVolume, SoundType.VoiceChat);
        gameSounds.SetVolume(voiceActivationLevel, SoundType.VoiceActivation);

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

