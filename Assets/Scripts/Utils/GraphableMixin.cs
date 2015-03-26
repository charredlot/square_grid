using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GraphableMixin<T> {
	public readonly T Parent;
	public IEnumerable<T> Adjacent { 
		get { return from m in this.adjacent_m select m.Parent; } 
	}

	/* for A-star and dijkstra's */
	protected GraphableMixin<T> prev;
	protected float score;

	private List<GraphableMixin<T>> adjacent_m;

	public delegate int Cmp(T left, T right);

	public GraphableMixin(T parent) {
		this.Parent = parent;
		this.adjacent_m = new List<GraphableMixin<T>>();
	}
	
	public void ResetForSearch() {
		this.prev = null;
		this.score = int.MaxValue;
	}

	public IEnumerable<GraphableMixin<T>> GetPathTo(
		IEnumerable<GraphableMixin<T>> vertices, GraphableMixin<T> dst) {
		GraphableMixin<T> curr;
		var all = new List<GraphableMixin<T>>(vertices);

		this.score = 0;
		this.prev = null;
		all.Sort (delegate(GraphableMixin<T> l, GraphableMixin<T> r) {
			return l.score.CompareTo(r.score);
		});

		while (all.Count > 0) {
			curr = all[0];
			all.RemoveAt(0);
			if (curr == dst) {
				break;
			}

			foreach (var adj in curr.adjacent_m) {
				float new_score = curr.score + 1; // TODO: add edge weights

				if (new_score < adj.score) {
					adj.score = new_score;
					adj.prev = curr;
				}
			}

			all.Sort (delegate(GraphableMixin<T> l, GraphableMixin<T> r) {
				return l.score.CompareTo(r.score);
			});
		}

		var s = new Stack<GraphableMixin<T>>();
		curr = dst;
		do {
			s.Push (curr);
			curr = curr.prev;
		} while (curr != null);
		return s;
	}

	public void GetShortestPath(
		IEnumerable<GraphableMixin<T>> vertices,
		GraphableMixin<T> src, GraphableMixin<T> dst) {
		var heap = new BinaryHeap<GraphableMixin<T>>(vertices);

		src.score = 0; // src distance starts at 0, assume everyone's been inited

		while (heap.Count > 0) {
			GraphableMixin<T> curr;

			curr = heap.PopRoot();
			foreach (var m in curr.adjacent_m) {
				float dist;

				/* TODO: some delegate to calculate distance */
				dist = (float)curr.score + 1;
				if (dist < m.score) {
					m.score = dist;
					m.prev = curr;
				}
			}
		}
	}

	public void Reset() {
		this.adjacent_m.Clear();
		this.ResetForSearch();
	}

	public void AddAdjacent(GraphableMixin<T> node) {
		this.adjacent_m.Add(node);
	}
}
