using BNG;
using Unity.VisualScripting;
using UnityEngine;

namespace LegoVR.Scripts {
	public class LegoBrick : GrabbableEvents {
		[SerializeField]
		public Material initialMaterial;

		public new Rigidbody rigidbody { get; private set; }

		private LegoStud[] studs;
		private LegoStudConnectionPoint[] connectionPoints;
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
			this.UpdateBreakability(this.IsTriggerPressed());
		}

		public override void OnRelease() {
			this.thisGrabber = null;
			this.CheckForConnections();
		}

		private void CheckForConnections() {
			foreach (LegoStud stud in this.studs) {
            	LegoStudConnectionPoint connectionPoint = stud.ConnectedTo;
            	if (connectionPoint != null && !connectionPoint.Connected) {
	                CreateJoint(stud, connectionPoint);
	                connectionPoint.Brick.CheckForConnections();
                }
            }
            foreach (LegoStudConnectionPoint connectionPoint in this.connectionPoints) {
            	LegoStud stud = connectionPoint.ConnectedTo;
            	if (stud != null && !connectionPoint.Connected) {
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

		private void UpdateBreakability(bool isBreakable) {
			if (this.breakable == isBreakable)
				return;
			this.breakable = isBreakable;
			foreach (LegoStudConnectionPoint connectionPoint in this.connectionPoints)
				if (connectionPoint.Joint)
					connectionPoint.Joint.breakForce = isBreakable ? BREAKABLE_FORCE : UNBREAKABLE_FORCE;
			/*if (isBreakable) {
				foreach (LegoStudConnectionPoint connectionPoint in this.connectionPoints)
					connectionPoint.Connected = false;
				LegoManager.Instance.Bricks.Disconnect(this);
				LegoManager.PlayDisconnectSound(this.transform.position);
			} else
				this.CheckForConnections();*/
		}

		private static void CreateJoint(LegoStud stud, LegoStudConnectionPoint connectionPoint) {
			connectionPoint.Joint = connectionPoint.Brick.AddComponent<FixedJoint>();
			connectionPoint.Joint.connectedBody = stud.Brick.rigidbody;
			connectionPoint.Joint.enableCollision = true;
			connectionPoint.Joint.breakForce = UNBREAKABLE_FORCE;
			/*connectionPoint.Connected = true;
			LegoManager.Instance.Bricks.Connect(connectionPoint.Brick, stud.Brick);*/
			LegoManager.PlayConnectSound(stud.transform.position);
		}

		private void OnJointBreak(float breakForce) {
			LegoManager.PlayDisconnectSound(this.transform.position);
			if (this.thisGrabber)
				this.input.VibrateController(0.1f, 0.1f, 0.1f, this.thisGrabber.HandSide);
		}

		public void SetMaterial(Material material) {
			foreach (MeshRenderer r in this.renderers)
				r.material = material;
		}

		public void UpdateMaterial() {
			this.SetMaterial(LegoManager.Instance.CurrentMaterial);
		}
	}
}
