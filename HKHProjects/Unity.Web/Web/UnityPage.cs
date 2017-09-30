/*******************************************************
 * Filename: UnityPage.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	6/27/2013 5:44:02 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Unity.Web;

namespace System.Web.UI
{
   public  class UnityPage : Page, IDependencyObject
    {
       public UnityPage()
        {
            if(WebUnity.Container!=null)
                WebUnity.Container.BuildUp(this.GetType().BaseType, this);
        }
    }
}
