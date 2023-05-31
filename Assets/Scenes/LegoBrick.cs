using BNG;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes {
	public class LegoBrick : GrabbableEvents {
		private LegoStud[] studs;
		private LegoStudConnectionPoint[] connectionPoints;
		private new Rigidbody rigidbody;

		protected override void Awake() {
			this.studs = this.GetComponentsInChildren<LegoStud>();
			this.connectionPoints = this.GetComponentsInChildren<LegoStudConnectionPoint>();
			this.rigidbody = this.GetComponent<Rigidbody>();
		}

		public override void OnRelease() {
			foreach (LegoStud stud in this.studs) {
				LegoStudConnectionPoint connectionPoint = stud.ConnectedTo;
				if (connectionPoint != null)
					connectionPoint.Brick.CreateJoint(this);
			}
			foreach (LegoStudConnectionPoint connectionPoint in this.connectionPoints) {
				LegoStud stud = connectionPoint.ConnectedTo;
				if (stud != null)
					this.CreateJoint(stud.Brick);
			}

			base.OnRelease();
		}

		private void CreateJoint(LegoBrick brick) {
			Joint joint = this.AddComponent<FixedJoint>();
			joint.connectedBody = brick.rigidbody;
			joint.enableCollision = true;
			joint.breakForce = 150;
		}
	}
}
