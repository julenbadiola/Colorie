using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _2_FastReactionButton : MonoBehaviour {
        _2_FastReaction script;
        Button button;
        ColorObject color;
        bool incorrect = false;
        bool correct = false;
        bool touched = false;
        int score = 0;

        float incMultiplier;
        float corMultiplier;

        // Start is called before the first frame update
        float playedTime;
        bool shown = false;
        void Update () {
            if ((correct || incorrect) && !touched && shown) {
                playedTime += Time.deltaTime;
            }

        }
        void Start () {
            script = GameObject.Find("Dynamics").GetComponent<_2_FastReaction> ();
            incMultiplier = 50f * script.percIncorrect;
            corMultiplier = 50f * (1f - script.percIncorrect);
            button = gameObject.GetComponent<Button> ();

            button.image.color = Color.white;
            button.onClick.AddListener (delegate () {
                touch ();
            });
        }

        public void setIncorrect (ColorObject c) {
            incorrect = true;
            color = c;
        }

        public void setCorrect (ColorObject c) {
            correct = true;
            color = c;
        }

        public void show(){
            button.image.color = color.Color;
            StartCoroutine (wait ());
        }

        public void touch () {
            touched = true;

            if (!correct) {
                if (incorrect) {
                    //print("1 Score -" + Mathf.FloorToInt(incMultiplier * playedTime));
                    score -= Mathf.FloorToInt(incMultiplier * playedTime); 
                } else {
                    //print("2 Score -15");
                    score -= 15;
                }
            } else {
                //print("3 Score +" + ((float) script.timeShown / (float) playedTime));
                score += Mathf.FloorToInt(corMultiplier * ((float) script.timeShown / (float) playedTime));
            }
            button.image.color = Color.white;
        }

        public int getScore () {
            //Mathf.FloorToInt (score * (script.timeShown * 2f / playedTime));
            if(incorrect && !touched){
                //print("4 Score +" + incMultiplier);
                score = (int)incMultiplier;
            }else if(correct && !touched){
                //print("5 Score -" + corMultiplier);
                score = (int) corMultiplier * -1;
            }
            return score;
        }

        IEnumerator wait () {
            shown = true;
            //Si no ha sido tocado en timeShown segundos y no es negro, incorrecto, si es negro, correcto
            yield return new WaitForSeconds (script.timeShown);
            button.image.color = Color.white;

        }
    }
}