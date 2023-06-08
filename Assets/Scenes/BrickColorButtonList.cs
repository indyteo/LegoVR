using System.Collections.Generic;
using UnityEngine;

namespace Scenes {
	public class BrickColorButtonList : MonoBehaviour {
		[SerializeField]
		private List<Material> materials;
		[SerializeField]
		private GameObject buttonPrefab;

		private void Start() {
			foreach (Material material in this.materials) {
				GameObject button = Instantiate(this.buttonPrefab, this.transform);
				button.GetComponent<BrickColorButton>().Material = material;
			}
		}
	}
}
