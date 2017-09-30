/*******************************************************
 * Filename: ParallelTaskBase.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	5/30/2013 10:05:05 AM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKH.Tasks
{
	/// <summary>
	/// ParallelTaskBase
	/// </summary>
	public abstract class ParallelTaskBase : IParallelTask
	{
		#region Variables

		protected ParallelTaskState _state = ParallelTaskState.Queued;

		#endregion

		#region Properties

		public string Id { get; set; }

		public ParallelTaskState State
		{
			get { return _state; }
			protected set { _state = value; }
		}

		#endregion

		#region Methods

		public ParallelTaskResult Execute()
		{
			ParallelTaskResult result = new ParallelTaskResult();

			OnExecuting(result);
			if (result.Status == ExecutionStatus.OK)
				ExecuteCore(result);
			OnExecuted(result);

			return result;
		}

		public virtual void OnException(ParallelTaskResult result, AggregateException ex)
		{
			result.Status = ExecutionStatus.Error;
			result.Exception = ex;
		}

		protected virtual void OnExecuting(ParallelTaskResult result)
		{
		}

		protected abstract void ExecuteCore(ParallelTaskResult result);

		protected virtual void OnExecuted(ParallelTaskResult result)
		{
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (obj.GetType() != this.GetType())
				return false;

			return this.Id.Equals((obj as IParallelTask).Id);
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		#endregion

		#region Helper

		#endregion
	}
}