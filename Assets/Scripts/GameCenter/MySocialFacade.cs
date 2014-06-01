public static class MySocialFacade
{
    private enum SocialPlugin
    {
        Native,
        Prime31
    }

    private const SocialPlugin _currentPlugin = SocialPlugin.Prime31;

    public static int MaxVisibleScores
    {
        get
        {
            if (_currentPlugin == SocialPlugin.Prime31)
                return MySocialPrime31.MaxVisibleScores;
            else
                return MySocialNative.MaxVisibleScores;
        }
    }

    public static void SubmitScore(long score)
    {
        if (_currentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.SubmitScore(score);
        else
            MySocialNative.SubmitScore(score);
    }

    /// <param name="aroundMyRankResults">прислать только результаты, вокруг текущего игрока</param>
    /// <param name="count">число видимых результатов</param>
    public static void LoadScoresForLeaderboard(bool aroundMyRankResults, int count = 0)
    {
        if (_currentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.LoadScoresForLeaderboard(aroundMyRankResults, count);
        else
            MySocialNative.LoadScoresForLeaderboard(aroundMyRankResults, count);
    }

    public static void ShowLeaderboard()
    {
        if (_currentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.ShowLeaderboard();
        else
            MySocialNative.ShowLeaderboard();
    }

    public static void Authenticate()
    {
        if (_currentPlugin == SocialPlugin.Prime31)
            MySocialPrime31.Authenticate();
        else
            MySocialNative.Authenticate();
    }


    public static GPGPlayerInfo GetLocalPlayerInfo()
    {
        if (_currentPlugin == SocialPlugin.Prime31)
            return MySocialPrime31.GetLocalPlayerInfo();
        else
            return MySocialNative.GetLocalPlayerInfo();
    }
}
