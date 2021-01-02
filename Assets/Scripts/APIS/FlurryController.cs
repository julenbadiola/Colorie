using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameSpace {
	public class FlurryController : MonoBehaviour {
        GameObject instance = null;
        void Awake () {
            if (instance != null) {
                Debug.Log("YA HAY");
                Destroy (this.gameObject);
            }else{
                Debug.Log("CREANDO FLURRYCONTROLLER");
                instance = this.gameObject;
                DontDestroyOnLoad (this.gameObject);
            }
        }

		void OnApplicationFocus (bool focus) {
            if (focus) SceneManagerController.ResumeApplication ();
            else SceneManagerController.LeaveApplication ();
        }
        void OnApplicationPause (bool pause) {
            if (pause) SceneManagerController.LeaveApplication ();
            else SceneManagerController.ResumeApplication ();
        }
	}
}