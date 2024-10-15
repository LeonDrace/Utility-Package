using System.Collections.Generic;
using UnityEngine;

namespace LeonDrace.Utility.Helpers
{
	public static class CoroutineHelper
	{
		private static readonly WaitForFixedUpdate s_FixedUpdate = new WaitForFixedUpdate();
		public static WaitForFixedUpdate FixedUpdate => s_FixedUpdate;


		private static readonly WaitForEndOfFrame s_EndOfFrame = new WaitForEndOfFrame();
		public static WaitForEndOfFrame EndOfFrame => s_EndOfFrame;


		private static readonly Dictionary<float, WaitForSeconds> s_WaitForSecondsDict = new(100, new FloatComparer());

		/// <summary>
		/// Get or create and cache <see cref="WaitForSeconds"/> instance to reduce garbage when reusing.
		/// Will return null if the time is below or equal to one frame.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static WaitForSeconds GetWaitForSeconds(float seconds)
		{
			return GetOrCreateWaitFor(seconds);
		}

		private static WaitForSeconds GetOrCreateWaitFor(float seconds)
		{
			if (seconds < 1f / Application.targetFrameRate) return null;

			if (!s_WaitForSecondsDict.TryGetValue(seconds, out var forSeconds))
			{
				forSeconds = new WaitForSeconds(seconds);
				s_WaitForSecondsDict[seconds] = forSeconds;
			}

			return forSeconds;
		}

		class FloatComparer : IEqualityComparer<float>
		{
			public bool Equals(float x, float y) => Mathf.Abs(x - y) <= Mathf.Epsilon;
			public int GetHashCode(float obj) => obj.GetHashCode();
		}
	}
}
