using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace LeonDrace.Utility.Helpers
{
	public static class CameraHelper
	{
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
	}
}
