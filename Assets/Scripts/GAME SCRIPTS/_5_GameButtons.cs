using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {
    public class _5_GameButtons : MonoBehaviour {
        Button button;
        List<_5_GameCircles> listCircles;
        ColorObject myColor;

        void Awake () {
            button = GetComponent<Button> ();
        }
        void Start () {
            button.interactable = false;
            button.onClick.AddListener (delegate () {
                touch ();
            });
        }

        public void show (ColorObject color, List<GameObject> list) {
            listCircles = list.Select (o => o.GetComponent<_5_GameCircles> ()).ToList ();
            myColor = color;
            button.image.color = color.Color;
            button.GetComponentInChildren<TextMeshProUGUI> ().text = color.Name.ToUpper ();
            button.GetComponentInChildren<TextMeshProUGUI> ().color = color.Color;
        }

        public void touch () {
            bool next = true;
            for (int i = 0; i < listCircles.Count; i++) {
                if (!listCircles[i].played && next) {
                    listCircles[i].playColor (myColor);
                    next = false;
                    
                    //If it is the last 
                    if (i == listCircles.Count - 1) {
                        GameObject.Find ("Dynamics").GetComponent<_5_Game5> ().finishGame ();
                    }
                }

            }
        }
    }

}