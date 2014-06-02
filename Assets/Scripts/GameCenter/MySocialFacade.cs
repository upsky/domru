using UnityEngine;

public class MySocialFacade : MonoSingleton<MySocialFacade>
{
    [SerializeField]
    private SocialPlugin _currentPlugin;

    private void Start()
    {
        dynamic instance;
        if (CurrentPlugin == SocialPlugin.Prime31)
            instance = MySocialPrime31.Instance;
        else
            instance = MySocialNative.Instance;

        //Authenticate();
    }

    private enum SocialPlugin
    {
        Native,
        Prime31
    }

    private static SocialPlugin CurrentPlugin
    {
        get { return Instance._currentPlugin; }
    }

    public static int MaxVisibleScores
    {
        get
        {
            if (CurrentPlugin == SocialPlugin.Prime31)
                return MySocialPrime31.MaxVisibleScores;
            else
                return MySocialNative.MaxVisibleScores;
        }
    }

    public static void SubmitScore(long score)
    {
        if (CurrentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.SubmitScore(score);
        else
            MySocialNative.SubmitScore(score);
    }

    /// <param name="aroundMyRankResults">прислать только результаты, вокруг текущего игрока</param>
    /// <param name="count">число видимых результатов</param>
    public static void LoadScoresForLeaderboard(bool aroundMyRankResults, int count = 0)
    {
        if (CurrentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.LoadScoresForLeaderboard(aroundMyRankResults, count);
        else
            MySocialNative.LoadScoresForLeaderboard(aroundMyRankResults, count);
    }

    public static void ShowLeaderboard()
    {
        if (CurrentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.ShowLeaderboard();
        else
            MySocialNative.ShowLeaderboard();
    }

    public static void Authenticate()
    {
        if (CurrentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.Authenticate();
        else
            MySocialNative.Authenticate();
    }


    public static GPGPlayerInfo GetLocalPlayerInfo()
    {
        if (CurrentPlugin == SocialPlugin.Prime31)
            return MySocialPrime31.GetLocalPlayerInfo();
        else
            return MySocialNative.GetLocalPlayerInfo();
    }
}
