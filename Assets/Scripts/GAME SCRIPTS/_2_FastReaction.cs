using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameSpace {
    public class _2_FastReaction : GameMode {
        public float percIncorrect;
        public GameObject buttonGroup;
        public GameObject circleGroup;
        //How much time a cube is shown
        public float timeShown;

        List<GameObject> buttons;
        List<GameObject> randButtons;
        int[] incorrectButtons;
        public GameObject text;

        protected override void Awake () {
            timerBarValue = (times * waitTime) + timeShown;
            base.Awake();
        }

        void Start () {
            buttons = new List<GameObject> ();
            setButtons();
            setColors();
            StartCoroutine (show ());
        }

        public void setButtons(){
            //Adds script to the child of "buttonGroup", before add them to a list
            for (int i = 0; i < buttonGroup.transform.childCount; i++) {
                GameObject but = buttonGroup.transform.GetChild (i).gameObject;
                but.AddComponent<_2_FastReactionButton> ();
                buttons.Add (but);
            }
            //Takes X buttons randomly determined by "times", randomly selects "percBlack %" to be black
            randButtons = buttons.OrderBy (x => random.Next ()).Take (times).ToList ();
            incorrectButtons = Enumerable.Range (0, times - 1).OrderBy (x => random.Next ()).Take (Mathf.FloorToInt((float) times * percIncorrect )).ToArray ();
        }

        public void setColors(){
            int numCircles = circleGroup.transform.childCount;
            List<ColorObject> colorsList = GlobalVar.colorsWithoutWhite;
            List<ColorObject> correctColors = colorsList.OrderBy (x => random.Next ()).Take (numCircles).ToList();
            List<ColorObject> incorrectColors = colorsList.Except(correctColors).ToList();
            
            for(int i=0; i<numCircles; i++)
            {
                circleGroup.transform.GetChild(i).GetComponent<Button> ().image.color = correctColors[i].Color;
            }

            for (int i = 0; i < times; i++) {
                if (incorrectButtons.Contains (i)) {
                    randButtons[i].GetComponent<_2_FastReactionButton> ().setCorrect (correctColors[random.Next (correctColors.Count)]);
                } else {
                    randButtons[i].GetComponent<_2_FastReactionButton> ().setIncorrect (incorrectColors[random.Next (incorrectColors.Count)]);
                }
            }
        }

        IEnumerator show () {
            topCanvasScr.startCountdown(time_before_start);
            yield return new WaitForSeconds (time_before_start);
            text.SetActive(false);

            for (int i = 0; i < times; i++) {
                randButtons[i].GetComponent<_2_FastReactionButton> ().show();
                yield return new WaitForSeconds (waitTime);
            }
            yield return new WaitForSeconds (time_after_finish);

            int score = 0;
            for (int i = 0; i < buttonGroup.transform.childCount; i++) {
                GameObject but = buttonGroup.transform.GetChild (i).gameObject;
                score += but.GetComponent<_2_FastReactionButton> ().getScore ();
                print("IT " + i + " | " + score);
            }
            StartCoroutine(showMessage(score));
        }
    }

}