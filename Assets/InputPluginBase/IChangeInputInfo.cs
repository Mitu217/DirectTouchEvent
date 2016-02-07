namespace InputSupport
{
	public interface IChangeInputInfo<T> {
		void Add (T info);
		void AddAll (System.Collections.Generic.IEnumerable<T> info);
		void Remove (T info);
		void RemoveAt (int index);
		void RemoveAll (System.Collections.Generic.IEnumerable<T> info);
		void Clear ();
		void Update (T info);
		void UpdateAll (System.Collections.Generic.IEnumerable<T> info);
	}
}
