using BNG;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes {
	public class LegoBrick : GrabbableEvents {
		[SerializeField]
		public Material initialMaterial;

		private LegoStud[] studs;
		private LegoStudConnectionPoint[] connectionPoints;
		private new Rigidbody rigidbody;
		private MeshRenderer[] renderers;
		private bool breakable;

		private const float BREAKABLE_FORCE = 50;
		private const float UNBREAKABLE_FORCE = 500;

		protected override void Awake() {
			this.studs = this.GetComponentsInChildren<LegoStud>();
			this.connectionPoints = this.GetComponentsInChildren<LegoStudConnectionPoint>();
			this.rigidbody = this.GetComponent<Rigidbody>();
			this.renderers = this.GetComponentsInChildren<MeshRenderer>();
			base.Awake();
			if (this.initialMaterial)
				this.SetMaterial(this.initialMaterial);
		}

		public override void OnTriggerDown() {
			this.UpdateBreakability(true);
		}

		public override void OnTriggerUp() {
			this.UpdateBreakability(false);
		}

		public override void OnGrab(Grabber grabber) {
			base.OnGrab(grabber);
			this.UpdateBreakability(IsTriggerPressed());
		}

		public override void OnRelease() {
			this.CheckForConnections();
		}

		private void CheckForConnections() {
			foreach (LegoStud stud in this.studs) {
            	LegoStudConnectionPoint connectionPoint = stud.ConnectedTo;
            	if (connectionPoint != null && !connectionPoint.Joint) {
	                CreateJoint(stud, connectionPoint);
	                connectionPoint.Brick.CheckForConnections();
                }
            }
            foreach (LegoStudConnectionPoint connectionPoint in this.connectionPoints) {
            	LegoStud stud = connectionPoint.ConnectedTo;
            	if (stud != null && !connectionPoint.Joint) {
	                CreateJoint(stud, connectionPoint);
	                stud.Brick.CheckForConnections();
                }
            	if (connectionPoint.Joint)
            		connectionPoint.Joint.breakForce = UNBREAKABLE_FORCE;
            }
			this.breakable = false;
		}

		private bool IsTriggerPressed() {
			if (!this.thisGrabber)
				return false;
			return (this.thisGrabber.HandSide == ControllerHand.Left ? this.input.LeftTrigger : this.input.RightTrigger) > 0.2f;
		}

		private void UpdateBreakability(bool newBreakable) {
			if (this.breakable == newBreakable)
				return;
			this.breakable = newBreakable;
			foreach (LegoStudConnectionPoint connectionPoint in this.connectionPoints)
				if (connectionPoint.Joint)
					connectionPoint.Joint.breakForce = newBreakable ? BREAKABLE_FORCE : UNBREAKABLE_FORCE;
		}

		private static void CreateJoint(LegoStud stud, LegoStudConnectionPoint connectionPoint) {
			connectionPoint.Joint = connectionPoint.Brick.AddComponent<FixedJoint>();
			connectionPoint.Joint.connectedBody = stud.Brick.rigidbody;
			connectionPoint.Joint.enableCollision = true;
			connectionPoint.Joint.breakForce = UNBREAKABLE_FORCE;
			LegoManager.PlayConnectSound(stud.transform.position);
		}

		private void OnJointBreak(float breakForce) {
			LegoManager.PlayDisconnectSound(this.transform.position);
		}

		public void SetMaterial(Material material) {
			foreach (MeshRenderer r in this.renderers)
				r.sharedMaterial = material;
		}

		public void UpdateMaterial() {
			this.SetMaterial(LegoManager.Instance.CurrentMaterial);
		}
	}
}
