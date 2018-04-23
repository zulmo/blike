using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class SyncTransformSystem : ComponentSystem
{
	public struct Data
	{
		public int Length;
		public ComponentArray<TilePosition> Position;
		public ComponentArray<Transform> Output;
	}

	[Inject] Data m_data;

	protected override void OnUpdate()
	{
		for(int i = 0; i < m_data.Length; i++)
		{
			float2 p = m_data.Position[i].Value;
			m_data.Output[i].position = new Vector3(1f * p.x , 0, -1f * p.y);
		}
	}
}
