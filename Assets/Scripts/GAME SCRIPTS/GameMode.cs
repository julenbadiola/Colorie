using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class ColorObject {
        public string Name { get; set; }
        public Color Color { get; set; }
        public Color TextColor { get; set; }

        public ColorObject (string name, Color color, Color text) {
            Name = name;
            Color = color;
            TextColor = text;
        }
    }

    public class GameMode : MonoBehaviour {
        public int times;
        public float waitTime;
        public int time_before_start;
        public int time_after_finish;
        public TextMeshProUGUI ingameText;
        [HideInInspector]
        public float timerBarValue;
        
        public GameObject topCanvasPrefab;
        [HideInInspector]
        public TopCanvas topCanvasScr;
        public GameObject messageCanvasPrefab;

        SoundPlayer soundPlayer;
        public System.Random random;
        [HideInInspector]
        public Button pauseButton;
        public GameObject pausePrefab;
        [HideInInspector]
        public GameObject pauseCanvas;
        Button resumeButton;
        Button menuButton;
        Button nextButton;

        protected virtual void Awake () {
            //Defecto = times * waitTime
            random = new System.Random ();
            //Lang manager
            try {
                //makeTranslations ();
                soundPlayer = UnityEngine.EventSystems.EventSystem.current.GetComponent<SoundPlayer> ();
                if(ingameText != null)
                {
                    string key = "game" + GlobalVar.getGamemodeNumber() + "ingame";
                    ingameText.text = LangDataset.getText (key);
                }
            } catch (System.Exception) {
                print ("ERROR EN GAMEMODE");
            }
            GameObject canvasObj = (GameObject)Instantiate(topCanvasPrefab);
            canvasObj.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            topCanvasScr = canvasObj.GetComponent<TopCanvas>();
        }

        public IEnumerator showMessage (int score) {
            int gamemode = GlobalVar.getGamemodeNumber();
            
            StartCoroutine (topCanvasScr.startAnimation ());
            //Message and add score
            //The score to show is the map of the score given by the max of the BD
            Instantiate(messageCanvasPrefab).transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            /*
            GameObject.Find("MessageText1").GetComponent<TextMeshProUGUI>().text = LangDataset.getText ("message1");
            string second = LangDataset.getText ("message2") + " " + GlobalVar.getPlaysNumber(gamemode).ToString() + " " + LangDataset.getText ("message3");
            GameObject.Find("MessageText2").GetComponent<TextMeshProUGUI>().text =  second;
            GameObject.Find("MessagePercentText").GetComponent<TextMeshProUGUI>().text = GlobalVar.getPercent(gamemode, score).ToString() + "%";
            */
            int perc = GlobalVar.getPercent(gamemode, score);
            string stars = GlobalVar.getStars(perc);
            GameObject.Find("MessageStars").GetComponent<RawImage>().texture = Resources.Load<Texture2D>(stars);
            GameObject.Find("MessageNextText").GetComponent<TextMeshProUGUI>().text = LangDataset.getText ("next");
            nextButton = GameObject.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener (delegate () {
                GlobalVar.addScore (gamemode, score);
            });
            yield return new WaitForSeconds (1.5f);
            nextButton.interactable = true;
        }
    }
}