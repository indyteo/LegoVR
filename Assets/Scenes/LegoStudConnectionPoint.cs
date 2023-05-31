using UnityEngine;

namespace Scenes {
	public class LegoStudConnectionPoint : MonoBehaviour {
		public LegoBrick Brick { get; private set; }
		public LegoStud ConnectedTo { get; set; }

		private void Awake() {
			this.Brick = this.gameObject.GetComponentInParent<LegoBrick>();
		}

		private void OnTriggerEnter(Collider other) {
			LegoStud stud = other.gameObject.GetComponent<LegoStud>();
			if (stud != null) {
				stud.ConnectedTo = this;
				this.ConnectedTo = stud;
			}
		}

		private void OnTriggerExit(Collider other) {
			LegoStud stud = other.gameObject.GetComponent<LegoStud>();
			if (stud != null) {
				stud.ConnectedTo = null;
				this.ConnectedTo = null;
			}
		}
	}
}
