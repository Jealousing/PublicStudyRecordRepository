using UnityEngine;
using UnityEngine.UI;

namespace SuikaGame3D
{
    public class SuikaGameOption : MonoBehaviour
    {
        [SerializeField] Slider masterSoundSlider;
        [SerializeField] Slider bgmSoundSlider;
        [SerializeField] Slider sfxSoundSlider;
        [SerializeField] Slider mouseSensitivitySlider;
        [SerializeField] Slider keyboardSensitivitySlider;

        private const float DefaultValueForMasterSound = 0.2f;
        private const float DefaultValueForBgmSound = 0.2f;
        private const float DefaultValueForSfxSound = 0.2f;
        private const float DefaultValueForMouseSensitivity = 10f;
        private const float DefaultValueForKeyboardSensitivity = 10f;

        private void Start()
        {
            InitializeSliders();
            UpdateSettings();
        }

        private void OnEnable()
        {
            // 불러오기
            InitializeSliders();
        }
        private void OnDisable()
        {
            SaveSliderValues();
        }

        // 초기화 및 불러오기
        private void InitializeSliders()
        {
            masterSoundSlider.value = PlayerPrefs.GetFloat("MasterSound", DefaultValueForMasterSound);
            bgmSoundSlider.value = PlayerPrefs.GetFloat("BgmSound", DefaultValueForBgmSound);
            sfxSoundSlider.value = PlayerPrefs.GetFloat("SfxSound", DefaultValueForSfxSound);
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", DefaultValueForMouseSensitivity);
            keyboardSensitivitySlider.value = PlayerPrefs.GetFloat("KeyboardSensitivity", DefaultValueForKeyboardSensitivity);
        }

        // 저장
        private void SaveSliderValues()
        {
            PlayerPrefs.SetFloat("MasterSound", masterSoundSlider.value);
            PlayerPrefs.SetFloat("BgmSound", bgmSoundSlider.value);
            PlayerPrefs.SetFloat("SfxSound", sfxSoundSlider.value);
            PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivitySlider.value);
            PlayerPrefs.SetFloat("KeyboardSensitivity", keyboardSensitivitySlider.value);
        }

        private void UpdateSettings()
        {
            MasterSoundSet(masterSoundSlider.value);
            BgmSoundSet(bgmSoundSlider.value);
            SfxSoundSet(sfxSoundSlider.value);
            MouseSensitivitySet(mouseSensitivitySlider.value);
            KeyboardSensitivitySet(keyboardSensitivitySlider.value);
        }

        // 설정
        public void MasterSoundSet(float value)
        {
            SoundManager.GetInstance.MasterSoundVolume(value);
        }
        public void BgmSoundSet(float value)
        {
            SoundManager.GetInstance.BGMSoundVolume(value);
        }
        public void SfxSoundSet(float value)
        {
            SoundManager.GetInstance.SFXSoundVolume(value);
        }
        public void MouseSensitivitySet(float value)
        {
            SuikaGameController.GetInstance.sensitivity =  value*0.5f;
        }
        public void KeyboardSensitivitySet(float value)
        {
            SuikaGameController.GetInstance.movementSpeed = value * 0.5f;
        }
    }

}