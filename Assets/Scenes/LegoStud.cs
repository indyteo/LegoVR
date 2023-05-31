﻿using UnityEngine;

namespace Scenes {
	public class LegoStud : MonoBehaviour {
		public LegoBrick Brick { get; private set; }
		public LegoStudConnectionPoint ConnectedTo { get; set; }

		private void Awake() {
			this.Brick = this.gameObject.GetComponentInParent<LegoBrick>();
		}
	}
}
