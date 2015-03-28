using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SquareGridArea : IEnumerable {
	public readonly HashSet<SquareGridSquare> Squares;
	public readonly GraphableMixin<SquareGridSquare> RootVertex;
	public readonly IEnumerable<GraphableMixin<SquareGridSquare>> Vertices;

	public SquareGridArea(HashSet<SquareGridSquare> squares,
	                      SquareGridSquare root_square) {
		this.Squares = squares;
		this.RootVertex = root_square.TmpVertex;

		/* need to append root because it might not be in squares */
		var all = new List<GraphableMixin<SquareGridSquare>>(
			from s in squares select s.TmpVertex);
		this.Vertices = all;
	}

	public void ResetForSearch() {
		foreach (var s in this.Squares) {
			s.TmpVertex.ResetForSearch();
		}
	}

	public IEnumerable<SquareGridSquare> GetPath(SquareGridSquare dst) {
		return (from v in this.RootVertex.GetPathTo(this.Vertices, dst.TmpVertex)
			select v.Parent);
	}
	
	IEnumerator IEnumerable.GetEnumerator() {
		return this.Squares.GetEnumerator();
	}

	public IEnumerable<SquareGridSquare> GetEnumerable() {
		foreach (var s in this.Squares) {
			yield return s;
		}
	}
}
