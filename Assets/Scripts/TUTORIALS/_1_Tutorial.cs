using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RDG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {

    public class _1_Tutorial : MonoBehaviour {
        public List<GameObject> buttons;
        public GameObject colorText;

        List<GameObject> randButtons;
        ColorObject correctColor;
        List<ColorObject> randColors;

        System.Random random;

        public GameObject tutorialCanvasPrefab;
        void Start () {
            random = new System.Random ();
            GameObject canvasObj = (GameObject)Instantiate(tutorialCanvasPrefab);
            canvasObj.transform.SetParent(GameObject.Find("GameCanvas").transform, false);

            foreach (GameObject but in buttons) {
                but.AddComponent<_1_ColorButton> ();
            }
            setCorrectColor();
            setButtonsColor();
        }
        public void setCorrectColor () {
            GlobalVar.setColors();
            randColors = GlobalVar.colors.OrderBy (x => random.Next ()).Take (buttons.Count).ToList ();
            correctColor = randColors[0];
            colorText.GetComponent<TextMeshProUGUI> ().text = correctColor.Name.ToUpper ();
            colorText.GetComponent<TextMeshProUGUI> ().color = randColors[1].Color;
        }

        //For each iteration, orders the colors randomly
        public void setButtonsColor () {
            randButtons = buttons.OrderBy (x => random.Next ()).ToList ();
            int i = 0;
            foreach (GameObject but in randButtons) {
                Color a = randColors[i].Color;
                if (a == correctColor.Color) {
                    but.GetComponent<_1_ColorButton> ().setCorrect ();
                }
                but.GetComponent<_1_ColorButton> ().setColor (a);
                i++;
            }
        }

    }
}