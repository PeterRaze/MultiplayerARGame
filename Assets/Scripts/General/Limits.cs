using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Limits : MonoBehaviour
{
	private float offset = 0.011f;

	[SerializeField]
	private List<TransformExtention> transformExtentions = new List<TransformExtention>();

	[Serializable]
	public struct TransformExtention
	{
		public enum Axis
		{
			MaxX,
			MaxZ,
			MinX,
			MinZ
		}

		public Transform transform;
		public Axis axis;

		public float GetLimit()
		{
			switch (axis)
			{
				case Axis.MaxX:
				case Axis.MinX:
					return transform.position.x;
				case Axis.MaxZ:
				case Axis.MinZ:
					return transform.position.z;
				default:
					return 0;
			}
		}
	}

	public float GetTransformExtentions(TransformExtention.Axis axis)
	{
		var limit = transformExtentions.Where(x => x.axis == axis).FirstOrDefault().GetLimit();

		limit = limit > 0 ? limit -= offset : limit += offset;

		return limit;
	}

}
