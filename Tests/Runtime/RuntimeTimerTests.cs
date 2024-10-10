#if ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

using LeonDrace.Utility.Extensions;
using LeonDrace.Utility.Timers;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace LeonDrace.Utility.Tests
{
	public class RuntimeTimerTests
	{

		[UnityTest]
		public IEnumerator Countdown()
		{
			float duration = 1;

			bool success = false;
			void StateValidator()
			{
				success = true;
			}

			var timer = TimerCreator.GetCountdownTimer(duration, StateValidator);
			timer.Start();
			while (!success)
			{
				timer.Tick(Time.deltaTime);
				yield return null;
			}

			Assert.That(success, Is.True);
			Assert.That(timer.IsFinished, Is.True);
		}

		[UnityTest]
		public IEnumerator StopWatch()
		{
			var timer = TimerCreator.GetStopwatchTimer();
			timer.Start();

			while (timer.GetTime() < 1)
			{
				timer.Tick(Time.deltaTime);
				yield return null;
			}
			timer.Stop();

			Assert.That(timer.IsRunning, Is.False);
			Assert.IsTrue(timer.GetTime() >= 1);
		}

		[UnityTest]
		public IEnumerator Countdown_Async()
		{
			float progress = 0;
			void StateValidator(float value)
			{
				progress = value;
			}

			yield return TimerCreator.StartCountdownAsync(1, 10, StateValidator, default).AsCoroutine();

			Assert.That(progress, Is.EqualTo(1));
		}

#if ENABLE_UNITASK

		[UnityTest]
		public IEnumerator Countdown_UniAsync()
		{
			float progress = 0;
			void StateValidator(float value)
			{
				progress = value;
			}

			yield return TimerCreator.StartUniCountdownAsync(1, 16, StateValidator, default).ToCoroutine();

			Assert.That(progress, Is.EqualTo(1));
		}

#endif
	}
}
