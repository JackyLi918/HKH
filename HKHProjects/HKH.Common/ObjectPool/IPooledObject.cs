/*******************************************************
 * Filename: IPooledObject.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/29/2013 4:06:13 PM
 * Author:	JackyLi
 * 
*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HKH.ObjectPool
{
	public interface IPooledObject : IDisposable
	{
		bool IsBusy { get; set; }
	}

	class WeakReferenceList : List<WeakReference>
	{
	}

	class WeakReferenceDictionary : Dictionary<Type, WeakReferenceList>
	{
	}
}
