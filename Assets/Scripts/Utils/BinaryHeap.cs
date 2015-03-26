using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BinaryHeap<T> : IEnumerable<T> {
	public int MaxCount { get { return this.values.Length; } }
	public int Count { get { return this.curr_count; } }

	private int curr_count;
	private T[] values;


	public BinaryHeap(int max_values) {
		this.values = new T[max_values];
		this.curr_count = 0;
	}

	public BinaryHeap(IEnumerable<T> items) {
		this.values = items.ToArray();
		this.curr_count = this.values.Length;
	}

	public T PopRoot() {
		return default(T); // FIXME
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator() {
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return null;
	}

}
