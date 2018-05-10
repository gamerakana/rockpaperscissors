using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using System;

namespace RPS
{
	/// <summary>
	/// The multiplayer RPS model. Manages all network communication in its implementation of the IRockPaperScissorsModel
	/// Ideally, there would be a seperate network layer. However, this is fine for such a simple class.
	/// </summary>
	public class MultiRockPaperScissorsModel : IRockPaperScissorsModel
	{
		private const int MATCH_RETRY_ATTEMPTS = 3;

		private class CoroutineExecutor : MonoBehaviour { }
		private CoroutineExecutor coroutineExecutor = null;

		[Serializable]
		private class MatchParams { public string userId; }

		[Serializable]
		private class MatchResults { public string matchId = string.Empty; }

		[Serializable]
		private class RollParams 
		{ 
			public string matchId = string.Empty; 
			public string userId = string.Empty; 
			public string shot = string.Empty;
		}

		[Serializable]
		private class QuitParams { public string userId; }

		private string userId = Guid.NewGuid().ToString();

		private string matchId;

		private int matchAttempts = 0;

		#region IRockPaperScissorsModel implementation

		/// <summary>
		/// Attempts to find a match
		/// </summary>
		/// <param name="success">Success.</param>
		/// <param name="failure">Failure.</param>
		public void Match (System.Action success, System.Action<Globals.RPSModelResultCodes> failure)
		{
			ValidateCoroutineExecutor();

			++matchAttempts;

			// Send our request
			coroutineExecutor.StartCoroutine(SendRequest(
				"http://localhost:3000/match",
				JsonUtility.ToJson(new MatchParams{userId = userId}),
				(request, result) =>
				{
					if(result == Globals.RPSModelResultCodes.Success)
					{
						try
						{
							this.matchId = JsonUtility.FromJson<MatchResults>(Encoding.UTF8.GetString(request.downloadHandler.data)).matchId;
						}
						catch(Exception)
						{
							if(failure != null)
								failure(Globals.RPSModelResultCodes.InvalidReturnData);
							return;
						}

						if(success != null)
							success();
					}
					else
					{
						// Retry
						if(result == Globals.RPSModelResultCodes.Timeout && matchAttempts < MATCH_RETRY_ATTEMPTS)
							Match(success, failure);
						else
						{
							// Quit the pending match to ensure nobody gets matched with us.
							Quit();
							if(failure != null)
								failure(result);
						}
					}
				}));
					
		}			

		/// <summary>
		/// Attempts to process the user's shot.
		/// </summary>
		/// <param name="shot">Shot.</param>
		/// <param name="success">Success.</param>
		/// <param name="failure">Failure.</param>
		public void Shoot (string shot, System.Action<RollVO> success, System.Action<Globals.RPSModelResultCodes> failure)
		{
			ValidateCoroutineExecutor();
			coroutineExecutor.StartCoroutine(SendRequest(
				"http://localhost:3000/roll",
				JsonUtility.ToJson(new RollParams
					{
						matchId = matchId,
						userId = userId,
						shot = shot
					}),
				(request, result) =>
				{
					if(result == Globals.RPSModelResultCodes.Success)
					{
						RollVO rollVO = null;
						try
						{
							rollVO = JsonUtility.FromJson<RollVO>(Encoding.UTF8.GetString(request.downloadHandler.data));

							// Track our tally
							if(rollVO.result == Globals.RollResults.Won) { ++Wins; }
							else if(rollVO.result == Globals.RollResults.Lost) { ++Losses; }
							else { ++Ties; }
						}
						catch(Exception)
						{
							if(failure != null)
								failure(Globals.RPSModelResultCodes.InvalidReturnData);
							return;
						}

						if(success != null)
							success(rollVO);
					}
					else
					{
						if(failure != null)
							failure(result);
					}
				}));
		}

		/// <summary>
		/// Quits the game
		/// </summary>
		public void Quit ()
		{
			ValidateCoroutineExecutor();
			coroutineExecutor.StartCoroutine(SendRequest(
				"http://localhost:3000/quit",
				JsonUtility.ToJson(new QuitParams { userId = userId }),
				(request, result) =>
				{
					DestroyCoroutineExecutor();
				}));
		}			

		/// <summary>
		/// Gets or sets the number of wins.
		/// </summary>
		/// <value>The wins.</value>
		public int Wins { get; set; }

		/// <summary>
		/// Gets or sets the number of losses.
		/// </summary>
		/// <value>The losses.</value>
		public int Losses { get; set; }

		/// <summary>
		/// Gets or sets the number of ties.
		/// </summary>
		/// <value>The ties.</value>
		public int Ties { get; set; }

		#endregion

		/// <summary>
		/// Sends a request
		/// </summary>
		/// <returns>The request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="data">Data.</param>
		/// <param name="complete">Complete.</param>
		private IEnumerator SendRequest(string url, string data, Action<UnityWebRequest, Globals.RPSModelResultCodes> complete, int timeout = 30)
		{
			using(UnityWebRequest request = new UnityWebRequest(url, "POST"))
			{				
				byte[] bytes = Encoding.UTF8.GetBytes(data);
				UploadHandlerRaw uploadHandler = new UploadHandlerRaw(bytes);
				uploadHandler.contentType = "application/json";
				request.uploadHandler = uploadHandler;
				request.SetRequestHeader("Content-Type", "application/json");
				DownloadHandler downloadHandler = new DownloadHandlerBuffer();
				request.downloadHandler = downloadHandler;
				request.timeout = timeout;

				yield return request.SendWebRequest();

				if(request.isNetworkError)
				{
					if(complete != null)
						complete(request, request.error == "Request timeout" ? Globals.RPSModelResultCodes.Timeout : Globals.RPSModelResultCodes.NetworkError);
				}
				else if(request.isHttpError)
				{
					if(complete != null)
						complete(request, request.responseCode == 999 ? Globals.RPSModelResultCodes.OpponentQuit : Globals.RPSModelResultCodes.GenericError);
				}
				else
				{
					if(request.responseCode == 200)
					{
						if(complete != null)
							complete(request, request.responseCode == 200 ? Globals.RPSModelResultCodes.Success : Globals.RPSModelResultCodes.GenericError);
					}
				}
			}
		}

		/// <summary>
		/// Validates the coroutine executor. Creates one if it doesn't exist.
		/// </summary>
		private void ValidateCoroutineExecutor()
		{
			if(coroutineExecutor == null)
				coroutineExecutor = new GameObject("RPS-CoroutineExecutor").AddComponent<CoroutineExecutor>();
		}

		/// <summary>
		/// Destroys the coroutine executor.
		/// </summary>
		private void DestroyCoroutineExecutor()
		{
			if(coroutineExecutor != null)
			{
				GameObject.Destroy(coroutineExecutor.gameObject);
				coroutineExecutor = null;
			}
		}
	}
}

