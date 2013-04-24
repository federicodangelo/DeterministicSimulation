using UnityEngine;
using DeterministicSimulation;
using System.Collections.Generic;

public class VoxelWorldView : MonoBehaviour
{
	private VoxelWorld voxelWorld;

	private Mesh mesh;
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	protected List<int> triangleCubeMap = new List<int>(); //triangleCubeMap[triangleIndex] = z * (sizeX * sizeY) + y * (sizeX) + x (index into data array!)
	protected List<byte> triangleCubeFaceNumber = new List<byte>(); //triangleCubeMap[triangleIndex] = 0..5 (index into MeshUtils.faceNormalsTile]

	public void Awake () 
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		
		mesh = new Mesh();
	}

	public void Init(VoxelWorld voxelWorld)
	{
		this.voxelWorld = voxelWorld;

		UpdateMesh();
	}

	static private List<int> triangles = new List<int>();
	static private List<Vector3> vertices = new List<Vector3>();
	static private List<Vector3> normals = new List<Vector3>();
	static private List<Vector2> uvs = new List<Vector2>();

	private void UpdateMesh()
	{
		MeshUtils.InitStaticValues();
		
		Vector3[] faceVectors = MeshUtils.faceVectorsNormal;
		IntVector3[] faceNormalsTile = MeshUtils.faceNormalsTile;
		Vector3[] faceNormals = MeshUtils.faceNormals;
		
		mesh.Clear();
		
		vertices.Clear();
		normals.Clear();
		triangles.Clear();
		uvs.Clear();
		
		triangleCubeMap.Clear();
		triangleCubeFaceNumber.Clear();

		int sizeX = voxelWorld.SizeX;
		int sizeY = voxelWorld.SizeY;
		int sizeZ = voxelWorld.SizeZ;

		int index = 0;
		int dataOffset = 0;
		
		Vector3 cubeCenter = MeshUtils.GetCubeCenter(sizeX, sizeY, sizeZ);
		
		for (int z = 0; z < sizeZ; z++)
		{
			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					byte voxel = voxelWorld.GetVoxel(x, y, z);
					
					if (voxel == 0)
						continue;
					
					IntVector3 position = new IntVector3(x, y, z);
					Vector3 offset = new Vector3(x, y, z) * MeshUtils.VOXEL_SIZE - cubeCenter;
					
					float fromTX = (1.0f / 8.0f) * (((int) voxel) % 8);
					float toTX = fromTX + (1.0f / 8.0f);
					
					float fromTY = 1.0f - (1.0f / 8.0f) * (((int) voxel) / 8);
					float toTY = fromTY - (1.0f / 8.0f);
					
					for (int face = 0; face < 6; face++)
					{
						IntVector3 normalInt = faceNormalsTile[face];
						IntVector3 positionNear = position + normalInt;
						
						//We draw this face only if there isn't a visible tile in the direction of the face
						if (!voxelWorld.IsValidPosition(positionNear.x, positionNear.y, positionNear.z) || 
						    voxelWorld.GetVoxel(positionNear.x, positionNear.y, positionNear.z) == 0)
						{
							Vector3 faceNormal = faceNormals[face];
							
							//for (int i = 0; i < 4; i++)
							//{
							//    normals.Add(faceNormal);
							//    vertices.Add(faceVectors[(face << 2) + i] + offset);
							//}
							
							//START MANUAL LOOP UNROLLING OF LOOP ABOVE
							normals.Add(faceNormal);
							normals.Add(faceNormal);
							normals.Add(faceNormal);
							normals.Add(faceNormal);
							
							vertices.Add(faceVectors[(face << 2) + 0] + offset);
							vertices.Add(faceVectors[(face << 2) + 1] + offset);
							vertices.Add(faceVectors[(face << 2) + 2] + offset);
							vertices.Add(faceVectors[(face << 2) + 3] + offset);
							//END MANUAL LOOP UNROLLING
							
							uvs.Add(new Vector2(fromTX, fromTY));
							uvs.Add(new Vector2(fromTX, toTY));
							uvs.Add(new Vector2(toTX, toTY));
							uvs.Add(new Vector2(toTX, fromTY));
							
							triangles.Add(index + 0);
							triangles.Add(index + 1);
							triangles.Add(index + 2);
							
							triangles.Add(index + 2);
							triangles.Add(index + 3);
							triangles.Add(index + 0);
							
							triangleCubeMap.Add(dataOffset - 1);
							triangleCubeMap.Add(dataOffset - 1);
							
							triangleCubeFaceNumber.Add((byte) face);
							triangleCubeFaceNumber.Add((byte) face);
							
							index += 4;
						}
					}
				}
			}
		}
		
		mesh.vertices = vertices.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.normals = normals.ToArray();
		mesh.triangles = triangles.ToArray();
		
		if (meshFilter)
			meshFilter.sharedMesh = mesh;
		
		if (meshCollider)
		{
			meshCollider.sharedMesh = null;
			meshCollider.sharedMesh = mesh;
		}
	}

	public void OnDrawGizmosSelected()
	{
		OnDrawGizmos();
	}
	
	public void OnDrawGizmos()
	{
		if (voxelWorld != null)
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(voxelWorld.SizeX * MeshUtils.VOXEL_SIZE, voxelWorld.SizeY * MeshUtils.VOXEL_SIZE, voxelWorld.SizeZ * MeshUtils.VOXEL_SIZE));
		}
	}
}


