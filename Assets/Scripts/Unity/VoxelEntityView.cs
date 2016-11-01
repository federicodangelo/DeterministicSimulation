using UnityEngine;
using DeterministicSimulation;
using System.Collections.Generic;

public class VoxelEntityView : MonoBehaviour
{
	private VoxelComponent voxelComponent;
	private Transform trans;

	private Vector3 lastPosition;
	private Vector3 nextPosition;
	private float lastPositionTime;
	private float lastPositionTimeDelta;

	public void Awake()
	{
		trans = transform;
	}

	public void Init(VoxelComponent voxelComponent)
	{
		this.voxelComponent = voxelComponent;

		gameObject.name = voxelComponent.GetType().Name.ToString() + "-" + voxelComponent.Entity.Id;

		UpdateMesh();
	}

	private void UpdateMesh()
	{
		GameObject view = null;

		switch(voxelComponent.shape)
		{
			case VoxelShape.Sphere:
				view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				view.transform.parent = trans;
				view.transform.localPosition = Vector3.zero;
				view.transform.localRotation = Quaternion.identity;
				view.transform.localScale = Vector3.one * voxelComponent.radius.ToFloat() * 2.0f;
				break;

			case VoxelShape.Cylinder:
				view = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				view.transform.parent = trans;
				view.transform.localPosition = Vector3.zero;
				view.transform.localRotation = Quaternion.identity;
				view.transform.localScale = new Vector3(voxelComponent.radius.ToFloat() * 2.0f, voxelComponent.height.ToFloat() * 0.5f, voxelComponent.radius.ToFloat() * 2.0f);
				break;
		}

		if (view != null)
		{
			Vector3 colorVector3 = voxelComponent.color.ToVector3();
			view.GetComponent<Renderer>().material.color = new Color(colorVector3.x, colorVector3.y, colorVector3.z);
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
			lastPosition = nextPosition = voxelComponent.position.ToVector3();
			lastPositionTime = 0.0f;
			lastPositionTimeDelta = 0.0f;

			trans.localPosition = lastPosition * MeshUtils.VOXEL_SIZE;
		}
		else
		{
			if (voxelComponent.position.ToVector3() != nextPosition)
			{
				trans.localPosition = nextPosition;

				lastPosition = nextPosition;
				nextPosition = voxelComponent.position.ToVector3();

				lastPositionTime = SimulationTime.deltaTime.ToFloat();
				lastPositionTimeDelta = 0.0f;
			}
			else if (lastPositionTimeDelta < lastPositionTime)
			{
				lastPositionTimeDelta += Time.deltaTime;
				if (lastPositionTimeDelta > lastPositionTime)
					lastPositionTimeDelta = lastPositionTime;

				trans.localPosition = Vector3.Lerp(lastPosition, nextPosition, lastPositionTimeDelta / lastPositionTime);
			}
		}
	}
}
