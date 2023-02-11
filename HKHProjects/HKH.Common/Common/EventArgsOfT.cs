namespace System//HKH.Common
{
	public class EventArgs<T>
	{
		private T data = default(T);
		public EventArgs(T eventData)
		{
			this.data = eventData;
		}

		public T Data
		{
			get { return data; }
		}
	}
}
