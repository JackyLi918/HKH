using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEF.Web;

namespace System.Web.UI
{
	public class MEFPage : Page, IDependencyObject
	{
		public MEFPage()
		{
			if (WebMEF .Container != null)
				WebMEF.Container.ComposeParts(this);
		}
	}
}
