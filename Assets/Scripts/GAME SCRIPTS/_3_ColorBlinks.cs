using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _3_ColorBlinks : GameMode {
        public GameObject circle;
        public GameObject[] buttons;
        public int numColor;

        List<ColorObject> randColors;
        List<ColorObject> colors;
        ColorObject maxColor;
        bool stopped = true;
        float playedTime = 0f;
        
        protected override void Awake () {
            timerBarValue = times * waitTime;
            base.Awake();
        }

        void Start () {
            
            setColorLists ();
            //We add the script to each button
            for (int i = 0; i < randColors.Count; i++) {
                buttons[i].AddComponent<_3_ColorBlinksButton> ().setButtonParameters(randColors[i]);
            }
            print ("ES " + maxColor.Name);
            StartCoroutine (show ());
        }
        
        IEnumerator show () {
            //Shows a color every second until times
            for (int i = 0; i < times; i++) {
                circle.GetComponent<Button> ().image.color = colors[i].Color;
                yield return new WaitForSeconds (waitTime / 2f);
                circle.GetComponent<Button> ().image.color = Color.white;
                yield return new WaitForSeconds (waitTime / 2f);
                
            }
            //When all the colors are shown, make buttons interactable and colored
            for (int i = 0; i < randColors.Count; i++) {
                buttons[i].GetComponent<_3_ColorBlinksButton>().show();
            }
            //Start counting time
            stopped = false;
        }

        void Update () {
            if (!stopped) {
                playedTime += Time.deltaTime;
            }
        }
        
        public void checkAnswerAndSendScore(Color color){
            stopped = true;
            int score = 0;
            //If clicked button is the correct, score is positive; on the contrary, is 0
            if(color == maxColor.Color){
                score = Mathf.FloorToInt(100f / playedTime);
            }
            StartCoroutine (showMessage (GlobalVar.mapScore(score, GlobalVar.getGamemodeNumber())));
        }

        void setColorLists () {
            bool seguir = false;
            //RandColors example = [Blue, Red, Yellow]
            randColors = GlobalVar.visibleColors.OrderBy (x => random.Next ()).Take (numColor).ToList ();

            while (!seguir) {
                //Colors = List of colors that are going to be shown, determined by times
                //Ex: Blue, Blue, Red, Blue, Yellow, Yellow ... until times
                colors = new List<ColorObject> ();
                for (int i = 0; i < times; i++) {
                    colors.Add (randColors[random.Next (randColors.Count)]);
                }
                int e = 0;

                //Array that contains the number of times each color is going to be shown
                //Ex [3, 3, 4]
                int[] timesArray = new int[numColor];
                for (int i = 0; i < randColors.Count; i++) {
                    timesArray[i] = colors.Count (x => x == randColors[i]);
                }

                //If there is a color that is going to be shown more times than the other ones, break loop
                //Ex: one more than the other ones 4 > 3, 3
                for (int i = 0; i < timesArray.Length; i++) {
                    if (timesArray[i] == timesArray.Max ()) {
                        e++;
                    }
                }
                if (e == 1) {
                    maxColor = randColors[Array.IndexOf (timesArray, timesArray.Max ())];
                    seguir = true;
                }

            }
        }
    }

}