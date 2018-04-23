using Unity.Entities;
using Unity.Mathematics;

public class MoveSystem : ComponentSystem 
{
	public struct Movers
	{
		public int Length;
		public ComponentArray<Move> Move;
		public ComponentArray<TilePosition> TilePosition;
	}

	public struct Blockers
	{
		public int Length;
		public ComponentArray<Blocker> Blocker;
		public ComponentArray<TilePosition> TilePosition;
	}

	[Inject] Movers m_movers;
	[Inject] Blockers m_blockers;

	protected override void OnUpdate()
	{
		for(int i = 0; i < m_movers.Length; i++)
		{
			float2 destination = m_movers.Move[i].Destination;
			bool isValid = true;
			for(int j = 0; j < m_blockers.Length; j++)
			{
				bool2 compare = m_blockers.TilePosition[j].Value == destination; 
				if(compare.x && compare.y)
				{
					isValid = false;
					break;
				}
			}

			if(isValid)
			{
				m_movers.TilePosition[i].Value = m_movers.Move[i].Destination;
			}
		}
	}
}
