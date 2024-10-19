using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeonDrace.Utility.Helpers
{
	public static class CombineHelper
	{
		/// <summary>
		/// Combine gameobjects into one.
		/// Creates the <see cref="RendererInfo"/>> from the first object.
		/// </summary>
		/// <param name="gameObjects"></param>
		/// <param name="combinedName"></param>
		/// <param name="applyLocalToWorldMatrix"></param>
		/// <returns>New objects <see cref="MeshInfo"/></returns>
		public static MeshInfo Combine(ReadOnlySpan<GameObject> gameObjects, string combinedName)
		{
			CombineInstance[] combineInstances = CreateCombineInstances(gameObjects);

			RendererInfo rendererInfo = new RendererInfo();
			rendererInfo.FromGameObject(gameObjects[0]);

			return Combine(combineInstances, rendererInfo, combinedName);
		}

		/// <summary>
		/// Combine gameobjects into one.
		/// </summary>
		/// <param name="gameObjects"></param>
		/// <param name="combinedName"></param>
		/// <param name="applyLocalToWorldMatrix"></param>
		/// <returns></returns>
		public static MeshInfo Combine(ReadOnlySpan<GameObject> gameObjects, RendererInfo rendererInfo, string combinedName)
		{
			CombineInstance[] combineInstances = CreateCombineInstances(gameObjects);
			return Combine(combineInstances, rendererInfo, combinedName);
		}

		private static CombineInstance[] CreateCombineInstances(ReadOnlySpan<GameObject> gameObjects)
		{
			CombineInstance[] combineInstances = new CombineInstance[gameObjects.Length];
			for (int i = 0; i < gameObjects.Length; i++)
			{
				combineInstances[i].mesh = gameObjects[i].GetComponentInChildren<MeshFilter>().sharedMesh;
				combineInstances[i].transform = gameObjects[i].transform.localToWorldMatrix;
				gameObjects[i].SetActive(false);
			}
			return combineInstances;
		}

		/// <summary>
		/// Combine the instances into one object and apply the renderer info.
		/// </summary>
		/// <param name="combineInstances"></param>
		/// <param name="rendererInfo"></param>
		/// <param name="combinedName"></param>
		/// <returns></returns>
		public static MeshInfo Combine(CombineInstance[] combineInstances, RendererInfo rendererInfo, string combinedName)
		{
			var combined = CreateNewGameObject(combinedName);
			combined.meshFilter.mesh.CombineMeshes(combineInstances);
			rendererInfo.ApplyTo(combined);

			return combined;
		}

		private static MeshInfo CreateNewGameObject(string name)
		{
			GameObject newGameObject = new GameObject(name);
			var filter = newGameObject.AddComponent<MeshFilter>();
			filter.mesh = new();
			return new MeshInfo() { meshFilter = filter, meshRenderer = newGameObject.AddComponent<MeshRenderer>() };
		}

		public struct MeshInfo
		{
			public MeshFilter meshFilter;
			public MeshRenderer meshRenderer;
		}

		public struct RendererInfo
		{
			public Material shareMaterial;
			public uint renderingLayerMask;
			public ShadowCastingMode shadowCastingMode;
			public MaterialPropertyBlock materialPropertyBlock;

			public void FromGameObject(GameObject go)
			{
				var renderer = go.GetComponentInChildren<MeshRenderer>();

				shareMaterial = renderer.sharedMaterial;
				renderingLayerMask = renderer.renderingLayerMask;
				shadowCastingMode = renderer.shadowCastingMode;
			}

			public void ApplyTo(MeshInfo info)
			{
				info.meshRenderer.shadowCastingMode = shadowCastingMode;
				info.meshRenderer.renderingLayerMask = renderingLayerMask;
				info.meshRenderer.sharedMaterial = shareMaterial;
				if (materialPropertyBlock != null)
				{
					info.meshRenderer.SetPropertyBlock(materialPropertyBlock);
				}
			}
		}
	}
}
