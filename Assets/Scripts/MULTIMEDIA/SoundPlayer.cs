using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpace {

    public class SoundPlayer : MonoBehaviour {
        public const float DefaultVolumeLevel = 0.5f;
        public const float adjuster = 0.5f;
        [HideInInspector]
        public float mLevel;
        [HideInInspector]
        public float eLevel;

        public AudioClip buttonSound;
        public AudioClip disabledButtonSound;
        public AudioClip failed;
        public AudioClip correct;
        public GameObject backgroundMusic;
        
        private AudioSource thisAudio;
        void Start () {
            thisAudio = gameObject.GetComponent<AudioSource> ();
            refresh ();
        }

        public void refresh () {
            mLevel = PlayerPrefs.HasKey ("MusicLevel") ? PlayerPrefs.GetFloat ("MusicLevel") : DefaultVolumeLevel;
            eLevel = PlayerPrefs.HasKey ("EffectsLevel") ? PlayerPrefs.GetFloat ("EffectsLevel") : DefaultVolumeLevel;

            thisAudio.volume = eLevel * adjuster;
            try { backgroundMusic.GetComponent<AudioSource>().volume = mLevel * adjuster; } catch (System.Exception) { print ("There is no background music"); }
        }

        public void changeEffectsLevel (float value) {
            PlayerPrefs.SetFloat ("EffectsLevel", value);
            refresh ();
        }

        public void changeMusicLevel (float value) {
            PlayerPrefs.SetFloat ("MusicLevel", value);
            refresh ();
        }

        public void playButtonSound () {
            thisAudio.PlayOneShot (buttonSound, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

        public void playDisabledButtonSound () {
            thisAudio.PlayOneShot (disabledButtonSound, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

        public void playFailedSound () {
            thisAudio.PlayOneShot (failed, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

        public void playCorrectSound () {
            thisAudio.PlayOneShot (correct, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

    }
}