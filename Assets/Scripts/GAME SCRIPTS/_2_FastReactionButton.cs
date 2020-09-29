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
        bool blue = false;
        bool touched = false;
        int score = 0;

        // Start is called before the first frame update
        float playedTime;
        void Update () {
            if ((black || blue) && !touched) {
                playedTime += Time.deltaTime;
            }

        }
        void Start () {
            script = gameObject.GetComponent<_2_FastReaction> ();
            button = gameObject.GetComponent<Button> ();

            button.image.color = Color.white;
            button.onClick.AddListener (delegate () {
                touch ();
            });
        }
        public void setIncorrect () {
            button.image.color = Color.red;
        }

        public void setBlack () {
            black = true;
            button.image.color = Color.black;
            StartCoroutine (wait ());
        }

        public void setBlue () {
            blue = true;
            button.image.color = Color.blue;
            StartCoroutine (wait ());
        }

        public void touch () {
            touched = true;
            //Si no es blue, el toque es incorrecto
            //Puntuacion
            //azul tocado = 10 ptos
            //negro tocado = -10 puntos
            //blanco tocado = -5 puntos
            if (!blue) {
                if (black) {
                    score -= 10;
                } else {
                    score -= 5;
                }

                setIncorrect ();
            } else {
                score += 10;
                button.image.color = Color.white;
            }
        }

        public int getScore () {
            return Mathf.FloorToInt (score * (script.timeShown * 2f / playedTime));
        }

        IEnumerator wait () {
            //Si no ha sido tocado en timeShown segundos y no es negro, incorrecto, si es negro, correcto
            yield return new WaitForSeconds (script.timeShown);
            if (!touched) {
                if (!black) {
                    setIncorrect ();
                } else if (black) {
                    button.image.color = Color.white;
                }
            }

        }
    }
}