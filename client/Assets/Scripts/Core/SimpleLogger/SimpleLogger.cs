using UnityEngine;
using System.Collections;

namespace Core.Log
{
	public static class SimpleLogger
	{
		public static void Log(object caller, string log)
		{
			Debug.Log(Format(caller, log));
		}

		public static void LogWarning(object caller, string log)
		{
			Debug.LogWarning(Format(caller, log));
		}

		public static void LogError(object caller, string log)
		{
			Debug.LogError(Format(caller, log));
		}

		private static string Format(object caller, string log)
		{
			return string.Format("{0} : {1}", caller.GetType().Name, log);
		}
	}
}

