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

        void Start () {
            refresh ();
        }

        public void refresh () {
            mLevel = PlayerPrefs.HasKey ("MusicLevel") ? PlayerPrefs.GetFloat ("MusicLevel") : DefaultVolumeLevel;
            eLevel = PlayerPrefs.HasKey ("EffectsLevel") ? PlayerPrefs.GetFloat ("EffectsLevel") : DefaultVolumeLevel;

            gameObject.GetComponent<AudioSource> ().volume = eLevel * adjuster;
            try { backgroundMusic.GetComponent<AudioSource> ().volume = mLevel * adjuster; } catch (System.Exception) { print ("There is no background music"); }
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
            gameObject.GetComponent<AudioSource> ().PlayOneShot (buttonSound, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

        public void playDisabledButtonSound () {
            gameObject.GetComponent<AudioSource> ().PlayOneShot (disabledButtonSound, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

        public void playFailedSound () {
            gameObject.GetComponent<AudioSource> ().PlayOneShot (failed, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

        public void playCorrectSound () {
            gameObject.GetComponent<AudioSource> ().PlayOneShot (correct, PlayerPrefs.GetFloat ("EffectsLevel"));
        }

    }
}