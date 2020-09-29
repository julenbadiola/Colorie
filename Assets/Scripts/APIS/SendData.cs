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
        public GameObject loadingText;

        public int counterma = 0;
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
            int time = 0;
            float result = 0f;
            string difficulty = "";
            string message = "";
            int max = 0;

            if (PlayerPrefs.GetString ("send") == "unlock") {
                loadingText.SetActive (true);
                total = 1;
                messagedone = true;
                SetUnlock (PlayerPrefs.GetInt ("unlock"));
            }
            if (PlayerPrefs.GetString ("send") == "all") {
                loadingText.SetActive (false);
                total = 2;

                time = Mathf.RoundToInt (PlayerPrefs.GetFloat ("resTime"));
                result = PlayerPrefs.GetFloat ("resResult");
                difficulty = PlayerPrefs.GetString ("resDifficulty");
                message = PlayerPrefs.GetString ("resMessage");
                max = PlayerPrefs.GetInt ("max");
                gamemode = PlayerPrefs.GetInt("gamemode");

                //SetUnlock (PlayerPrefs.GetInt ("unlock"));
                SetScore (time + "", result + "", difficulty, gamemode);
                if (max == 1) {
                    SetMaxStreak (difficulty);
                } else {
                    total -= 1;
                }
                StartCoroutine (showMessage (message, result, time));

            }

        }

        void Update () {

            timePassed += Time.deltaTime;
            counterma += 1;
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

        public void SetUnlock (int val) {
            SetStruct ("setunlock", new string[] { "unlock:" + val });
        }
        public void SetMaxStreak (string difficulty) {
            SetStruct ("setmaxstreak", new string[] { "difficulty:" + difficulty, "streakValue:" + PlayerPrefs.GetInt (difficulty + "MaxStreak") });
        }
        public void SetScore (string time, string result, string difficulty, int gm) {
            Debug.Log ("AQUI RESUL: " + time + ":" + result + ":" + difficulty + ":" + gm);
            SetStruct ("savemazescore", new string[] {
                "time:" + time,
                "result:" + result,
                "difficulty:" + difficulty,
                "game_mode:"+gm
            });
        }
        public void CreateMazeProfileIfNew () {
            SetStruct ("createmazeprofileifnew", new string[] { });
        }

        public IEnumerator showMessage (string text, float res, float time) {
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
            messagedone = true;

        }
    }

}