using LeonDrace.Utility.Helpers;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace LeonDrace.Utility.Tests
{
	public class RuntimeCoroutineTests
	{
		[TestCase(10)]
		[TestCase(100)]
		[TestCase(1000)]
		public void Add_Cached_WaitForSeconds(int amount)
		{
			List<WaitForSeconds> list = new();

			for (int i = 0; i < amount; i++)
			{
				list.Add(CoroutineHelper.GetWaitForSeconds(i));
			}

			Assert.That(list.Count, Is.EqualTo(amount));
			foreach (WaitForSeconds wait in list)
			{
				Assert.That(wait, Is.Not.Null);
			}
		}


		[UnityTest]
		public IEnumerator Receive_And_WaitFor_One_Second()
		{
			WaitForSeconds waitForSeconds = CoroutineHelper.GetWaitForSeconds(1);

			Assert.That(waitForSeconds, Is.Not.Null);

			yield return waitForSeconds;
		}
	}
}
