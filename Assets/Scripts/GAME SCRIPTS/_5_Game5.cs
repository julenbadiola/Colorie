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

    public class _5_Game5 : GameMode {
        public List<GameObject> circles;
        [HideInInspector]
        public List<_5_GameCircles> circlesList;
        public List<Button> buttons;
        List<ColorObject> randColorsForButtons;
        bool finish = false;

        bool countTime = false;
        float passedTime = 0f;

        protected override void Awake () {
            timerBarValue = times;
            base.Awake ();
        }

        void Start () {
            randColorsForButtons = GlobalVar.colors.OrderBy (x => random.Next ()).Take (buttons.Count).ToList ();
            circles.ForEach (c => c.AddComponent<_5_GameCircles> ().setCorrectColor (randColorsForButtons.Select (o => o.Name).ToList (), random));
            circles.ForEach(c => circlesList.Add(c.GetComponent<_5_GameCircles>()));
            buttons.ForEach (c => c.gameObject.AddComponent<_5_GameButtons> ());
            StartCoroutine (waitToStart ());
        }

        IEnumerator waitToStart () {
            buttons.ForEach (b => b.interactable = false);
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].GetComponent<_5_GameButtons> ().create (randColorsForButtons[i]);
            }
            yield return new WaitForSeconds (timerBarValue);
            
            circlesList.ForEach (c => c.hide ());
            buttons.ForEach (b => b.GetComponent<_5_GameButtons> ().show ());
            
            topCanvasScr.resetTimeBar ();
            countTime = true;

            yield return new WaitForSeconds (timerBarValue);
            if (!finish) {
                finishGame ();
            }

        }

        void Update(){
            if(countTime){
                passedTime += Time.deltaTime;
            }
        }

        public void finishGame () {
            finish = true;
            topCanvasScr.setTimerBarToZero();

            int score = 0;
            circlesList.ForEach (c => score += c.getScore ());
            score = Mathf.FloorToInt(score * (timerBarValue / passedTime));
            StartCoroutine (showMessage (score));
        }

    }
}