using LeonDrace.Utility.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace LeonDrace.Utility.Tests
{
	public class RuntimeCombineTests
	{
		private const string CombinedName = "NEW";

		[Test]
		public void Combined_Object_Has_Correct_Name()
		{
			var combined = CombineCubes(CombinedName, 2, out GameObject[] gameObjects);

			Assert.That(combined.meshFilter.gameObject.name, Is.EqualTo(combined.meshFilter.name));
		}

		[Test]
		public void Combined_Object_Has_Correct_Vertices()
		{
			int count = 2;
			var combined = CombineCubes(CombinedName, count, out GameObject[] gameObjects);

			Assert.That(combined.meshFilter.mesh.vertices.Length, Is.EqualTo(count * 24));
		}

		[Test]
		public void Combined_Object_Has_Applied_Rendering_Values()
		{
			var combined = CombineCubes(CombinedName, 2, out GameObject[] gameObjects);

			var meshRenderer = gameObjects[0].GetComponent<MeshRenderer>();
			Assert.That(meshRenderer.renderingLayerMask, Is.EqualTo(combined.meshRenderer.renderingLayerMask));
			Assert.That(meshRenderer.shadowCastingMode, Is.EqualTo(combined.meshRenderer.shadowCastingMode));
			Assert.That(meshRenderer.sharedMaterial, Is.EqualTo(combined.meshRenderer.sharedMaterial));
		}

		[Test]
		public void Combined_Object_Has_Expected_Bounds_Center_Of_Previous_Singles()
		{
			var combined = CombineCubes(CombinedName, 2, out GameObject[] gameObjects);

			Bounds preCalculatedBounds = GetBounds(gameObjects);
			Assert.That(CompareVector3(preCalculatedBounds.center, combined.meshRenderer.bounds.center));
		}

		[Test]
		public void Combined_Object_Has_Expected_Bounds_Size_Of_Previous_Singles()
		{
			var combined = CombineCubes(CombinedName, 2, out GameObject[] gameObjects);

			Bounds preCalculatedBounds = GetBounds(gameObjects);
			Assert.That(CompareVector3(preCalculatedBounds.size, combined.meshRenderer.bounds.size));
		}

		[TestCase(10)]
		[TestCase(100)]
		[TestCase(1000)]
		public void Combine_Cubes(int count)
		{
			var combined = CombineCubes(CombinedName, 2, out GameObject[] gameObjects);

			Assert.That(combined.meshRenderer);
			Assert.That(combined.meshFilter);
		}

		[Test]
		public void Combine_Primitives_With_Custom_RendererInfo()
		{
			GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube); //24 vertices
			GameObject cylinder1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder); //88 vertices
			cylinder1.transform.position = Vector3.one;

			GameObject[] gameObjects = new GameObject[] { cube1, cylinder1 };
			CombineHelper.RendererInfo rendererInfo = new CombineHelper.RendererInfo();
			rendererInfo.FromGameObject(cube1);
			rendererInfo.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			string name = "NEW";
			var combined = CombineHelper.Combine(gameObjects, rendererInfo, name);

			var meshRenderer = cube1.GetComponent<MeshRenderer>();
			Assert.That(rendererInfo.renderingLayerMask, Is.EqualTo(combined.meshRenderer.renderingLayerMask));
			Assert.That(rendererInfo.shadowCastingMode, Is.EqualTo(combined.meshRenderer.shadowCastingMode));
			Assert.That(rendererInfo.shareMaterial, Is.EqualTo(combined.meshRenderer.sharedMaterial));
		}

		private CombineHelper.MeshInfo CombineCubes(string name, int amount, out GameObject[] gameObjects)
		{
			gameObjects = new GameObject[amount];
			for (int i = 0; i < amount; i++)
			{
				gameObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
				gameObjects[i].transform.position = Vector3.one * i;
			}

			return CombineHelper.Combine(gameObjects, name);
		}

		private Bounds GetBounds(GameObject[] gameObjects)
		{
			var bounds = default(Bounds);
			for (var i = 0; i < gameObjects.Length; ++i)
			{
				bounds.Encapsulate(gameObjects[i].GetComponent<Renderer>().bounds);
			}
			return bounds;
		}

		private bool CompareVector3(Vector3 a, Vector3 b)
		{
			// Truncat to ignore floating precision error.
			return a.x.ToString("F2") == b.x.ToString("F2")
				&& a.y.ToString("F2") == b.y.ToString("F2")
				&& a.z.ToString("F2") == b.z.ToString("F2");
		}
	}
}
