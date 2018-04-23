using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerInputSystem : ComponentSystem
{
	public struct Data
	{
		public int Length;
		public ComponentArray<Move> Move;
		public ComponentArray<TilePosition> TilePosition;
	}

	[Inject] Data m_data;

	protected override void OnUpdate()
	{
		for(int i = 0; i < m_data.Length; i++)
		{
			float2 delta = new float2(0,0);

			if(Input.GetKeyDown(KeyCode.W))
			{
				delta.y = -1;
			}
			else if(Input.GetKeyDown(KeyCode.A))
			{
				delta.x = -1;
			}
			else if(Input.GetKeyDown(KeyCode.S))
			{
				delta.y = 1;
			}
			else if(Input.GetKeyDown(KeyCode.D))
			{
				delta.x = 1;
			}
			else if(Input.GetKeyDown(KeyCode.Space))
			{
				//drop bomb
				GameObject bomb = GameObject.Instantiate(BlikeGame.Bomb, Vector3.zero, Quaternion.identity);
				TilePosition pos = bomb.GetComponent<TilePosition>();
				pos.Value = m_data.TilePosition[i].Value;
			}

			m_data.Move[i].Destination = m_data.TilePosition[i].Value + delta;
		}
	}
}
