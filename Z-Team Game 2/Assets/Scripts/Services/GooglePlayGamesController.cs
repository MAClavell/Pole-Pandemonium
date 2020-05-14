#if UNITY_ANDROID

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayGamesController
{
	//Singleton
	private static GooglePlayGamesController instance = null;
	/// <summary>
	/// Get the instance of our Google Play Games API
	/// </summary>
	public static GooglePlayGamesController Instance
	{
		get
		{
			if (instance == null)
				instance = new GooglePlayGamesController();
			return instance;
		}
	}

	public bool Authenticated { get => PlayGamesPlatform.Instance.IsAuthenticated(); }

	private GooglePlayGamesController()
	{
		AuthenticateUser();
	}

	private void AuthenticateUser()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
		{
			//Successful signin
			if (result == SignInStatus.Success)
			{
				Leaderboard.Init();
			}
		});
	}

	public void ShowLeaderboardUI()
	{
		//Try to sign in if we aren't
		if (!PlayGamesPlatform.Instance.IsAuthenticated())
		{
			//Successful signin
			PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
			{
				if (result == SignInStatus.Success)
					PlayGamesPlatform.Instance.ShowLeaderboardUI();
			});
		}
		else
		{
			PlayGamesPlatform.Instance.ShowLeaderboardUI();
		}
	}
}

#endif