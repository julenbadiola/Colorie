using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace GameSpace {

    [System.Serializable]
    public class PlayerInfo
    {
        public int max1;
        public int max2;
        public int max3;
        public int max4;
        public int max5;
        public int maxScoreInGame;
    }

    public static class GlobalVar {
        public static string AuthorizationToken = "Token c83c28761403c5cac48319d062a5ddcf161980dc";
        public static string BearerToken = "";
        public const int games = 5;
        public static bool launch = true;
        public static bool tryagain = false;
        //Una corrutina que recibe la acción (primer parámetro) a realizar al enviar el form (segundo parámetro)
        //Dentro del action hay un bool (que muestra si el request ha tirado error) y el string (lo devuelto por el server)

        public static List<ColorObject> basicColors;
        public static List<ColorObject> visibleColors;
        public static List<ColorObject> colorsWithoutWhite;
        public static List<ColorObject> colors;

        public static Dictionary<int, int> scores;
        public static List<int> gamemodes;
        public static int error = 0;
        
        //FROM SERVER
        public static PlayerInfo info;
        public static Dictionary<int, Dictionary<string, int>> scoreSummary;

        public static void InitScoreSummary()
        {
            scoreSummary = new Dictionary<int, Dictionary<string, int>>();
        }

        public static void SaveScoresLocally()
        {
            foreach(int gamemode in scores.Keys)
            {
                int score = scores[gamemode];
                Dictionary<string, int> summary = scoreSummary[gamemode];
                bool done = false;
                foreach(string key in summary.Keys)
                {
                    string[] subs = key.Split(char.Parse("-"));
                    int min = System.Int32.Parse(subs[0]);
                    int max = System.Int32.Parse(subs[1]);
                    if(min <= score && score <= max)
                    {
                        Debug.Log("INTRODUCIENDO LOCALLY EN " + key);
                        summary[key] = summary[key] + 1;
                        done = true;
                        break;
                    }
                }
                if(!done){
                    string key = score.ToString() + "-" + score.ToString();
                    summary.Add(key, 1);
                    Debug.Log("INTRODUCIENDO LOCALLY 2 EN " + key);
                }
            }
        }

        public static void SetScoreSummary(int gamemode, Dictionary<string, string> summary)
        {   
            if(summary.Count > 0)
            {
                Dictionary<string, int> spec = new Dictionary<string, int>();
                foreach(var i in summary)
                {
                    spec.Add(i.Key, int.Parse(i.Value));
                }
                scoreSummary.Add(gamemode, spec);
            }
        }

        public static void CreatePlayerInfoFromJSON(string jsonString)
        {
            info = JsonUtility.FromJson<PlayerInfo>(jsonString);
            Debug.Log("CREATED FROM JSON " + info.max1 + " - "+ info.max2 + " - "+ info.max3 + " - "+ info.max4 + " - "+ info.max5 + " - "+ info.maxScoreInGame);
        }

        public static void resetStartGame () {
            scores = null;
            randomizeGamemodes ();
            setColors ();
            SceneManager.LoadScene ("game" + gamemodes[0]);
        }

        public static ColorObject getColorByName(string key){
            string name = LangDataset.getText (key);
            for(int i = 0; i < colors.Count; i++) {
                ColorObject c = colors[i];
                if ( c.Name == name || c.Name == key) 
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

        public static int getPlaysNumber(int gamemode)
        {
            return scoreSummary[gamemode].Count;
        }

        public static int getPercent(int gamemode, int score)
        {
            int total = 0;
            int lower = 0;
            Dictionary<string, int> data = scoreSummary[gamemode];
            foreach(string key in data.Keys)
            {
                Debug.Log(key);
                total += data[key];
                string[] subs = key.Split(char.Parse("-"));
                int min = System.Int32.Parse(subs[0]);
                int max = System.Int32.Parse(subs[1]);
                
                Debug.Log(min + " - " + max);
                if(min <= score && score <= max)
                {
                    Debug.Log("MI SCORE ESTA DENTRO DE " + key);
                    lower += Mathf.RoundToInt(data[key] / 2);
                }
                else if(min < score)
                {
                    Debug.Log("MI SCORE ES MAYOR QUE " + key);
                    lower += data[key];
                }
                else
                {
                    Debug.Log("MI SCORE ES MENOR QUE " + key);
                }
            }
            Debug.Log("TOTAL: " + total + " / LOWER: " + lower);
            if(total != 0)
            {
                if(lower == 0)
                {
                    return 0;
                }
                return Mathf.FloorToInt (((float) lower / (float) total) * 100);
            }
            else
            {
                return 100;
            }
        }

        public static void randomizeGamemodes () {
            gamemodes = new List<int> () {};
            for(int i=0; i <games; i++)
            {
                gamemodes.Add(i+1);
            }
            //SceneManager load first gamemode
        }

        public static string getStars(int perc)
        {
            if(perc < 33)
            {
                return "1stars";
            }
            if(perc < 66)
            {
                return "2stars";
            }
            else
            {
                return "3stars";
            }
        }

        public static float getProgress () {
            //Para poner la barra de progreso general en su sitio
            float ina = gamemodes.IndexOf (getGamemodeNumber ());
            float of = gamemodes.Count;
            //Debug.Log("In " + ina + " of " + of + " = " + (ina/of));
            return ina / of ;
        }

        public static float getProgressOfNextGamemode () {
            //Para la animación de la barra de progreso hacia el siguiente gamemode
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

        public static void addScore (int gamemode, int score) {
            if (scores == null) {
                scores = new Dictionary<int, int> ();
            }
            
            scores[gamemode] = score;
            int i = 0;
            foreach (var item in scores) {
                Debug.Log ("ITEMS" + item + " - " + i);
                i++;
            }
            Debug.Log ("ADDING SCORE " + score + " TO " + gamemode + " - LENGTH:" + scores.Count);
            goToNextGamemode (gamemode);
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
                SceneManagerController.ChangeSceneSender ("Menu");
                
            } else {
                SceneManager.LoadScene ("game" + gamemodes[nextGm]);
            }
        }

        public static void setColors () {
            basicColors = new List<ColorObject> ();
            basicColors.Add (new ColorObject (LangDataset.getText ("yellow"), Color.yellow, Color.black));
            basicColors.Add (new ColorObject (LangDataset.getText ("blue"), Color.blue, Color.white));
            basicColors.Add (new ColorObject (LangDataset.getText ("red"), Color.red, Color.white));
            basicColors.Add (new ColorObject (LangDataset.getText ("green"), Color.green, Color.black));
            basicColors.Add (new ColorObject (LangDataset.getText ("purple"), Color.magenta, Color.white));
            basicColors.Add (new ColorObject (LangDataset.getText ("cyan"), Color.cyan, Color.black));
            visibleColors = new List<ColorObject> (basicColors);
            visibleColors.Add (new ColorObject (LangDataset.getText ("black"), Color.black, Color.white));
            visibleColors.Add (new ColorObject (LangDataset.getText ("orange"), new Color32 (255, 137, 0, 255), Color.black));
            colorsWithoutWhite = new List<ColorObject> (visibleColors);
            colorsWithoutWhite.Add (new ColorObject (LangDataset.getText ("gray"), Color.gray, Color.black));
            colors = new List<ColorObject> (colorsWithoutWhite);
            colors.Add (new ColorObject (LangDataset.getText ("white"), Color.white, Color.black));
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
    }

}