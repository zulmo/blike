using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BombSystem : ComponentSystem 
{
	public struct BombData
	{
		public int Length;
		public ComponentArray<Bomb> Bomb;
		public ComponentArray<TilePosition> TilePosition;
		public EntityArray Entity;
	}

	public struct TileData
	{
		public int Length;
		public ComponentArray<TilePosition> TilePosition;
		public EntityArray Entity;

	}
	[Inject] BombData m_bombs;
	[Inject] TileData m_tiles;

	override protected void OnUpdate()
	{
		List<GameObject> toDestroy = new List<GameObject>();
		float elapsed = Time.deltaTime;
		for(int i = 0; i < m_bombs.Length; i++)
		{
			m_bombs.Bomb[i].Elapsed += elapsed;
			if(m_bombs.Bomb[i].Elapsed > m_bombs.Bomb[i].Timer)
			{
				m_bombs.Bomb[i].Elapsed = 0;
				ExplodeBomb(m_bombs.TilePosition[i], m_bombs.Bomb[i].Range);

				toDestroy.Add(m_bombs.Bomb[i].gameObject);
			} 
		}
		foreach(GameObject go in toDestroy)
		{
			GameObject.Destroy(go);
		}
	}

	private void ExplodeBomb(TilePosition position, int range)
	{
		//spawn fire in every direction until we meet a blocker
		//up
		//for	
	}
}
