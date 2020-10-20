using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GameSpace {

    public class SendData : MonoBehaviour {
        public GameObject MessageCanvas;
        public GameObject StarsImage;
        public GameObject MessageCanvasText;
        public GameObject ResultPercentText;
        public SoundPlayer soundPlayer;
        public VibrationManager vibrManager;
        public GameObject FinalMessageCanvas;
        public int gamemode = 0;
        public int total = 0;
        public int counter = 0;
        public float timePassed = 0f;
        public bool hecho = false;
        public bool messagedone = false;

        public void SetStruct (string url, string[] atts) {
            StartCoroutine (
                GlobalVar.StartFormCoroutine (
                    value => {
                        counter += 1;
                        if (value.Key) {
                            //Si el valor es true (no ha tirado error), hace lo que sea con el response
                            Debug.Log ("RESPONSE DE UN SET" + value.Value);
                        } else {
                            //si queremos hacer algo cuando ha habido error
                            Debug.Log ("Error en el response");
                            PlayerPrefs.SetString ("errorExp", "ERROR RESPONSE");
                            PlayerPrefs.SetInt ("error", 1);
                        }
                    },
                    url, atts
                )
            );
        }


        void Start () {
            vibrManager = gameObject.GetComponent<VibrationManager>();
            soundPlayer = gameObject.GetComponent<SoundPlayer> ();

            PlayerPrefs.SetInt ("error", 0);
            FinalMessageCanvas.SetActive (true);
            total = 1;
            SendScore();
        }

        void Update () {

            timePassed += Time.deltaTime;
            if (PlayerPrefs.GetInt ("error") == 1 && !hecho) {
                hecho = true;
                Debug.Log ("ERROR EN ALGUNO " + timePassed + " s.");

                if(GlobalVar.tryagain){
                    SceneManagerController.goToScene ("Menu");
                }else{
                    SceneManagerController.goToScene ("SendData");
                }
                
            } else if (total == counter && !hecho && messagedone) {
                hecho = true;
                Debug.Log ("TODOS RESUELTOS " + timePassed + " s.");
                SceneManagerController.goToScene (PlayerPrefs.GetString ("scene"));
            }
        }
        public void SendScore () {
            string[] resList = new string[] {};
            for (int i = 0; i < GlobalVar.scores.Count; i++)
            {
                resList[resList.Length] = "result"+(i+1)+":"+GlobalVar.scores[i];
            }
            print(resList);
            SetStruct ("savemazescore", resList);
        }

        public IEnumerator showMessage (string text, float res, float time) {
            messagedone = true;
            MessageCanvasText.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText (text);
            if (res < 0.5f) {
                Destroy (StarsImage);
            } else if (res <= 0.60f) {
                StarsImage.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("1stars");
            } else if (0.6f < res && res <= 0.8f) {
                StarsImage.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("2stars");
            } else {
                StarsImage.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("3stars");
            }
            MessageCanvas.SetActive (true);
            //ResultPercentText.GetComponent<TextMeshProUGUI> ().text = (res * 100).ToString() + " %";

            if (text == "gameWellDoneMessage") {
                soundPlayer.playCorrectSound ();
                vibrManager.shortVibration();
            } else if (text == "gameFailedMessage") {
                soundPlayer.playFailedSound ();
                vibrManager.longVibration();
            }
            
            
            yield return new WaitForSeconds (2);
            MessageCanvas.SetActive (false);
            

        }
    }

}