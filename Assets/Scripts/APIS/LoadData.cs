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
            total = 1;
            timePassed = 0f;

            Debug.Log ("REALIZANDO EL GET PROFILE");
            GetProfile ();
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
                SceneManagerController.ChangeSceneDataLoad();
                
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

        public void GetStruct (string url, string[] atts, string prefKey) {
            StartCoroutine (
                GlobalVar.StartFormCoroutine (
                    value => {
                        counter += 1;
                        if (value.Key) {
                            //Si el valor es true (no ha tirado error), hace lo que sea con el response
                            Debug.Log ("RESPONSE DE UN GET" + value.Value);
                            PlayerPrefs.SetString(prefKey, value.Value);
                            
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

        public void GetProfile () {
            GetStruct ("getcolorieprofile", new string[] { }, "profileResponse");
        }
    }
}