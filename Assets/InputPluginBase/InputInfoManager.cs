namespace InputSupport
{
	public abstract class InputInfoManager<T> : IChangeInputInfo<T>, IGetInputInfo<T> {
		#region IGetInputInfo implementation
		public abstract System.Collections.Generic.List<T> CurrentInfo { get; }
		public abstract int InfoCount { get; }
		#endregion

		#region IChangeInputInfo implementation
		public abstract void Add (T info);
		public abstract void Remove (T info);
		public abstract void Update (T info);
		public abstract void Clear ();

		public void AddAll (System.Collections.Generic.IEnumerable<T> info)
		{
			foreach (var i in info) {
				Add (i);
			}
		}

		public void RemoveAt (int index)
		{
			if (CurrentInfo.Count < index) return;
			Remove (CurrentInfo [index]);
		}

		public void RemoveAll (System.Collections.Generic.IEnumerable<T> info)
		{
			foreach (var i in info) {
				Remove (i);
			}
		}

		public void UpdateAll (System.Collections.Generic.IEnumerable<T> info)
		{
			foreach(var i in info) {
				Update(i);
			}
		}
		#endregion
	}
}
