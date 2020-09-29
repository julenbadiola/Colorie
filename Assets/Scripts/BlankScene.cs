using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSpace {
    public class BlankScene : MonoBehaviour {
        void Start () {
            initializePrefs ();
            Communication.initialize ();
            LangDataset.initialize ();
            Communication.getBearerToken ();
            PlayerPrefs.SetInt ("error", 0);

            GlobalVar.setColors();
            SceneManagerController.ChangeSceneMenu ();
            /*
            GlobalVar.colors.Add(new ColorObject("brown", Color.brown));
            GlobalVar.colors.Add(new ColorObject("purple", Color.purple));
            GlobalVar.colors.Add(new ColorObject("orange", Color.orange));*/

        }

        public void initializePrefs () {
            
            if (!PlayerPrefs.HasKey ("EffectsLevel")) {
                PlayerPrefs.SetFloat ("EffectsLevel", 0.5f);
            }
            if (!PlayerPrefs.HasKey ("MusicLevel")) {
                PlayerPrefs.SetFloat ("MusicLevel", 0.5f);
            }
            if (!PlayerPrefs.HasKey ("Vibration")) {
                PlayerPrefs.SetString ("Vibration", "on");
            }
        }
    }

}