using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _1_ColorButton : MonoBehaviour {
        bool correct = false;
        int score = 0;
        float playedTime = 0f;
        float timePerfection = 1f;
        void Update(){
            if(correct){
                playedTime += Time.deltaTime;
            }
        }

        public void setColor (Color c, float t) {
            gameObject.GetComponent<Button> ().image.color = c;
            timePerfection = t;
        }

        public void setCorrect (bool value) {
            correct = value;
        }

        public void touch () {
            int sum = 0;
            if (correct) {
                if (playedTime <= timePerfection){
                    sum = 100;
                }else{
                    float multiplier = timePerfection / playedTime;
                    float s = 100f * (float) multiplier;
                    sum = Mathf.FloorToInt(GlobalVar.checkIfFloatInInterval(s, 0f, 100f));
                }
            } else {
                sum = -50;
            }
            Debug.Log("CORRECT:" +correct + " / score: " + sum);
            score += sum;
            reset();
            GameObject.Find ("Dynamics").GetComponent<_1_SelectTheColor>().nextColor ();
        }

        public void reset () {
            playedTime = 0f;
            correct = false;
            setColor (Color.white, timePerfection);
        }

        public int getScore () {
            return score;
        }

        void Start(){
            gameObject.GetComponent<Button> ().onClick.AddListener (delegate () {
                touch ();
            });
        }
    }
}