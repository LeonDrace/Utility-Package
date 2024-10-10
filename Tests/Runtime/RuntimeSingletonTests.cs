using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace LeonDrace.Utility.Tests
{
	public class RuntimeSingletonTests
	{
		SingletonTest m_Singleton;
		RegulatorSingletonTest m_RegulatorSingleton;
		PersistentSingletonTest m_PersistentSingleton;

		[SetUp]
		public void Create()
		{
			m_Singleton = SingletonTest.Instance;
			m_RegulatorSingleton = RegulatorSingletonTest.Instance;
			m_PersistentSingleton = PersistentSingletonTest.Instance;
		}

		[Test]
		public void Singleton_Exists()
		{
			Assert.That(SingletonTest.Instance != null);
			Assert.That(m_Singleton == SingletonTest.Instance);
		}

		[Test]
		public void Destroy_Duplicate_Singleton()
		{
			var singleton2 = new GameObject("2nd Singleton").AddComponent<SingletonTest>();
			Assert.That(SingletonTest.Instance != singleton2);
		}

		[Test]
		public void RegulatorSingleton_Exists()
		{
			Assert.That(RegulatorSingletonTest.Instance != null);
			Assert.That(m_RegulatorSingleton == RegulatorSingletonTest.Instance);
		}

		[UnityTest]
		public IEnumerator Replace_Previous_RegulatorSingleton()
		{
			var singleton2 = new GameObject("2nd Regulator Singleton").AddComponent<RegulatorSingletonTest>();
			Assert.That(RegulatorSingletonTest.Instance != m_RegulatorSingleton);
			Assert.That(RegulatorSingletonTest.Instance == singleton2);

			//Wait till others are destroyed
			yield return new WaitForSeconds(0.2f);

			var elements = GameObject.FindObjectsOfType<RegulatorSingletonTest>();

			Assert.That(elements.Length == 1);
			Assert.That(elements[0] == singleton2);
		}

		[Test]
		public void PersistentSingleton_Exists()
		{
			Assert.That(PersistentSingletonTest.Instance != null);
			Assert.That(m_PersistentSingleton == PersistentSingletonTest.Instance);
		}

		[Test]
		public void Destroy_Duplicate_PersistentSingleton()
		{
			var singleton2 = new GameObject("2nd Persistent Singleton").AddComponent<PersistentSingletonTest>();
			Assert.That(PersistentSingletonTest.Instance == m_PersistentSingleton);
		}

		public class SingletonTest : Singletons.Singleton<SingletonTest>
		{

		}

		public class RegulatorSingletonTest : Singletons.RegulatorSingleton<RegulatorSingletonTest>
		{

		}

		public class PersistentSingletonTest : Singletons.PersistentSingleton<PersistentSingletonTest>
		{

		}
	}
}
