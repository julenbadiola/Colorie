using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _5_BlinksReflejo : MonoBehaviour {
        public GameObject circle;
        public GameObject[] buttons;
        public int times;
        public int numColor;
        System.Random random;

        List<ColorObject> randColors;
        List<ColorObject> colors;
        bool waiting = true;
        ColorObject rightColor;
        // Start is called before the first frame update
        void Start () {
            GlobalVar.setColors ();
            random = new System.Random ();

            randColors = GlobalVar.visibleColors.OrderBy (x => random.Next ()).Take (numColor).ToList ();
            uuuala ();

            for (int i = 0; i < randColors.Count; i++) {
                buttons[i].GetComponent<Button> ().image.color = randColors[i].Color;
                buttons[i].GetComponentInChildren<TextMeshProUGUI> ().text = randColors[i].Name.ToUpper ();
                buttons[i].GetComponentInChildren<TextMeshProUGUI> ().color = randColors[i].TextColor;
            }

            StartCoroutine (show ());

        }

        void uuuala () {
            bool seguir = false;
            while (!seguir) {
                colors = new List<ColorObject> ();
                for (int i = 0; i < times; i++) {
                    colors.Add (randColors[random.Next (randColors.Count)]);
                }
            }
        }

        IEnumerator show () {
            for (int i = 0; i < times; i++) {
                rightColor = colors[i];
                print ("ES " + rightColor.Name);
                circle.GetComponent<Button> ().image.color = colors[i].Color;

                foreach (GameObject but in buttons) {
                    if (but.GetComponent<Button> ().image.color == rightColor.Color) {
                        but.GetComponent<Button> ().onClick.AddListener (delegate () {
                            print ("correct");
                            waiting = false;
                        });
                    } else {
                        but.GetComponent<Button> ().onClick.AddListener (delegate () {
                            print ("incorrect");
                            waiting = false;
                        });
                    }
                }

                while (waiting) {
                    yield return new WaitForSeconds (0.05f);
                }

                circle.GetComponent<Button> ().image.color = Color.white;
                yield return new WaitForSeconds (1);
            }
        }
    }
}