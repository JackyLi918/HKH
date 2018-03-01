/*******************************************************
 * Filename: UnityPage.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	6/27/2013 5:44:02 PM
 * Author:	JackyLi
 * 
*****************************************************/

using Unity.Web;

namespace System.Web.UI
{
    public class UnityPage : Page, IDependencyObject
    {
        public UnityPage()
        {
            if (WebUnity.Container != null)
                WebUnity.Container.BuildUp(this.GetType().BaseType, this, "UnityPage");
        }
    }
}
