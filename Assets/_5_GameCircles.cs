using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {
    public class _5_GameCircles : MonoBehaviour {

        private ColorObject CorrectColor;
        private ColorObject playedColor;
        private Image image;
        bool played = false;

        void Start(){
            image = gameObject.GetComponent<Image>();
        }
        public void setCorrectColor(List<string> list){
            CorrectColor = GlobalVar.getRandomColorFrom(true, list, new System.Random());
        }

        public void playColor(ColorObject color){
            played = true;
            playedColor = color;
            image.color = playedColor.Color;
        }

        public void erasePlay(){
            played = false;
            image.color = Color.white;
        }

        public void showCorrectColor(){
            image.color = CorrectColor.Color;
        }

        void hide () {
            image.color = Color.white;
        }

        
    }
}