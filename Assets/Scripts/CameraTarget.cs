using System;
using UnityEngine;

namespace UI
{
	[Serializable]
	public class CameraTarget
	{
		public Transform target;
		public float zoomLevel;
		public int priority;
		public bool RotateWithPlayer;
		public bool Interpolate;
	}
}