/*******************************************************
 * Filename: PooledObjectLocator.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	3/29/2013 4:06:13 PM
 * Author:	JackyLi
 * 
*****************************************************/
using System;
using System.Timers;

namespace HKH.ObjectPool
{
	public static class PooledObjectLocator
	{
		static Timer destroyTimer;
		static WeakReferenceDictionary ObjectPool { get; set; }

		static PooledObjectLocator()
		{
			ObjectPool = new WeakReferenceDictionary();
			destroyTimer = new Timer();
			destroyTimer.Interval = 60000;
			destroyTimer.Elapsed += new ElapsedEventHandler(Destroy);
			destroyTimer.Start();
		}

		/// <summary>
		/// gets an instance from Pool
		/// </summary>
		/// <param name="objType"></param>
		/// <returns></returns>
		public static IPooledObject GetInstance(Type objType)
		{
			if (!typeof(IPooledObject).IsAssignableFrom(objType))
			{
				throw new InvalidCastException("InstanceType must be HKH.ObjectPool.IPooledObject");
			}

			if (!ObjectPool.ContainsKey(objType))
			{
				ObjectPool[objType] = new WeakReferenceList();
			}

			WeakReferenceList instList = ObjectPool[objType];

			lock (objType)
			{
				IPooledObject objInst = null;

				foreach (WeakReference weakReference in instList)
				{
					objInst = weakReference.Target as IPooledObject;

					if (objInst != null && !objInst.IsBusy)
					{
						objInst.IsBusy = true;
						return objInst;
					}
				}

				objInst = Activator.CreateInstance(objType) as IPooledObject;
				objInst.IsBusy = true;
				instList.Add(new WeakReference(objInst));

				return objInst;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetInstance<T>() where T : IPooledObject
		{
			return (T)GetInstance(typeof(T));
		}

		/// <summary>
		/// return an instance to Pool
		/// </summary>
		/// <param name="objInst"></param>
		public static void ReleaseInstance(IPooledObject objInst)
		{
			objInst.IsBusy = false;
		}

		static void Destroy(object sender, ElapsedEventArgs args)
		{
			foreach (Type instType in ObjectPool.Keys)
			{
				lock (instType)
				{
					WeakReferenceList instList = ObjectPool[instType];

					for (int i = instList.Count - 1; i > -1; i--)
					{
						if (instList[i].Target == null)
						{
							instList.RemoveAt(i);
						}
					}
				}
			}
		}
	}
}
