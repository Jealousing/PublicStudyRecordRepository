using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOption : MonoBehaviour
{
    public void BGMSoundSet(float value)
    {
        SoundManager.GetInstance.BGMSoundVolume(value);
    }

    public void SFXSoundSet(float value)
    {
        SoundManager.GetInstance.SFXSoundVolume(value);
    }
}
