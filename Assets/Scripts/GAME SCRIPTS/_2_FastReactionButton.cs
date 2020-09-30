using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _2_FastReactionButton : MonoBehaviour {
        _2_FastReaction script;
        Button button;
        bool black = false;
        bool colored = false;
        bool touched = false;
        int score = 0;

        // Start is called before the first frame update
        float playedTime;
        void Update () {
            if ((black || colored) && !touched) {
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

        public void setBlack () {
            black = true;
            button.image.color = Color.black;
            StartCoroutine (wait ());
        }

        public void setColored () {
            colored = true;
            button.image.color = GlobalVar.getRandomColorFrom(new List<string> {"yellow", "blue", "green", "purple", "cyan"}, script.random).Color;
            StartCoroutine (wait ());
        }

        public void touch () {
            touched = true;

            if (!colored) {
                if (black) {
                    score -= Mathf.FloorToInt(10f * playedTime); 
                } else {
                    score -= 5;
                }

                button.image.color = Color.red;
            } else {

                score += Mathf.FloorToInt(10f * ((float) script.timeShown / (float) playedTime));
                button.image.color = Color.white;

            }
        }

        public int getScore () {
            //Mathf.FloorToInt (score * (script.timeShown * 2f / playedTime));
            return score;
        }

        IEnumerator wait () {
            //Si no ha sido tocado en timeShown segundos y no es negro, incorrecto, si es negro, correcto
            yield return new WaitForSeconds (script.timeShown);
            if (!touched) {
                if (!black) {
                    button.image.color = Color.red;
                } else if (black) {
                    button.image.color = Color.white;
                }
            }

        }
    }
}