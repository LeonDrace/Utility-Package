#if ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace LeonDrace.Utility.Timers
{
	public static class TimerHelper
	{
		/// <summary>
		/// Create a stopwatch.
		/// </summary>
		/// <returns></returns>
		public static StopwatchTimer GetStopwatchTimer()
		{
			return new StopwatchTimer();
		}

		/// <summary>
		/// Create a countdown with optional on stop callback.
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="onStop"></param>
		/// <returns></returns>
		public static CountdownTimer GetCountdownTimer(float duration, System.Action onStop = null)
		{
			return new CountdownTimer(duration, onStop);
		}

		/// <summary>
		/// Start a countdown using Task and calling onProgress every updateInMs.
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="updateInMs"></param>
		/// <param name="onProgress"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static async Task StartCountdownAsync(float duration, int updateInMs, Action<float> onProgress, CancellationToken token)
		{
			float currentSteps = 0;
			duration *= 1000;
			int maxSteps = (int)duration / updateInMs;
			while (currentSteps <= maxSteps)
			{
				token.ThrowIfCancellationRequested();
				await Task.Delay(updateInMs);
				currentSteps++;
				duration -= updateInMs;
				onProgress?.Invoke(Mathf.Min(currentSteps / maxSteps, 1));
			}
		}

#if ENABLE_UNITASK
		/// <summary>
		/// Start a countdown using UniTask and calling onProgress every updateInMs.
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="updateInMs"></param>
		/// <param name="onProgress"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static async UniTask StartUniCountdownAsync(float duration, int updateInMs, Action<float> onProgress, CancellationToken token)
		{
			float currentSteps = 0;
			duration *= 1000;
			int maxSteps = (int)duration / updateInMs;
			while (currentSteps <= maxSteps)
			{
				token.ThrowIfCancellationRequested();
				await UniTask.Delay(updateInMs);
				currentSteps++;
				duration -= updateInMs;
				onProgress?.Invoke(Mathf.Min(currentSteps / maxSteps, 1));
			}
		}
#endif
	}

	public abstract class Timer
	{
		protected float m_InitialTime;
		protected float Time { get; set; }
		public bool IsRunning { get; protected set; }

		public float Progress => Time / m_InitialTime;

		public Action OnTimerStart = delegate { };
		public Action OnTimerStop = delegate { };

		protected Timer(float value)
		{
			m_InitialTime = value;
			IsRunning = false;
		}

		public void Start()
		{
			Time = m_InitialTime;
			if (!IsRunning)
			{
				IsRunning = true;
				OnTimerStart.Invoke();
			}
		}

		public void Stop()
		{
			if (IsRunning)
			{
				IsRunning = false;
				OnTimerStop.Invoke();
			}
		}

		public void Resume() => IsRunning = true;
		public void Pause() => IsRunning = false;

		public abstract void Tick(float deltaTime);
	}

	public class CountdownTimer : Timer
	{
		public CountdownTimer(float value) : base(value) { }

		public CountdownTimer(float value, Action onStop) : base(value)
		{
			this.OnTimerStop = onStop;
		}

		public override void Tick(float deltaTime)
		{
			if (IsRunning && Time > 0)
			{
				Time -= deltaTime;
			}

			if (IsRunning && Time <= 0)
			{
				Stop();
			}
		}

		public bool IsFinished => Time <= 0;

		public void Reset() => Time = m_InitialTime;

		public void Reset(float newTime)
		{
			m_InitialTime = newTime;
			Reset();
		}
	}

	public class StopwatchTimer : Timer
	{
		public StopwatchTimer() : base(0) { }

		public override void Tick(float deltaTime)
		{
			if (IsRunning)
			{
				Time += deltaTime;
			}
		}

		public void Reset() => Time = 0;

		public float GetTime() => Time;
	}
}