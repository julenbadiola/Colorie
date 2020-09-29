using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameSpace {
    public class _3_ColorBlinksButton : MonoBehaviour {
        Color color;
        Color textColor;
        string text;

        void Start () {
            gameObject.GetComponent<Button> ().onClick.AddListener (() => { 
                GameObject.Find ("Dynamics").GetComponent<_3_ColorBlinks>().checkAnswerAndSendScore (color);
             });
        }

        public void setButtonParameters (ColorObject colorObj) {
            color = colorObj.Color;
            textColor = colorObj.TextColor;
            text = colorObj.Name;
            hide ();
        }

        public void show () {
            gameObject.GetComponent<Button> ().image.color = color;
            gameObject.GetComponentInChildren<TextMeshProUGUI> ().color = textColor;
            gameObject.GetComponentInChildren<TextMeshProUGUI> ().text = text.ToUpper ();
            gameObject.GetComponent<Button> ().interactable = true;

        }

        public void hide () {
            gameObject.GetComponent<Button> ().image.color = Color.grey;
            gameObject.GetComponentInChildren<TextMeshProUGUI> ().color = Color.grey;
            gameObject.GetComponentInChildren<TextMeshProUGUI> ().text = "";
            gameObject.GetComponent<Button> ().interactable = false;
        }
    }

}