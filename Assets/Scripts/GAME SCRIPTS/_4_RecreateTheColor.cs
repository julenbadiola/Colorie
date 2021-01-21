using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameSpace {

    public class _4_RecreateTheColor : GameMode {
        public GameObject Circle;
        public GameObject CirclePlayer;
        public Slider sliderRed;
        public Slider sliderGreen;
        public Slider sliderBlue;
        public Button acceptButton;
        public Button hintButton;
        public TextMeshProUGUI hintText;
        public TextMeshProUGUI finishText;

        private Color RandomColorCircle;
        private Color PlayerColor;
        private Slider[] sliders;

        private float timePassed = 0f;
        private bool hint = false;
        private bool sent = false;

        protected override void Awake () {
            timerBarValue = times;
            base.Awake ();
        }

        void Update(){
            timePassed += Time.deltaTime;
        }

        private void setTexts()
        {
            finishText.text = LangDataset.getText ("next");
            hintText.text = LangDataset.getText ("hint");
        }

        void Start () {
            setTexts();
            sliderRed.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliderGreen.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliderBlue.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliders = new Slider[3] { sliderRed, sliderGreen, sliderBlue };
            acceptButton.onClick.AddListener (delegate { OnClick_accept (); });
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
            hintButton.interactable = false;
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
            topCanvasScr.startCountdown(time_before_start);
            yield return new WaitForSeconds (time_before_start);
            
            yield return new WaitForSeconds (timerBarValue);
            if(!sent){
                compareColorsAndSendScore ();
            }
            
        }

        public void OnClick_accept(){
            topCanvasScr.count = false;
            compareColorsAndSendScore();
        }

        public void compareColorsAndSendScore () {
            sent = true;
            float suma = Mathf.Abs (RandomColorCircle.r - PlayerColor.r) +
                Mathf.Abs (RandomColorCircle.g - PlayerColor.g) +
                Mathf.Abs (RandomColorCircle.b - PlayerColor.b);

            float percDiferente = suma * 100 / 255;
            float percParecido = 1 - percDiferente;

            //Tiempo que ha tardado (mas 5 de ventaja)
            float tiPassed = GlobalVar.checkIfFloatInInterval(timePassed - 5f, 0f, (float) timerBarValue);
            float t = tiPassed / timerBarValue;
            float timeScore = 1 - GlobalVar.checkIfFloatInInterval(t, 0f, 1f);

            print ("PERC: " + percParecido + " de igualdad, TIME MULTIPLIER: " + timeScore + " (" + timePassed + " / " + timerBarValue+ ")");
            
            float finalPerc = (percParecido * 0.7f) + (timeScore * 0.3f);
            int score = Mathf.FloorToInt(finalPerc * 1000f);
            StartCoroutine (showMessage (score));
        }
    }
}