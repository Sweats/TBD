using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    [SerializeField]
    private AudioSource[] survivorSounds;

    public enum SurvivorSound
    {
        None = 0,
        Walking,
        Sprinting,
        FlashlightClick,
        MaleGasp
    }

    public void SetVolume(float volume)
    {
        for (var i  = 0; i < survivorSounds.Length; i++)
        {
            survivorSounds[i].volume = volume;
        }
    }
}
