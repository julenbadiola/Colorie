using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

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
            timePassed = 0f;

            Debug.Log ("REALIZANDO LOS GETS");

            total = 7;

            GetProfile ();
            GlobalVar.InitScoreSummary();
            GetScoreSummary(0);
            GetScoreSummary(1);
            GetScoreSummary(2);
            GetScoreSummary(3);
            GetScoreSummary(4);
            GetScoreSummary(5);
        }

        void Update () {
            timePassed += Time.deltaTime;

            if (PlayerPrefs.GetInt ("error") == 1) {
                hecho = true;
                Debug.Log ("ERROR EN ALGUNO " + timePassed + " s.");
                GlobalVar.error += 1;
                if(GlobalVar.error > 10){
                    SceneManagerController.goToScene ("Menu");
                }
                SceneManagerController.ChangeSceneDataLoad(PlayerPrefs.GetString("scene"));
                
            } else if (total == counter && !hecho) {
                hecho = true;
                Debug.Log ("TODOS RESUELTOS " + timePassed + " s.");
                GlobalVar.launch = false;
                
                GlobalVar.error = 0;
                PlayerPrefs.SetInt ("error", 0);
                PlayerPrefs.SetString ("errorExp", "");
                
                SceneManagerController.goToScene (PlayerPrefs.GetString("scene"));
            } else {
                //Debug.Log("ESPERANDO A LA RESPUESTA DE TODOS " + timePassed + " s: " + counter + " / " + total);
            }

        }
        public void GetProfile () 
        {
            StartCoroutine (
                GlobalVar.StartFormCoroutine (
                    value => {
                        if (value.Key) {
                            Debug.Log ("RESPONSE DE GETPROFILE: " + value.Value);
                            GlobalVar.CreatePlayerInfoFromJSON(value.Value);                      
                        } 
                        else {
                            Debug.Log ("ERROR EN EL RESPONSE DE GETPROFILE");
                            PlayerPrefs.SetString("errorExp", "ERROR RESPONSE");
                            PlayerPrefs.SetInt("error", 1);
                        }
                        counter += 1;
                    },
                    "getcolorieprofile", new string[] { }
                )
            );
        }
        public void GetScoreSummary (int gamemode) 
        {
            StartCoroutine (
                GlobalVar.StartFormCoroutine (
                    value => {
                        if (value.Key) {
                            //Si el valor es true (no ha tirado error), hace lo que sea con el response
                            Debug.Log ("RESPONSE DE GETSUMMARY " + gamemode.ToString() + ": " + value.Value);
                            GlobalVar.SetScoreSummary(gamemode, JsonConvert.DeserializeObject<Dictionary<string, string>>(value.Value.Replace(@"\", "").Trim('"')));                       
                        } 
                        else {
                            //si queremos hacer algo cuando ha habido error
                            Debug.Log ("ERROR EN EL RESPONSE DE GETSUMMARY " + gamemode.ToString());
                            PlayerPrefs.SetString("errorExp", "ERROR RESPONSE");
                            PlayerPrefs.SetInt("error", 1);
                        }
                        counter += 1;
                    },
                    "getscoresummary", new string[] { "gameMode:" + gamemode }
                )
            );
        }

        
    }
}