#if UNITY_ANDROID

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesServicesController
{
	/// <summary>
	/// If the user has authenticated with the respective game service
	/// </summary>
	public static bool Authenticated 
	{
		get =>
#if UNITY_ANDROID
			PlayGamesPlatform.Instance.IsAuthenticated();
#else
		false;
#endif
	}
	
	private static bool initiated = false;
	/// <summary>
	/// Initialize the platform's game service
	/// </summary>
	public static void Init()
	{
		if(!initiated)
		{
			initiated = true;
			AuthenticateUser();
		}
	}

	/// <summary>
	/// Authenticate the user with the platform's game service
	/// </summary>
	private static void AuthenticateUser()
	{
#if UNITY_ANDROID
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
		{
			//Successful signin
			if (result == SignInStatus.Success)
			{
				Leaderboard.InitLeaderboard();
			}
		});
#endif
	}

	/// <summary>
	/// Show the platform's leaderboard UI
	/// </summary>
	public static void ShowLeaderboardUI()
	{
#if UNITY_ANDROID
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
#endif
	}
}

#endif