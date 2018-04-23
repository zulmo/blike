using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;

public class BlikeGame : MonoBehaviour 
{
	public static GameObject Player = null;
	public static GameObject Tile = null;
	public static GameObject Wall = null;
	public static GameObject Bomb = null;

	public GameObject P = null;
	public GameObject T = null;
	public GameObject W = null;
	public GameObject B = null;

	EntityManager entityManager;
	public float2 GridSize = new float2(40,20);

	// Use this for initialization
	void Start () 
	{
		Player = P;
		Tile = T;
		Wall = W;
		Bomb = B;
		
		entityManager = World.Active.GetOrCreateManager<EntityManager>();
		CreateTiles();
		CreateWalls();
		CreatePlayer();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void CreateTiles()
	{
		for(int i=0;i<40;i++)
		{
			for(int j=0;j<20;j++)
			{
				GameObject tile = GameObject.Instantiate(Tile, Vector3.zero, Quaternion.identity);
				TilePosition tilePosition = tile.GetComponent<TilePosition>();
				tilePosition.Value = new float2(i,j);
			}
		}
	}

	void CreateWalls()
	{
		//Create walls randomly 1 chance on 3
		for(int i=0;i<40;i++)
		{
			for(int j=0;j<20;j++)
			{
				if(Random.Range(0, 100) > 66)
				{
					GameObject wall = GameObject.Instantiate(Wall, Vector3.zero, Quaternion.identity);
					TilePosition tilePosition = wall.GetComponent<TilePosition>();
					tilePosition.Value = new float2(i,j);
				}
				
			}
		}
	}

	void CreatePlayer()
	{
		GameObject player = GameObject.Instantiate(Player, Vector3.zero, Quaternion.identity);
		TilePosition tilePosition = player.GetComponent<TilePosition>();
		tilePosition.Value = new float2(0,0);
	}
}
