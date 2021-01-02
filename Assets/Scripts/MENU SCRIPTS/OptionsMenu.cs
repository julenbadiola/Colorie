using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {
    public class OptionsMenu : MonoBehaviour {
        public GameObject OptionsText;
        public Button BackButton;

        public GameObject MusicVolumeText;
        public Slider musicSlider;
        public Slider effectsSlider;
        public GameObject EffectsVolumeText;

        public GameObject vibrationText;
        public Button vibrationButton;

        private SoundPlayer sounder;
        private VibrationManager vibrator;

        void Start () {
            MusicVolumeText.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("musicvolume");
            EffectsVolumeText.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("effectsvolume");
            OptionsText.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText ("options");
            BackButton.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("back");
            vibrationText.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText ("vibration");
            
            sounder = GameObject.Find("FeedbackManager").GetComponent<SoundPlayer>();
            vibrator = GameObject.Find("FeedbackManager").GetComponent<VibrationManager>();
            colorVibration();
            refreshAudio ();
        }

        public void refreshAudio () {
            musicSlider.normalizedValue = sounder.mLevel;
            effectsSlider.normalizedValue = sounder.eLevel;
        }

        public void setAudioMusic (Slider slider) {
            sounder.changeMusicLevel (slider.value);
            refreshAudio ();
        }
        public void setAudioEffects (Slider slider) {
            sounder.changeEffectsLevel (slider.value);
            refreshAudio ();
        }
        
        public void refreshVibration () {
            vibrator.change ();
            colorVibration();
            
        }
        public void colorVibration(){
            if (vibrator.vibration == "off") {
                vibrationButton.GetComponent<Image> ().color = Color.red;
                vibrationButton.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("off");
                vibrationButton.GetComponentInChildren<TextMeshProUGUI> ().color = Color.white;

            } else {
                vibrationButton.GetComponent<Image> ().color = Color.green;
                vibrationButton.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("on");
                vibrationButton.GetComponentInChildren<TextMeshProUGUI> ().color = Color.black;
            }
        }

    }
}