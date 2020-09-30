using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {
    public class _2_FastReaction : GameMode {
        public float percBlack;
        public GameObject buttonGroup;
        //How much time a cube is shown
        public float timeShown;

        List<GameObject> buttons;
        List<GameObject> randButtons;
        int[] blackButtons;
        
        protected override void Awake () {
            timerBarValue = (times * waitTime) + timeShown;
            base.Awake();
        }

        void Start () {
            buttons = new List<GameObject> ();
            setButtons();
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
            blackButtons = Enumerable.Range (0, times - 1).OrderBy (x => random.Next ()).Take (Mathf.FloorToInt((float) times * percBlack )).ToArray ();
        }

        IEnumerator show () {
            yield return new WaitForSeconds (waitTime);
            for (int i = 0; i < times; i++) {
                if (blackButtons.Contains (i)) {
                    randButtons[i].GetComponent<_2_FastReactionButton> ().setBlack ();
                } else {
                    randButtons[i].GetComponent<_2_FastReactionButton> ().setBlue ();
                }
                yield return new WaitForSeconds (waitTime);
            }

            int score = 40;
            for (int i = 0; i < buttonGroup.transform.childCount; i++) {
                GameObject but = buttonGroup.transform.GetChild (i).gameObject;
                score += but.GetComponent<_2_FastReactionButton> ().getScore ();
            }
            StartCoroutine (showMessage (GlobalVar.mapScore(score, GlobalVar.getGamemodeNumber())));
        }
    }

}