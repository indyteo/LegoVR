using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

namespace LegoVR.Scripts {
	public class BrickInventoryMenu : MonoBehaviour {
		[SerializeField]
		private List<BrickMenuItem> bricks;
		[SerializeField]
		private GameObject bricksContainer;
		[SerializeField]
		private GameObject brickPrefab;
		[SerializeField]
		private BrickColorMenu colorMenu;

		private IEnumerator Start() {
			Vector3 topLeft = this.bricksContainer.transform.localPosition;
			float width = Mathf.Abs(topLeft.x) * 2;
			float padding = width * 0.05f;
			float itemWidth = (width - 5 * padding) / 4;
			float offset = itemWidth + padding;
			int i = 0;
			Vector3 position = new Vector3(padding + itemWidth / 2, -padding - itemWidth / 2);
			List<GameObject> instanciatedBricks = new List<GameObject>();
			foreach (BrickMenuItem item in this.bricks) {
				GameObject instance = Instantiate(this.brickPrefab, this.bricksContainer.transform);
				instance.transform.localPosition = position;
				SnapZone snap = instance.GetComponent<SnapZone>();
				GameObject brick = Instantiate(item.Brick, snap.transform.position, Quaternion.identity);
				instanciatedBricks.Add(brick);
				snap.ScaleItem = item.Scale * 0.4f;
				snap.GrabGrabbable(brick.GetComponent<Grabbable>());
				i++;
				if (i < 4)
					position.x += offset;
				else {
					i = 0;
					position.x = padding + itemWidth / 2;
					position.y -= offset;
				}
			}
			this.RefreshColor(LegoManager.Instance.CurrentMaterial);
			yield return null;
			foreach (GameObject brick in instanciatedBricks)
				brick.transform.localPosition = brick.GetComponent<SnapZoneOffset>().LocalPositionOffset;
		}

		private void OnEnable() {
			this.colorMenu.gameObject.SetActive(false);
		}

		public void SelectColor() {
			this.gameObject.SetActive(false);
			this.colorMenu.gameObject.SetActive(true);
		}

		public void RefreshColor(Material material) {
			if (!material)
				return;
			foreach (LegoBrick brick in this.GetComponentsInChildren<LegoBrick>())
				brick.SetMaterial(material);
		}
	}

	[Serializable]
	public class BrickMenuItem {
		[SerializeField] public GameObject Brick;
		[SerializeField] public float Scale = 1;
	}
}
