using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;


//public abstract class SocialBase : MonoSingleton<SocialBase>
//facade 
public class MySocialPrime31 : MonoSingleton<MySocialPrime31>
{
    private const string _leaderboardID = "CgkIuaqBk6sCEAIQEQ";

    public static int MaxVisibleScores { get; private set; }

    public static void SubmitScore(long score)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        PlayGameServices.submitScore(_leaderboardID, score);
#endif
    }

    public static void LoadScoresForLeaderboard(bool aroundMyRankreuslts, int count = 0)
    {
        MaxVisibleScores = (count > 0) ? count : int.MaxValue;
#if UNITY_IPHONE || UNITY_ANDROID
        PlayGameServices.loadScoresForLeaderboard(_leaderboardID, GPGLeaderboardTimeScope.AllTime, false, aroundMyRankreuslts);
        Debug.LogWarning("call LoadScoresForLeaderboard");
#endif
    }

    public static string ShowLeaderboard()
    {
        string ret = "ShowLeaderboardUI";
#if UNITY_IPHONE || UNITY_ANDROID
        // show leaderboard UI
        PlayGameServices.showLeaderboard(_leaderboardID, GPGLeaderboardTimeScope.AllTime);
#endif
        return ret;
    }



#if UNITY_IPHONE || UNITY_ANDROID



    //private string text = "123";
    // Use this for initialization
    void Start()
    {
        string s = MySocialPrime31EventListener.Instance.name;
        Init();
        Authenticate();
        //Invoke("Authenticate",3f);
    }

    private void Authenticate()
    {
        PlayGameServices.setAchievementToastSettings(GPGToastPlacement.Center, 50);
        PlayGameServices.authenticate();
        Debug.LogWarning("MySocial authenticate");
    }




    private static string Init()
    {
        PlayGameServices.enableDebugLog(true);
#if UNITY_IPHONE
        // we always want to call init as soon as possible after launch. Be sure to pass your own clientId to init on iOS!
        // This call is not required on Android.
        //PlayGameServices.init("80302069049-5lm6co43b05m3im6usj6c8ccpg7q7570.apps.googleusercontent.com", false);//только для IOS.  Надо вставить clientID
        //PlayGameServices.init("5lm6co43b05m3im6usj6c8ccpg7q7570.apps.googleusercontent.com", false);
        PlayGameServices.init("80302069049.apps.googleusercontent.com", false); //так должно быть судя по этому скрину,  https://developers.google.com/games/services/images/consoleLocationOfClientId2.png
#endif
        //GPGManager.loadScoresSucceededEvent += Instance.loadScoresSucceededEvent;

        return "inited";
    }


    public static GPGPlayerInfo GetLocalPlayerInfo()
    {

        var playerInfo = PlayGameServices.getLocalPlayerInfo();
        Prime31.Utils.logObject(playerInfo);

        // if we are on Android and have an avatar image available, lets download the profile pic
        //if (Application.platform == RuntimePlatform.Android && playerInfo.avatarUrl != null)
        //    PlayGameServices.loadProfileImageForUri(playerInfo.avatarUrl);
        return playerInfo;
    }


    //private static void EventsSubscibe()
    //{

    //    GPGManager.authenticationFailedEvent


    //}

#endif
}
