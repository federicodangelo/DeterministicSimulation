using UnityEngine;
using DeterministicSimulation;
using System.Collections.Generic;

public class VoxelEntityView : MonoBehaviour
{
	private VoxelEntity voxelEntity;
	private Transform trans;

	private Vector3 lastPosition;
	private Vector3 nextPosition;
	private float lastPositionTime;
	private float lastPositionTimeDelta;

	public void Awake()
	{
		trans = transform;
	}

	public void Init(VoxelEntity voxelEntity)
	{
		this.voxelEntity = voxelEntity;

		gameObject.name = voxelEntity.GetType().Name.ToString() + "-" + voxelEntity.Id;

		UpdateMesh();
	}

	private void UpdateMesh()
	{
		GameObject view = null;

		switch(voxelEntity.shape)
		{
			case VoxelEntityShape.Sphere:
				view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				view.transform.parent = trans;
				view.transform.localPosition = Vector3.zero;
				view.transform.localRotation = Quaternion.identity;
				view.transform.localScale = Vector3.one * voxelEntity.radius.ToFloat() * 2.0f;
				break;

			case VoxelEntityShape.Cylinder:
				view = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				view.transform.parent = trans;
				view.transform.localPosition = Vector3.zero;
				view.transform.localRotation = Quaternion.identity;
				view.transform.localScale = new Vector3(voxelEntity.radius.ToFloat() * 2.0f, voxelEntity.height.ToFloat() * 0.5f, voxelEntity.radius.ToFloat() * 2.0f);
				break;
		}

		if (view != null)
		{
			Vector3 colorVector3 = voxelEntity.color.ToVector3();
			view.renderer.material.color = new Color(colorVector3.x, colorVector3.y, colorVector3.z);
		}

		UpdatePosition(true);
	}

	public void Update()
	{
		UpdatePosition(false);
	}

	public void UpdatePosition(bool firstTime)
	{
		if (firstTime)
		{
			lastPosition = nextPosition = voxelEntity.position.ToVector3();
			lastPositionTime = 0.0f;
			lastPositionTimeDelta = 0.0f;

			trans.localPosition = lastPosition * MeshUtils.VOXEL_SIZE;
		}
		else
		{
			if (voxelEntity.position.ToVector3() != nextPosition)
			{
				trans.position = nextPosition;

				lastPosition = nextPosition;
				nextPosition = voxelEntity.position.ToVector3();

				lastPositionTime = SimulationTime.deltaTime.ToFloat();
				lastPositionTimeDelta = 0.0f;
			}
			else if (lastPositionTimeDelta < lastPositionTime)
			{
				lastPositionTimeDelta += Time.deltaTime;
				if (lastPositionTimeDelta > lastPositionTime)
					lastPositionTimeDelta = lastPositionTime;

				trans.position = Vector3.Lerp(lastPosition, nextPosition, lastPositionTimeDelta / lastPositionTime);
			}
		}
	}
}
