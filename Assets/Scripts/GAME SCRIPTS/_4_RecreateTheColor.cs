using System.Collections;
using System.Collections.Generic;
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

        private Color RandomColorCircle;
        private Color PlayerColor;
        // Start is called before the first frame update

        protected override void Awake () {
            timerBarValue = times;
            base.Awake();
        }

        void Start () {
            sliderRed.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliderGreen.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            sliderBlue.onValueChanged.AddListener (delegate { updatePlayerColor (); });
            acceptButton.onClick.AddListener (delegate { compareColorsAndSendScore (); });

            RandomColorCircle = new Color (
                Random.Range (0f, 1f),
                Random.Range (0f, 1f),
                Random.Range (0f, 1f)
            );

            Circle.GetComponent<Button> ().image.color = RandomColorCircle;
            updatePlayerColor ();
            StartCoroutine(waitSecondsAndSendScore());
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
            compareColorsAndSendScore();
        }

        public void compareColorsAndSendScore () {
            float suma = Mathf.Abs (RandomColorCircle.r - PlayerColor.r) +
                Mathf.Abs (RandomColorCircle.g - PlayerColor.g) +
                Mathf.Abs (RandomColorCircle.b - PlayerColor.b);

            float percDiferente = suma * 100 / 255;
            print ("PERC: " + percDiferente + " de difencia");

            StartCoroutine (showMessage (Mathf.FloorToInt(percDiferente * 100)));
        }
    }
}