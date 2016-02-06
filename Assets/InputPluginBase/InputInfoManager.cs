using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InputSupport
{
	public abstract class InputInfoManager<T> {
		public abstract List<T> CurrentInfo { get; }
		public abstract int InfoCount { get; }

		public abstract void Add (T info);
		public abstract void Remove (T info);
		public abstract void Clear ();
		public abstract void Update (T info);
	}
}
