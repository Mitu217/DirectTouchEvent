using UnityEngine;
using System.Collections.Generic;

namespace InputSupport
{
	public interface IGetInputInfo<T> {
		List<T> CurrentInfo { get; }
		int InfoCount { get; }
	}
}
