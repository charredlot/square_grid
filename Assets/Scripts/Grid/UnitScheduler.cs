using System.Collections;
using System.Collections.Generic;

public interface UnitSchedulable {
	int GetSpeed();
	void BeginTurn();
	void EndTurn();
};

public class UnitScheduler {
	public static readonly int MAX_SPEED = 99;

	public UnitSchedulable ActiveUnit { get { return this.curr_unit; } }
	
	private UnitSchedulable curr_unit = null;
	private IList<UnitSchedulable> units;
	private Queue<UnitSchedulable>[] calendar;
	private int time;
	
	public UnitScheduler()
	{
		int i;
		
		/* TODO: probably not exactly right */
		this.calendar = new Queue<UnitSchedulable>[(UnitScheduler.MAX_SPEED+1)*2];
		this.time = 0;
		
		for (i=0; i<this.calendar.Length; i++) {
			this.calendar[i] = new Queue<UnitSchedulable>();
		}
		
		this.units = new List<UnitSchedulable>();
	}

	public void AddUnit(UnitSchedulable p)
	{
		this.units.Add (p); // just for debugging i guess
		this.SchedulePiece(p);
	}

	private void SchedulePiece(UnitSchedulable p)
	{
		int target;
		
		target = (this.time + UnitScheduler.MAX_SPEED - p.GetSpeed() + 1) % this.calendar.Length;
		this.calendar[target].Enqueue (p);
	}
	
	private Queue<UnitSchedulable> GetNextQueue()
	{
		Queue<UnitSchedulable> ret = null;
		int count = 0;
		int i;
		
		i = this.time;

		// TODO: keep track of lowest nonzero queue on enqueue
		for (count = 0, i = this.time;
		     count < this.calendar.Length;
		     count++, i = (i+1) % this.calendar.Length) {
			Queue<UnitSchedulable> curr;
			
			curr = this.calendar[i];
			if (curr.Count > 0) {
				ret = curr;
				break;
			}
		}
		
		this.time = i;
		return ret;
	}
	
	private UnitSchedulable DequeueNext()
	{
		Queue<UnitSchedulable> curr;
		
		curr = this.GetNextQueue();
		if (curr != null) {
			return curr.Dequeue();
		} else { 
			return null;
		}
	}

	private void IterNext()
	{
		this.curr_unit = this.DequeueNext();
		this.curr_unit.BeginTurn();
	}
	
	public void Begin()
	{
		this.IterNext();
	}
	
	public void TurnEnded(UnitSchedulable piece)
	{
		if (this.curr_unit != piece) {
			throw new System.MissingFieldException(
				string.Format ("Expected {0}'s turn got {1}", this.curr_unit, piece));
		}

		this.curr_unit.EndTurn ();

		/* reschedule for next turn */
		this.SchedulePiece(piece);
		this.IterNext ();
	}
	
	public delegate void DebugPrint(object msg);
	public void Dump(DebugPrint debug_print)
	{
		Queue<UnitSchedulable> curr;
		int count;
		Queue <UnitSchedulable> first = null;
		
		for (count = 0; count < this.calendar.Length; count++) {
			curr = this.GetNextQueue();
			if (curr == null) {
				break;
			} 
			
			if (first == null) {
				first = curr;
			} else if (first == curr) {
				break;
			}
			
			foreach (UnitSchedulable h in curr) {
				debug_print(this.time.ToString() + " " + h.ToString ());
			}
		}
		debug_print("----done----");
	}
}
