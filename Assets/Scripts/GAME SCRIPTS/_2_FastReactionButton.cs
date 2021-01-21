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

        private bool isActive() {
            return correct || incorrect;
        }

        void Update () {
            if (shown && !touched && (correct || incorrect)) {
                playedTime += Time.deltaTime;
            }

        }
        void Start () {
            script = GameObject.Find("Dynamics").GetComponent<_2_FastReaction> ();
            incMultiplier = 50f * (1f - script.percIncorrect);
            corMultiplier = 50f * script.percIncorrect; //35 en caso de 0.7
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
            if(!touched && isActive()){
                touched = true;

                float multiplier = 0f; 
                float pTime = playedTime;
                if(pTime < 0.5f){
                    pTime = 0.5f;
                }
                
                if (correct) {
                    //así sabemos que el mínimo de pTime es 0.4, por lo tanto el máximo de timeShown = 6.25 (para 2.5 y 0.4)
                    float a = ((float) script.timeShown / (float) pTime - 0.4f);
                    float max = ((float) script.timeShown / 0.5f);
                    multiplier = GlobalVar.map(a, 1f, max, 1f, 2f);
                } else if (incorrect){
                    //El cubo tocado es incorrecto = (-) 50 * reverseTime (cuanto más tiempo ha visto que era incorrecto, más penalización)
                    float a = ((float) pTime / (float) script.timeShown);
                    multiplier = GlobalVar.map(a, 0f, 1f, 1f, 2f) * -1f;
                }
                Debug.Log("PLAYED TIME: " + playedTime + " / TIME SHOWN: " + script.timeShown);
                Debug.Log("MULTIPLIER: " + multiplier + " / RESULT: " + 50*multiplier);
                score = Mathf.FloorToInt(50 * multiplier);
                button.image.color = Color.white;
            }
        }

        public int getScore () {
            if(!touched){
                //Si el cubo no ha sido tocado y es incorrecto
                if(incorrect){
                    //Incorrecto y no tocado = correcto
                    return 50;
                }else if(correct){
                    //Correcto y no tocado = incorrecto
                    return -50;
                }else{
                    return 0;
                }
            }else{
                //Si el cubo ya ha sido tocado, el score es el que está calculado
                return score;
            }
        }

        IEnumerator wait () {
            //Si no ha sido tocado en timeShown segundos y no es negro, incorrecto, si es negro, correcto
            shown = true;
            yield return new WaitForSeconds (script.timeShown);
            button.image.color = Color.white;

        }
    }
}