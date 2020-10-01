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

    public class _5_Game5 : GameMode {
        public List<GameObject> circles;
        public List<Button> buttons;
        List<ColorObject> randColorsForButtons;

        protected override void Awake () {
            timerBarValue = times;
            base.Awake ();
        }

        void Start () {
            randColorsForButtons = GlobalVar.colors.OrderBy (x => random.Next ()).Take (buttons.Count).ToList ();
            for (int i = 0; i < circles.Count; i++) {
                circles[i].AddComponent<_5_GameCircles> ().setCorrectColor (randColorsForButtons.Select (o => o.Name).ToList ());
            }
            StartCoroutine (waitToStart ());
        }

        IEnumerator waitToStart () {
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].interactable = false;
                buttons[i].image.color = randColorsForButtons[i].Color;
                buttons[i].GetComponentInChildren<TextMeshProUGUI> ().text = randColorsForButtons[i].Name.ToUpper ();
                buttons[i].GetComponentInChildren<TextMeshProUGUI> ().color = randColorsForButtons[i].Color;
            }
            yield return new WaitForSeconds (timerBarValue);
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].interactable = true;
            }
        }

    }
}