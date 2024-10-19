using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LeonDrace.Utility.Helpers
{
	public static class CameraHelper
	{
		/// <summary>
		/// Convert the world position to the screen point.
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="wordlPosition"></param>
		/// <returns></returns>
		public static Vector3 WorldToScreenPoint(Camera camera, Vector3 wordlPosition)
		{
			Matrix4x4 mat = camera.projectionMatrix * camera.worldToCameraMatrix;
			Vector4 temp = mat * new Vector4(wordlPosition.x, wordlPosition.y, wordlPosition.z, 1f);

			if (temp.w == 0f)
			{
				return Vector3.zero;
			}
			else
			{
				temp.x = (temp.x / temp.w + 1f) * .5f * camera.pixelWidth;
				temp.y = (temp.y / temp.w + 1f) * .5f * camera.pixelHeight;
				return new Vector3(temp.x, temp.y, wordlPosition.z);
			}
		}

#if ENABLE_BURST
		[Unity.Burst.BurstCompile]
#endif
		/// <summary>
		/// Convert the world positions to the screen points in a parallel job.
		/// </summary>
		public struct WorldToScreenPointJob : IJobParallelFor
		{
			public float m_screenWidth;
			public float m_screenHeight;
			public Matrix4x4 m_cameraMatrix; // projectionMatrix * worldToCameraMatrix
			public NativeArray<Vector3> m_worldPositions;
			public NativeArray<Vector3> m_results;

			public void Execute(int index)
			{
				Vector4 temp = m_cameraMatrix * new Vector4(this.m_worldPositions[index].x, this.m_worldPositions[index].y, this.m_worldPositions[index].z, 1f);

				if (temp.w == 0f)
				{
					this.m_results[index] = Vector3.zero;
				}
				else
				{
					temp.x = (temp.x / temp.w + 1f) * .5f * m_screenWidth;
					temp.y = (temp.y / temp.w + 1f) * .5f * m_screenHeight;
					this.m_results[index] = new Vector3(temp.x, temp.y, this.m_worldPositions[index].z);
				}
			}
		}

		/// <summary>
		/// Check if a point is in the viewport.
		/// </summary>
		/// <param name="screenPoint"></param>
		/// <param name="minWidth"></param>
		/// <param name="minHeight"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		public static bool IsScreenPointInViewport(Vector3 screenPoint, Vector4 viewport)
		{
			return IsScreenPointInViewport(screenPoint, viewport.x, viewport.y, viewport.z, viewport.w);
		}

		/// <summary>
		/// Check if a point is in the viewport.
		/// </summary>
		/// <param name="screenPoint"></param>
		/// <param name="minWidth"></param>
		/// <param name="minHeight"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		public static bool IsScreenPointInViewport(Vector3 screenPoint, float minWidth, float maxWidth, float minHeight, float maxHeight)
		{
			return screenPoint.x > minWidth && screenPoint.x < maxWidth
				   && screenPoint.y > minHeight && screenPoint.y < maxHeight;
		}

		/// <summary>
		/// Get the screen dimensions with an additional pixel offset.
		/// </summary>
		/// <param name="canvasScale"></param>
		/// <param name="pixelOffset"></param>
		/// <returns><see cref="Vector4"/> containing: min. width, max. width, min. height, max. height.</returns>
		public static Vector4 GetViewportWithOffset(float canvasScale, float pixelOffset)
		{
			pixelOffset *= canvasScale;
			float minWidth = -pixelOffset;
			float minHeight = -pixelOffset;
			float maxWidth = Screen.width + pixelOffset;
			float maxHeight = Screen.height + pixelOffset;
			return new Vector4(minWidth, maxWidth, minHeight, maxHeight);
		}
	}
}
