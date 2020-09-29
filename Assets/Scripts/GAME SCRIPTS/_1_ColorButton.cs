using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _1_ColorButton : MonoBehaviour {
        bool correct = false;
        int score = 0;
        float playedTime = 0f;
        
        void Update(){
            if(correct){
                playedTime += Time.deltaTime;
            }
        }

        public void setColor (Color color) {
            gameObject.GetComponent<Button> ().image.color = color;
        }

        public void setCorrect () {
            correct = true;
        }

        public void touch () {
            if (correct) {
                score += Mathf.FloorToInt(40f / playedTime);
            } else {
                score += 3;
            }
            reset();
            GameObject.Find ("Dynamics").GetComponent<_1_SelectTheColor>().nextColor ();
        }

        public void reset () {
            playedTime = 0f;
            correct = false;
            setColor (Color.white);
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