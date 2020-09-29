﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSpace {
    public static class SceneManagerController {

        public static bool isGamePaused () {
            if (Time.timeScale == 0) {
                return true;
            } else {
                return false;
            }
        }
        public static void pauseGame () {
            Time.timeScale = 0;
        }

        public static void resumeGame () {
            Time.timeScale = 1;
        }

        public static void ChangeToGameScene () {
            //Communication.getBearerToken ();
            resumeGame();
            SceneManager.LoadScene ("Game");
        }

        public static void ChangeSceneMenu () {
            resumeGame ();
            //Communication.getBearerToken ();
            SceneManager.LoadScene ("Menu");
        }

        public static void ChangeSceneVideo () {
            resumeGame ();
            //Communication.getBearerToken ();
            SceneManager.LoadScene ("Video");
        }

        public static void ChangeSceneDataLoad(){
            //Communication.getBearerToken ();
            SceneManager.LoadScene ("LoadData");
        }

        public static void ChangeSceneSender(){
            //Communication.getBearerToken ();
            SceneManager.LoadScene ("SendData");
        }        

        public static void goToScene(string scene){
            ////Communication.getBearerToken ();
            SceneManager.LoadScene (scene);
        }

        public static void QuitGame () {
            Communication.endFlurry();
            Application.Quit();
        }
    }
}