using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {
    public class TopCanvas : MonoBehaviour {
        public Image fillTimer;
        public Slider progressBar;
        public Slider timerBar;
        public Button pause;
        
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
        public float time = 5f;
        float passedTime = 0f;

        float map (float s, float a1, float a2, float b1, float b2) {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        void Start () {
            pause = GameObject.Find("PauseButton").GetComponent<Button>();
            progress = GlobalVar.getProgress ();
            nextProgress = GlobalVar.getProgressOfNextGamemode ();

            progressBar.value = progress;
            valueToScale = nextProgress - progress;
            currentValue = progress;

            //timerBar animation
            timerBar.value = 1f;
            time = GameObject.Find ("Dynamics").GetComponent<GameMode> ().timerBarValue;
            fillTimer.color = new Color (0f, 0.7255f, 0f, 1f);
            //print ("Progress: " + progress + ", nextProgress: " + nextProgress);
        }

        public void setTimerBarValue (float valueTimer) {
            //Sets timer bar progress bar value and color, determined by the value
            timerBar.value = valueTimer;
            if (valueTimer > 0.5f) {
                fillTimer.color = new Color (map (valueTimer, 1f, 0.5f, 0f, 0.7255f), 0.7255f, 0f, 1f);
            } else {
                fillTimer.color = new Color (0.7255f, map (valueTimer, 0.5f, 0f, 0.7255f, 0f), 0f, 1f);
            }
        }

        public void timerBarUpdate () {
                float v = (time - passedTime) / time;
                setTimerBarValue (v);
                passedTime += Time.deltaTime;
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