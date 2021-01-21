using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameSpace {
    public class TopCanvas : MonoBehaviour {
        public Image fillTimer;
        public Slider progressBar;
        public Slider timerBar;
        public Button pause;
        public GameObject countdown;
        public GameObject pausePrefab;
        public TextMeshProUGUI countdowntext;
        [SerializeField]
        private TextMeshProUGUI pauseText; 
        [SerializeField]
        private TextMeshProUGUI timerText; 
        [SerializeField]
        private TextMeshProUGUI progressText; 
        [HideInInspector]
        public float totalTimeProgressAnim = 0.7f;
        float fps;
        //progressBar animation
        float progress;
        float nextProgress;
        float valueToScale;
        float currentValue;
        float inframes;
        float increment = 0f;
        bool progressBarAnim = false;

        //timerBar animation
        [HideInInspector]
        public bool count;
        public float time = 5f;
        float passedTime = 0f;
        private int time_to_start = 0;

        private void setTexts(){
            pauseText.text = LangDataset.getText("pause");
            timerText.text = LangDataset.getText("timer");
            progressText.text = LangDataset.getText("progress");
        }
        void Start () {
            setTexts();
            progress = GlobalVar.getProgress ();
            nextProgress = GlobalVar.getProgressOfNextGamemode ();

            progressBar.value = progress;
            valueToScale = nextProgress - progress;
            currentValue = progress;

            //timerBar animation
            timerBar.value = 1f;
            time = GameObject.Find ("Dynamics").GetComponent<GameMode> ().timerBarValue;
            time_to_start = GameObject.Find ("Dynamics").GetComponent<GameMode> ().time_before_start;
            fillTimer.color = new Color (0f, 0.7255f, 0f, 1f);
            //print ("Progress: " + progress + ", nextProgress: " + nextProgress);
            pauseCanvasStuff();
        }

        public void setTimerBarToZero () {
            passedTime = time;
        }

        public void setTimerBarValue (float valueTimer) {
            //Sets timer bar progress bar value and color, determined by the value
            timerBar.value = valueTimer;
            if (valueTimer > 0.5f) {
                fillTimer.color = new Color (GlobalVar.map (valueTimer, 1f, 0.5f, 0f, 0.7255f), 0.7255f, 0f, 1f);
            } else {
                fillTimer.color = new Color (0.7255f, GlobalVar.map (valueTimer, 0.5f, 0f, 0.7255f, 0f), 0f, 1f);
            }
        }

        public void resetTimeBar () {
            passedTime = 0f;
        }

        public void timerBarUpdate () {
            if (count && passedTime < time) {
                float v = (time - passedTime) / time;
                setTimerBarValue (v);
                passedTime += Time.deltaTime;
            }

        }

        public void stopTimerBar(){
            count = false;
            setTimerBarValue (0f);
        }

        public void startCountdown(float seconds)
        {
            count = false;
            StartCoroutine(countdownRoutine(seconds));
        }

        public IEnumerator countdownRoutine(float seconds)
        {
            float t = seconds;
            while(t > 0)
            {
                yield return new WaitForSeconds(0.1f);
                t -= 0.1f;
                countdowntext.text = Mathf.FloorToInt(t).ToString();
            }
            pause.gameObject.SetActive(true);
            countdown.SetActive(false);
            count = true;
        }

        public void progressBarUpdate () {
            //progressBar animation
            if (progressBarAnim && currentValue < nextProgress) {
                float value = currentValue + increment;
                progressBar.value = value;
                currentValue = value;
                //print ("Current: " + currentValue + " to " + nextProgress);
            } else {
                inframes = fps * totalTimeProgressAnim;
                increment = valueToScale / inframes;
                progressBarAnim = false;
            }
        }

        public void pauseCanvasStuff(){
            if(time_to_start == 0)
            {
                pause.gameObject.SetActive(true);
                countdown.SetActive(false);
            }
            GameObject pauseCanvas = (GameObject)Instantiate(pausePrefab);
            pauseCanvas.transform.SetParent(GameObject.Find("GameCanvas").transform, false);
            Button resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
            Button menuButton = GameObject.Find("MenuButton").GetComponent<Button>();
            
            pauseCanvas.SetActive(false);
            pause.onClick.AddListener (delegate () {
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

        void Update () {
            fps = (1.0f / Time.deltaTime);
            timerBarUpdate ();
            progressBarUpdate ();
        }

        public IEnumerator startAnimation () {
            //print ("ANIMATION STARTED");
            progressBarAnim = true;

            while (progressBarAnim) {
                yield return new WaitForSeconds (0.1f);
            }

        }
    }

}