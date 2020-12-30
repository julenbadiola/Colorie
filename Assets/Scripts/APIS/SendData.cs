using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace GameSpace {

    public class SendData : MonoBehaviour {
        [SerializeField]
        private GameObject MessageCanvas;
        [SerializeField]
        private TextMeshProUGUI MessagePercentText;
        [SerializeField]
        private TextMeshProUGUI LoadingText;
        [SerializeField]
        private TextMeshProUGUI MessageText1;
        [SerializeField]
        private TextMeshProUGUI MessageText2;
        [SerializeField]
        private TextMeshProUGUI MenuButtonText;
        [SerializeField]
        private TextMeshProUGUI PlayAgainButtonText;
        private SoundPlayer soundPlayer;
        private VibrationManager vibrManager;
        private int total = 0;
        private int counter = 0;
        private float timePassed = 0f;
        private bool hecho = false;
        private int totalScore = 0;

        void Start () {
            vibrManager = gameObject.GetComponent<VibrationManager>();
            soundPlayer = gameObject.GetComponent<SoundPlayer> ();

            PlayerPrefs.SetInt ("error", 0);
            total = 1;
            SendScore();
            LoadingText.text = LangDataset.getText ("loading") + "...";
            LoadingText.gameObject.SetActive(true);
        }

        void Update () {

            timePassed += Time.deltaTime;
            if (PlayerPrefs.GetInt ("error") == 1 && !hecho) {
                hecho = true;
                Debug.Log ("ERROR EN ALGUNO " + timePassed + " s.");

                if(GlobalVar.tryagain){
                    GlobalVar.SaveScoresLocally();
                    SceneManagerController.ChangeSceneDataLoad ("Menu");
                }else{
                    SceneManagerController.goToScene ("SendData");
                }
                
            } else if (total == counter && !hecho) {
                hecho = true;
                Debug.Log ("TODOS RESUELTOS " + timePassed + " s.");
                LoadingText.gameObject.SetActive(false);
                
                setTextsMessage();
                GlobalVar.SaveScoresLocally();
                MessageCanvas.SetActive(true);
            }
        }
        private void setTextsMessage()
        {
            MessageText1.text = LangDataset.getText ("ingeneral") + " " + LangDataset.getText ("message1");
            MessageText2.text =  LangDataset.getText ("message2") + " " + GlobalVar.getPlaysNumber(0).ToString() + " " + LangDataset.getText ("message3");
            MessagePercentText.text = GlobalVar.getPercent(0, totalScore).ToString() + "%";
            PlayAgainButtonText.text = LangDataset.getText ("playagain");
            MenuButtonText.text = LangDataset.getText ("menu");
        }
        private void SendScore () {
            string[] resList = new string[GlobalVar.games];
            for (int i = 0; i < GlobalVar.scores.Count; i++)
            {
                int sc = GlobalVar.scores[i+1];
                Debug.Log("INTRODUCIENDO " + "result"+(i+1)+":"+sc);
                resList[i] = "result" + (i+1) + ":" + sc;
                totalScore += sc;
            }
            //SceneManagerController.goToScene ("Menu");
            SetStruct("addcoloriescore", resList);
        }

        public void OnClick_PlayAgain(){
            GlobalVar.resetStartGame();
        }

        public void OnClick_Menu(){
            SceneManagerController.ChangeSceneMenu();
        }

        private void SetStruct (string url, string[] atts) {
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
    }

}