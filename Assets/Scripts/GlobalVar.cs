using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace GameSpace {

    public static class GlobalVar {
        public static string AuthorizationToken = "Token c83c28761403c5cac48319d062a5ddcf161980dc";
        public static string BearerToken = "";
        public static bool firstLaunch = true;
        public static bool tryagain = false;
        //Una corrutina que recibe la acción (primer parámetro) a realizar al enviar el form (segundo parámetro)
        //Dentro del action hay un bool (que muestra si el request ha tirado error) y el string (lo devuelto por el server)

        public static List<ColorObject> basicColors;
        public static List<ColorObject> visibleColors;
        public static List<ColorObject> colorsWithoutWhite;
        public static List<ColorObject> colors;

        public static Dictionary<int, int> scores;
        public static List<int> gamemodes;
        public static List<int> maxScores;

        public static void resetStartGame () {
            scores = null;
            randomizeGamemodes ();
            setColors ();
            //TO DO, retrieve max scores form DB
            maxScores = new List<int> () {
                200,
                200,
                200,
                200
            };
            SceneManager.LoadScene ("game" + gamemodes[0]);
        }

        public static ColorObject getColorByName(string name){
            for(int i = 0; i < colors.Count; i++) {
                ColorObject c = colors[i];
                if ( c.Name == name ) 
                {
                    return c;
                }
            }
            return null;
        }

        public static ColorObject getRandomColorFrom (bool isIn, List<string> list, System.Random random) {
            while (true) {
                ColorObject res = colors[random.Next (colors.Count)];
                if ( list.Contains (res.Name).Equals(isIn) ) 
                {
                    return res;
                }
            }
        }

        public static float map (float s, float a1, float a2, float b1, float b2) {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
            /*if(res>b2){
                return b2;
            }else{
                return res;
            }*/
        }

        public static int mapScore (float actualScore, int gamemode) {
            // TO DO, get max of all players on that gamemode;
            //float maxScore = getMaxOfGamemode(int gamemode);
            //For that, we have to send the actualScore to the server
            float maxScore = 1000f;
            return Mathf.FloorToInt (map (actualScore, 0f, maxScore, 0f, 1000f));
        }

        public static void randomizeGamemodes () {
            gamemodes = new List<int> () {
                2
            };
            gamemodes = gamemodes.OrderBy (i => Guid.NewGuid ()).ToList ();
            //SceneManager load first gamemode
        }

        public static float getProgress () {
            float ina = gamemodes.IndexOf (getGamemodeNumber ());
            float of = gamemodes.Count;
            //Debug.Log("In " + ina + " of " + of + " = " + (ina/of));
            return ina / of ;
        }

        public static float getProgressOfNextGamemode () {
            float ina = gamemodes.IndexOf (getGamemodeNumber ()) + 1;
            float of = gamemodes.Count;
            float value = ina / of ;
            if (value > 1) {
                return 1f;
            } else {
                return value;
            }
        }

        public static int getGamemodeNumber () {
            return Int16.Parse (SceneManager.GetActiveScene ().name.Replace ("game", ""));
        }

        public static void addScore (int score) {
            int gm = getGamemodeNumber ();

            if (scores == null) {
                scores = new Dictionary<int, int> ();
            }
            scores[gm] = score;
            int i = 0;
            foreach (var item in scores) {
                Debug.Log ("ITEMS" + item + " - " + i);
                i++;
            }
            goToNextGamemode (gm);
        }

        public static void goToNextGamemode (int gm) {
            int nextGm = gamemodes.IndexOf (gm) + 1;
            if (nextGm == gamemodes.Count) {
                //SceneManager sendResultsToDB
                int suma = 0;
                for (int i = 1; i <= scores.Count; i++) {
                    suma += scores[i];
                }
                Debug.Log ("JUEGO ACABADO con SCORE = " + suma);
            } else {
                SceneManager.LoadScene ("game" + gamemodes[nextGm]);
            }
        }

        public static void setColors () {
            basicColors = new List<ColorObject> ();
            basicColors.Add (new ColorObject ("yellow", Color.yellow, Color.black));
            basicColors.Add (new ColorObject ("blue", Color.blue, Color.white));
            basicColors.Add (new ColorObject ("red", Color.red, Color.white));
            basicColors.Add (new ColorObject ("green", Color.green, Color.black));
            basicColors.Add (new ColorObject ("purple", Color.magenta, Color.white));
            basicColors.Add (new ColorObject ("cyan", Color.cyan, Color.black));
            visibleColors = new List<ColorObject> (basicColors);
            visibleColors.Add (new ColorObject ("black", Color.black, Color.white));
            colorsWithoutWhite = new List<ColorObject> (visibleColors);
            colorsWithoutWhite.Add (new ColorObject ("gray", Color.gray, Color.black));
            colorsWithoutWhite.Add (new ColorObject ("orange", new Color32 (255, 137, 0, 255), Color.black));
            colors = new List<ColorObject> (colorsWithoutWhite);
            colors.Add (new ColorObject ("white", Color.white, Color.black));
        }

        public static IEnumerator StartFormCoroutine (Action<KeyValuePair<bool, string>> value, string url, string[] args) {
            WWWForm form = new WWWForm ();
            string argsAll = "";
            string BearerToken = Communication.getBearerToken ();
            yield return new WaitForSeconds (1);

            Debug.Log ("BEARER " + BearerToken);
            form.AddField ("token", BearerToken);
            //Crea los fields del form segun los args
            foreach (string i in args) {
                if (i.Length > 0) {
                    argsAll = argsAll + "/" + i;
                    string[] fieldArgs = i.Split (':');
                    form.AddField (fieldArgs[0], fieldArgs[1]);
                }
            }

            //Crea el request, añade el form y los headers
            string postUrl = "dapas.evidagroup.es/" + url;
            UnityWebRequest www = UnityWebRequest.Post (postUrl, form);
            www.SetRequestHeader ("Authorization", GlobalVar.AuthorizationToken);
            www.SetRequestHeader ("Content-Type", "application/x-www-form-urlencoded");

            yield return www.SendWebRequest ();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log (postUrl + "#txarto " + www.error);
                value (new KeyValuePair<bool, string> (false, "some error message: " + www.error));
            } else {
                Debug.Log (postUrl + "#ondo " + www.GetResponseHeader ("access_token"));
                value (new KeyValuePair<bool, string> (true, www.downloadHandler.text));
            }
            yield return null;
        }

        ///////////////////////////////////
        //RESULT FUNCTIONS
        ///////////////////////////////////

        public static void sendData (string send, string scene) {
            PlayerPrefs.SetString ("send", send);
            PlayerPrefs.SetString ("scene", scene);
            SceneManagerController.ChangeSceneSender ();
        }

        public static void setResultToSend (string difficulty, float time, string message, float result) {
            PlayerPrefs.SetString ("resDifficulty", difficulty);
            PlayerPrefs.SetFloat ("resTime", time);
            PlayerPrefs.SetString ("resMessage", message);
            PlayerPrefs.SetFloat ("resResult", result);
            PlayerPrefs.SetInt ("max", 0);
        }
    }

}