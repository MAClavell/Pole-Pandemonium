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

	private GooglePlayGamesController()
	{
		AuthenticateUser();
	}

	private static void AuthenticateUser()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate((success) => 
		{ 
			//Successful signin
			if(success)
			{
				Leaderboard.Init();
			}
		});
	}
}
