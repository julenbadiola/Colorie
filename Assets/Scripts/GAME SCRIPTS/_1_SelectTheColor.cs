using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RDG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _1_SelectTheColor : GameMode {
        public List<GameObject> buttons;
        public TextMeshProUGUI colorText;

        List<GameObject> randButtons;
        ColorObject correctColor;
        List<ColorObject> randColors;
        public int maxTimes = 10;
        private bool scoreShown = false;
        private int timesDone = 0;

        protected override void Awake () {
            timerBarValue = times;
            base.Awake();
        }

        void Start () {
            foreach (GameObject but in buttons) {
                but.AddComponent<_1_ColorButton> ();
            }
            StartCoroutine (waitSecondsAndSendScore ());
        }

        public void nextColor () {
            if(timesDone < maxTimes){
                setCorrectColor ();
                setButtonsColor ();
                timesDone++;
            }else{
                if(!scoreShown){
                    sendScore();
                }
            }
        }

        //Picks random colors determined by "buttons.Count" (buttons number in buttons list)
        //sets one of them as the correct and sets text attributes
        public void setCorrectColor () {
            randColors = GlobalVar.colors.OrderBy (x => random.Next ()).Take (buttons.Count).ToList ();
            correctColor = randColors[0];
            colorText.text = correctColor.Name.ToUpper ();
            colorText.color = randColors[1].Color;
        }

        //For each iteration, orders the colors randomly
        public void setButtonsColor () {
            randButtons = buttons.OrderBy (x => random.Next ()).ToList ();
            int i = 0;
            
            
            foreach (GameObject but in randButtons) {
                Color c = randColors[i].Color;
                if (c == correctColor.Color) {
                    but.GetComponent<_1_ColorButton> ().setCorrect (true);
                }else{
                    but.GetComponent<_1_ColorButton> ().setCorrect (false);
                }
                float timePerfection = ((float) times / (float) maxTimes);
                but.GetComponent<_1_ColorButton> ().setColor (c, timePerfection);
                i++;
            }
        }

        //When timeout -> shows message and sends score
        IEnumerator waitSecondsAndSendScore () {
            colorText.text = LangDataset.getText("ready") + "?";
            topCanvasScr.startCountdown(time_before_start);
            yield return new WaitForSeconds (time_before_start);
            nextColor ();
            
            yield return new WaitForSeconds (times);
            if(!scoreShown){
                sendScore();
            }
        }

        private void sendScore(){
            if(!scoreShown){
                topCanvasScr.stopTimerBar();
                scoreShown = true;
                int score = 0;
                foreach (GameObject but in randButtons) {
                    int butScore = but.GetComponent<_1_ColorButton> ().getScore ();
                    UnityEngine.Debug.Log("BUTTON | score of button: " + butScore);
                    score += butScore;
                }
                //100f es la puntuacion maxima de cada iteracion, por lo que:
                GlobalVar.mapScore(score, 0f, 100f * maxTimes);
                UnityEngine.Debug.Log("GAMEMODE 1: score " + score);
                StartCoroutine (showMessage (score)); 
            }
        }

    }
}