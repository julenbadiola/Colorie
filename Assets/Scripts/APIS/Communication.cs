using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlurrySDK;

public static class Communication
{
    //FLURRY
#if UNITY_ANDROID
    //private string FLURRY_API_KEY = "PCB3MN8TJR5DJBFGWBMZ";
    private static string FLURRY_API_KEY = "ZTXN74FY93P5HRV5FYP8";
#elif UNITY_IPHONE
    private static string FLURRY_API_KEY = "IOS_API_KEY";
#else
    private static string FLURRY_API_KEY = null;
#endif

    public static bool flurryStarted = false;

    //AUTH TOKEN
    private static string bearerToken = "";

    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject activity;

    private static AndroidJavaObject AccountManager;

    //the objects are initialized and the selection of the DAPAS account is asked
    public static void initialize()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AccountManager = new AndroidJavaObject("at.e_nnovation.emma.apps.mylibrary.DapasAccountManager", "deusto");

        AccountManager.Call("checkIfUserIsLoggedIn", activity);

        if (!flurryStarted)
        {
            startFlurry();
            flurryStarted = true;
        }
    }

    public static void startFlurry()
    {
        Debug.Log("#flurryStart");
        new Flurry.Builder()
          //.WithCrashReporting(true)
          .WithLogEnabled(true)
          //.WithLogLevel(Flurry.LogLevel.LogVERBOSE)
          //.WithMessaging(true)
          .Build(FLURRY_API_KEY);
        //Flurry.SetUserId("user123");
        Flurry.LogEvent("Colorie_Started");
    }

    public static void endFlurry()
    {
        Debug.Log("#flurryEnd");
        Flurry.LogEvent("Colorie_Ended");
    }

    public static string getBearerToken()
    {
        AccountManager.Call("getAccessToken", activity, new GetTokensCallback());
        return bearerToken;
    }

    class GetTokensCallback : AndroidJavaProxy
    {
        public GetTokensCallback() : base("at.e_nnovation.emma.apps.mylibrary.DapasAccountManager$GetTokensCallback") { }

        void onSuccess(string bearerTokenReceived)
        {
            bearerToken = bearerTokenReceived;
            Debug.Log("#onSuccess -> " + bearerToken);
        }

    }

    public static string getUserId()
    {

        return "";
    }

}
