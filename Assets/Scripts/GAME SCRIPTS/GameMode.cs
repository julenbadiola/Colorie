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

        protected virtual void Awake () {
            //Defecto = times * waitTime
            random = new System.Random ();
            //Lang manager
            try {
                //makeTranslations ();
                soundPlayer = UnityEngine.EventSystems.EventSystem.current.GetComponent<SoundPlayer> ();
            } catch (System.Exception) {
                print ("ERROR EN AWAKE LANGUAGE GAME Creator");
            }
            GameObject canvasObj = (GameObject)Instantiate(topCanvasPrefab);
            canvasObj.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            topCanvasScr = canvasObj.GetComponent<TopCanvas>();            
            pauseCanvasStuff();
        }

        public void pauseCanvasStuff(){
            pauseCanvas = (GameObject)Instantiate(pausePrefab);
            pauseCanvas.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
            resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
            menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
            
            pauseCanvas.SetActive(false);
            pauseButton.onClick.AddListener (delegate () {
                SceneManagerController.pauseGame();
                pauseCanvas.SetActive(true);
                resumeButton.interactable = true;
                menuButton.interactable = true;
            });

            resumeButton.onClick.AddListener (delegate () {
                SceneManagerController.resumeGame();
                pauseCanvas.SetActive(false);
                resumeButton.interactable = false;
                menuButton.interactable = false;
            });
            menuButton.onClick.AddListener (delegate () {
                SceneManagerController.resumeGame();
                SceneManagerController.ChangeSceneMenu();
            });
        }

        public IEnumerator showMessage (int score) {
            StartCoroutine (topCanvasScr.startAnimation ());
            //Message and add score
            //The score to show is the map of the score given by the max of the BD
            Instantiate(messageCanvasPrefab).transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            GameObject.Find("MessageText").GetComponent<TextMeshProUGUI>().text = GlobalVar.mapScore(score, GlobalVar.getGamemodeNumber()) + "";

            yield return new WaitForSeconds (3);            
            GlobalVar.addScore (score);
        }
    }
}