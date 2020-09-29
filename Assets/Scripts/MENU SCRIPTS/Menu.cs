using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameSpace {
   public class Menu : MonoBehaviour {
      public Button PlayButton;
      public Button OptionsButton;
      public Button ExitButton;
      public Button VideoButton;
      public GameObject ErrorObject;
      public GameObject ErrorExp;
      public GameObject tryagain;
      void makeTranslations () {
         // MAIN MENU
         PlayButton.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("play");
         ExitButton.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("exit");
         OptionsButton.GetComponentInChildren<TextMeshProUGUI> ().text = LangDataset.getText ("options");
         tryagain.GetComponent<TextMeshProUGUI> ().text = LangDataset.getText ("tryagain");
      }

      public void Start () {
         makeTranslations ();
         if(PlayerPrefs.GetInt("error")==1){
            ErrorObject.SetActive(true);
            ErrorExp.GetComponent<TextMeshProUGUI> ().text = PlayerPrefs.GetString("errorExp");
         }
      }
      public void playButton(){
         GlobalVar.resetStartGame();
      }
      public void quitButton(){
         SceneManagerController.QuitGame();
      }

      public void playVideo () {
         SceneManagerController.ChangeSceneVideo();
      }

      public void QuitGame () {
         Debug.Log ("QUIT!");
         Application.Quit ();
      }
   }
}