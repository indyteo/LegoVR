using System.Collections.Generic;
using BNG;
using UnityEngine;

namespace LegoVR.Scripts {
	public class BricksGraph {
		private int nodeIds;
		private Dictionary<LegoBrick, BrickGraphNode> nodes = new Dictionary<LegoBrick, BrickGraphNode>();

		public BrickGraphNode this[LegoBrick brick] {
			get {
				BrickGraphNode node;
				if (!this.nodes.TryGetValue(brick, out node))
					this.nodes.Add(brick, node = new BrickGraphNode(++this.nodeIds, brick));
				return node;
			}
		}

		public void Connect(LegoBrick brick, LegoBrick parent) {
			VRUtils.Instance.Log("Connect");
			BrickGraphNode node = this[brick];
			BrickGraphNode parentNode = this[parent];
			BrickGraphNode root = node.Root;
			BrickGraphNode parentRoot = parentNode.Root;
			node.Parents.Add(parentNode);
			parentNode.Children.Add(node);
			if (root != parentRoot)
				ConnectBricks(root.Brick, parentRoot.Brick);
		}

		public void Disconnect(LegoBrick brick) {
			BrickGraphNode node = this[brick];
			foreach (BrickGraphNode child in node.Children) {
				child.Parents.Remove(node);
				if (child.Parents.Count == 0)
					DisconnectBrick(child.Brick);
			}
			node.Children.Clear();
			foreach (BrickGraphNode parent in node.Parents)
				parent.Children.Remove(node);
			node.Parents.Clear();
			DisconnectBrick(brick);
		}

		private static void ConnectBricks(LegoBrick brick, LegoBrick root) {
			VRUtils.Instance.Log("OK");
			brick.transform.parent = root.transform;
			brick.rigidbody.isKinematic = true;
			// TODO Disable rigidbody
		}

		private static void DisconnectBrick(LegoBrick brick) {
			brick.transform.parent = null;
			brick.rigidbody.isKinematic = false;
			// TODO Enable rigidbody
		}
	}

	public class BrickGraphNode {
		private readonly int id;
		public LegoBrick Brick { get; private set; }
		public SortedSet<BrickGraphNode> Parents { get; private set; }
		public SortedSet<BrickGraphNode> Children { get; private set; }

		public BrickGraphNode Root => this.Parents.Count == 0 ? this : this.Parents.Min.Root;

		public BrickGraphNode(int id, LegoBrick brick) {
			this.id = id;
			this.Brick = brick;
			this.Parents = new SortedSet<BrickGraphNode>();
			this.Children = new SortedSet<BrickGraphNode>();
		}

		public bool Equals(BrickGraphNode other) {
			return id == other.id;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BrickGraphNode) obj);
		}

		public override int GetHashCode() {
			return this.id;
		}

		public static bool operator ==(BrickGraphNode left, BrickGraphNode right) {
			return Equals(left, right);
		}

		public static bool operator !=(BrickGraphNode left, BrickGraphNode right) {
			return !Equals(left, right);
		}
	}
}
