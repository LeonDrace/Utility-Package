using LeonDrace.Utility.Helpers;
using NUnit.Framework;
using UnityEngine;

namespace LeonDrace.Utility.Tests
{
	public class RuntimeCombineTests
	{
		[Test]
		public void Combine_Primitives()
		{
			GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube); //24 vertices
			GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube); //24 vertices
			cube2.transform.position = Vector3.one;

			GameObject[] gameObjects = new GameObject[] { cube1, cube2 };

			string name = "NEW";
			var combined = CombineHelper.Combine(gameObjects, name);

			Assert.That(combined.meshFilter.gameObject.name, Is.EqualTo(name));
			Assert.That(combined.meshFilter.mesh.vertices.Length, Is.EqualTo(48));

			var meshRenderer = cube1.GetComponent<MeshRenderer>();
			Assert.That(meshRenderer.renderingLayerMask, Is.EqualTo(combined.meshRenderer.renderingLayerMask));
			Assert.That(meshRenderer.shadowCastingMode, Is.EqualTo(combined.meshRenderer.shadowCastingMode));
			Assert.That(meshRenderer.sharedMaterial, Is.EqualTo(combined.meshRenderer.sharedMaterial));

			Bounds preCalculatedBounds = GetBounds(gameObjects);
			Assert.That(CompareVector3(preCalculatedBounds.center, combined.meshRenderer.bounds.center));
			Assert.That(CompareVector3(preCalculatedBounds.size, combined.meshRenderer.bounds.size));
		}

		[Test]
		public void Combine_Primitives_RendererInfo()
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

			Assert.That(combined.meshFilter.gameObject.name, Is.EqualTo(name));
			Assert.That(combined.meshFilter.mesh.vertices.Length, Is.EqualTo(24 + 88));

			var meshRenderer = cube1.GetComponent<MeshRenderer>();

			Assert.That(rendererInfo.renderingLayerMask, Is.EqualTo(combined.meshRenderer.renderingLayerMask));
			Assert.That(rendererInfo.shadowCastingMode, Is.EqualTo(combined.meshRenderer.shadowCastingMode));
			Assert.That(rendererInfo.shareMaterial, Is.EqualTo(combined.meshRenderer.sharedMaterial));

			Bounds preCalculatedBounds = GetBounds(gameObjects);
			Assert.That(CompareVector3(preCalculatedBounds.center, combined.meshRenderer.bounds.center));
			Assert.That(CompareVector3(preCalculatedBounds.size, combined.meshRenderer.bounds.size));
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
