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
                    print("2 Score +" + Mathf.FloorToInt(10f * playedTime));
                    score -= Mathf.FloorToInt(10f * playedTime); 
                } else {
                    print("Score -5");
                    score -= 5;
                }

                button.image.color = Color.red;
            } else {
                print("1 Score +" + Mathf.FloorToInt(10f * ((float) script.timeShown / (float) playedTime)));
                score += Mathf.FloorToInt(10f * ((float) script.timeShown / (float) playedTime));
                button.image.color = Color.white;

            }
        }

        public int getScore () {
            //Mathf.FloorToInt (score * (script.timeShown * 2f / playedTime));
            if(incorrect && !touched){
                print("Score +10");
                score = 10;
            }else if(correct && !touched){
                print("Score -20");
                score = -20;
            }
            return score;
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