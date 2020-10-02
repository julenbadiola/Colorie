using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSpace {
    public class _5_GameCircles : MonoBehaviour {

        private ColorObject CorrectColor;
        private ColorObject playedColor;
        private Image image;
        public bool played = false;
        int score = 0;

        void Awake(){
            image = gameObject.GetComponent<Image>();
        }

        public void setCorrectColor(List<string> list, System.Random random){
            CorrectColor = GlobalVar.getRandomColorFrom(true, list, random);
            image.color = CorrectColor.Color;
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

        public void hide () {
            image.color = Color.white;
        }

        public int getScore(){
            if(CorrectColor.Color == playedColor.Color){
                return 20;
            }
            else{
                return 0;
            }
        }

        
    }
}