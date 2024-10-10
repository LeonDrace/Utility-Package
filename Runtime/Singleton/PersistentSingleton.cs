using UnityEngine;

namespace LeonDrace.Utility.Singletons
{
	public class PersistentSingleton<T> : MonoBehaviour where T : Component
	{
		public bool AutoUnparentOnAwake = true;

		protected static T s_Instance;

		public static bool HasInstance => s_Instance != null;
		public static T TryGetInstance() => HasInstance ? s_Instance : null;

		public static T Instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = FindAnyObjectByType<T>();
					if (s_Instance == null)
					{
						var go = new GameObject(typeof(T).Name + " Auto-Generated");
						s_Instance = go.AddComponent<T>();
					}
				}

				return s_Instance;
			}
		}

		/// <summary>
		/// Make sure to call base.Awake() in override if you need awake.
		/// </summary>
		protected virtual void Awake()
		{
			InitializeSingleton();
		}

		protected virtual void InitializeSingleton()
		{
			if (!Application.isPlaying) return;

			if (AutoUnparentOnAwake)
			{
				transform.SetParent(null);
			}

			if (s_Instance == null)
			{
				s_Instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else if (s_Instance != this)
			{
#if UNITY_EDITOR
				UnityEngine.Debug.Log($"Instance {typeof(T).Name} already exists, duplicate will be destroyed.");
#endif
				Destroy(gameObject);
			}
		}
	}
}