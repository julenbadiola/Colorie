using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using TMPro;

namespace GameSpace {
    
    public class LoadData : MonoBehaviour {
        public int total = 0;
        public int counter = 0;
        public float timePassed = 0f;
        public bool hecho = false;
        public GameObject loadingText;

        void Start () {
            loadingText.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText ("loading") + "...";
            PlayerPrefs.SetInt("error", 0);
            counter = 0;
            total = 4;
            timePassed = 0f;

            Debug.Log ("REALIZANDO EL GET UNLOCK");
            GetUnlock ();
        }

        void Update () {
            timePassed += Time.deltaTime;

            if (PlayerPrefs.GetInt ("error") == 1) {
                hecho = true;
                Debug.Log ("ERROR EN ALGUNO " + timePassed + " s.");
                SceneManagerController.goToScene ("Menu");
            } else if (total == counter && !hecho) {
                hecho = true;
                Debug.Log ("TODOS RESUELTOS " + timePassed + " s.");
                GlobalVar.firstLaunch = false;
                SceneManagerController.goToScene (PlayerPrefs.GetString("scene"));
            } else {
                //Debug.Log("ESPERANDO A LA RESPUESTA DE TODOS " + timePassed + " s: " + counter + " / " + total);
            }

        }

        public void GetStruct (string url, string[] atts, string prefKey) {
            StartCoroutine (
                GlobalVar.StartFormCoroutine (
                    value => {
                        counter += 1;
                        if (value.Key) {
                            //Si el valor es true (no ha tirado error), hace lo que sea con el response
                            Debug.Log ("RESPONSE DE UN GET" + value.Value);
                            int number;
                            bool success = Int32.TryParse(value.Value, out number);
                            if(success){
                                PlayerPrefs.SetInt(prefKey, number);
                            }
                            else
                            {
                                PlayerPrefs.SetString("errorExp", "ERROR WHILE FORMATTING");
                                PlayerPrefs.SetInt("error", 1);
                            }
                            
                        } else {
                            //si queremos hacer algo cuando ha habido error
                            Debug.Log ("Error en el response");
                            PlayerPrefs.SetString("errorExp", "ERROR RESPONSE");
                            PlayerPrefs.SetInt("error", 1);
                        }
                    },
                    url, atts
                )
            );
        }


        //Llama a la corrutina para que envíe el form
        public void GetUnlock () {
            GetStruct ("getunlock", new string[] { }, "unlock");

            /*//LOCAL TEST
            PlayerPrefs.SetInt("unlock", 2);
            counter += 1;*/
        }

        public void GetMaxStreak (string difficulty) {
            GetStruct ("getmaxstreak", new string[] { "difficulty:" + difficulty }, difficulty + "MaxStreak");

            /*//LOCAL TEST
            PlayerPrefs.SetInt("easy" + "MaxStreak", 30);
            PlayerPrefs.SetInt("medium" + "MaxStreak", 0);
            PlayerPrefs.SetInt("progress", 23);
            PlayerPrefs.SetInt("hard" + "MaxStreak", 0);
            counter += 1;*/
        }

        /////////////// TO DO

        public void GetMazeTimeAvg (string difficulty) {
            GetStruct ("getmazetimeavg", new string[] { "difficulty:" + difficulty }, "TODO");
        }

        public void GetMazeAmountResult (int result, string difficulty) {
            GetStruct ("getmazeamountresult", new string[] {
                "result:" + result,
                "difficulty:" + difficulty
            }, "TODO");
        }

        public void GetMazeTotalTime (string difficulty) {
            GetStruct ("getmazetotaltime", new string[] {
                "difficulty:" + difficulty
            }, "TODO");
        }
    }
}