using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _4_RecreateTheColor : GameMode {
        public GameObject Circle;
        public GameObject CirclePlayer;
        public Slider sliderRed;
        public Slider sliderGreen;
        public Slider sliderBlue;
        public Button acceptButton;
        public Button hintButton;

        private Color RandomColorCircle;
        private Color PlayerColor;
        private Slider[] sliders;

        private float timePassed = 0f;
        private bool hint = false;

        protected override void Awake () {
            timerBarValue = times;
            base.Awake ();
        }

        void Update(){
            timePassed += Time.deltaTime;
        }

        void Start () {
            sliderRed.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliderGreen.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliderBlue.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliders = new Slider[3] { sliderRed, sliderGreen, sliderBlue };
            acceptButton.onClick.AddListener (delegate { compareColorsAndSendScore (); });
            hintButton.onClick.AddListener (delegate { showHint (); });

            RandomColorCircle = new Color (
                UnityEngine.Random.Range (0f, 1f),
                UnityEngine.Random.Range (0f, 1f),
                UnityEngine.Random.Range (0f, 1f)
            );

            Circle.GetComponent<Button> ().image.color = RandomColorCircle;
            updatePlayerColor ();
            StartCoroutine (waitSecondsAndSendScore ());
        }

        public void showHint () {
            float[] r = new float[3] { RandomColorCircle.r, RandomColorCircle.g, RandomColorCircle.b };
            int i = Array.FindIndex (r, x => x == r.Max ());
            sliders[i].value = r.Max ();
            sliders[i].interactable = false;
            hint = true;
        }

        public void updatePlayerColor () {
            PlayerColor = new Color (
                sliderRed.value,
                sliderGreen.value,
                sliderBlue.value
            );
            CirclePlayer.GetComponent<Button> ().image.color = PlayerColor;
        }

        IEnumerator waitSecondsAndSendScore () {
            yield return new WaitForSeconds (timerBarValue);
            compareColorsAndSendScore ();
        }

        public void compareColorsAndSendScore () {
            float suma = Mathf.Abs (RandomColorCircle.r - PlayerColor.r) +
                Mathf.Abs (RandomColorCircle.g - PlayerColor.g) +
                Mathf.Abs (RandomColorCircle.b - PlayerColor.b);

            float percDiferente = 100 * suma * 100 / 255;
            float percParecido = 100 - percDiferente;
            float timeScore = Mathf.FloorToInt(timePassed / (float) timerBarValue);
            print ("PERC: " + percParecido + " de igualdad, TIMESCORE: " + timeScore);

            int score = Mathf.FloorToInt(percParecido * timeScore);
            StartCoroutine (showMessage (score));
        }
    }
}