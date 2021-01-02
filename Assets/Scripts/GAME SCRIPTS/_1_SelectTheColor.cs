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
            setCorrectColor ();
            setButtonsColor ();
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
                Color a = randColors[i].Color;
                if (a == correctColor.Color) {
                    but.GetComponent<_1_ColorButton> ().setCorrect ();
                }
                but.GetComponent<_1_ColorButton> ().setColor (a);
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
            int score = 0;
            foreach (GameObject but in randButtons) {
                score += but.GetComponent<_1_ColorButton> ().getScore ();
            }
            StartCoroutine (showMessage (score));
        }

    }
}