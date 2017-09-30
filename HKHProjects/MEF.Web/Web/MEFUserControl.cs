using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEF.Web;

namespace System.Web.UI
{
	public class MEFUserControl : UserControl, IDependencyObject
	{
		public MEFUserControl()
		{
			if (WebMEF.Container != null)
				WebMEF.Container.ComposeParts(this);
		}
	}
}
