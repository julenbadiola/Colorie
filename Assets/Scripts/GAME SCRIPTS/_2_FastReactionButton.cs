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

        float multiplier = 50f;
        // Start is called before the first frame update
        float playedTime;
        void Update () {
            if ((correct || incorrect) && !touched) {
                playedTime += Time.deltaTime;
            }

        }
        void Start () {
            script = GameObject.Find("Dynamics").GetComponent<_2_FastReaction> ();
            button = gameObject.GetComponent<Button> ();

            button.image.color = Color.white;
            button.onClick.AddListener (delegate () {
                touch ();
            });
        }

        public void setIncorrect (ColorObject c) {
            print(c);
            incorrect = true;
            color = c;
        }

        public void setCorrect (ColorObject c) {
            print(c);
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
                    print("2 Score +" + Mathf.FloorToInt(multiplier * playedTime));
                    score -= Mathf.FloorToInt(multiplier * playedTime); 
                } else {
                    print("Score -15");
                    score -= 15;
                }

                button.image.color = Color.red;
            } else {
                print("1 Score +" + Mathf.FloorToInt(multiplier * ((float) script.timeShown / (float) playedTime)));
                score += Mathf.FloorToInt(multiplier * ((float) script.timeShown / (float) playedTime));
                button.image.color = Color.white;

            }
        }

        public int getScore () {
            //Mathf.FloorToInt (score * (script.timeShown * 2f / playedTime));
            if(incorrect && !touched){
                print("Score +10");
                score = 10;
            }else if(correct && !touched){
                print("Score -15");
                score = -15;
            }
            if(score<0){
                return 0;
            }else{
                return score;
            }
        }

        IEnumerator wait () {
            //Si no ha sido tocado en timeShown segundos y no es negro, incorrecto, si es negro, correcto
            yield return new WaitForSeconds (script.timeShown);
            if (!touched) {
                if (incorrect) {
                    button.image.color = Color.white;
                } else {
                    button.image.color = Color.red;
                }
            }

        }
    }
}