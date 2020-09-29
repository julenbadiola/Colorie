using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LangDataset;

namespace GameSpace {
    public class TutorialFindpath : MonoBehaviour {
        public GameObject skipButtonText;

        public GameObject JoinText;
        public GameObject ObstaclesText;
        public GameObject PathText;

        public GameObject JoinNextText;
        public GameObject ObstaclesNextText;
        public GameObject PathNextText;
        private int unlock;

        // Start is called before the first frame update
        void Start () {
            gameObject.GetComponent<AudioSource> ().volume = PlayerPrefs.GetFloat ("MusicLevel");
            unlock = PlayerPrefs.GetInt ("unlock");

            setText (skipButtonText, "skip");
            setText (JoinText, "tutFindpath1");
            setText (ObstaclesText, "tutFindpath2");
            setText (PathText, "tutFindpath3");
            setText (JoinNextText, "next");
            setText (ObstaclesNextText, "next");
            setText (PathNextText, "finish");
        }

        public void setText (GameObject obj, string txt) {
            try {
                obj.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText (txt);
            } catch (System.Exception) {
                print ("FALLO AL INTENTAR TRADUCIR " + txt + "PARA: " + obj);
            }
        }

        public void finish () {
            print ("FINISH() TUTORIAL=" + unlock);
            
        }
    }
}