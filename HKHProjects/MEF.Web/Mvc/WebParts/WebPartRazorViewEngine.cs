/*******************************************************
 * Filename: WebPartRazorViewEngine.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/15/2016 3:28:50 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Collections.Generic;
using System.Web.Mvc;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// WebPartRazorViewEngine
    /// </summary>
    public class WebPartRazorViewEngine : RazorViewEngine
    {
        #region Variables

        private List<IWebPart> webParts = null;

        #endregion

        #region Properties

        #endregion

        public WebPartRazorViewEngine(IList<IWebPart> webParts)
        {
            this.webParts = webParts as List<IWebPart>;

            MasterLocationFormats = GetMasterLocations();
            ViewLocationFormats = GetViewLocations();
            PartialViewLocationFormats = GetViewLocations();

            AreaMasterLocationFormats = GetAreaMasterLocations();
            AreaViewLocationFormats = GetAreaViewLocations();
            AreaPartialViewLocationFormats = GetAreaViewLocations();

            FileExtensions = new[] { "cshtml" };
        }

        #region Methods

        #endregion

        #region Helper

        private string[] GetMasterLocations()
        {
            var masterPages = new List<string>();

            masterPages.Add("~/Views/Shared/{0}.cshtml");

            webParts.ForEach(part =>
                masterPages.Add("~/" + part.ViewLocation + "/Views/Shared/{0}.cshtml")
            );

            return masterPages.ToArray();
        }

        private string[] GetViewLocations()
        {
            var views = new List<string>();
            views.Add("~/Views/{1}/{0}.cshtml");

            webParts.ForEach(part =>
                views.Add("~/" + part.ViewLocation + "/Views/{1}/{0}.cshtml")
            );
            return views.ToArray();
        }

        private string[] GetAreaMasterLocations()
        {
            var masterPages = new List<string>();

            masterPages.Add("~/Areas/Views/Shared/{0}.cshtml");

            webParts.ForEach(part =>
                masterPages.Add("~/" + part.ViewLocation + "/Areas/Views/Shared/{0}.cshtml")
            );

            return masterPages.ToArray();
        }

        private string[] GetAreaViewLocations()
        {
            var views = new List<string>();
            views.Add("~/Areas/Views/{1}/{0}.cshtml");

            webParts.ForEach(part =>
                views.Add("~/" + part.ViewLocation + "/Areas/Views/{1}/{0}.cshtml")
            );
            return views.ToArray();
        }

        #endregion
    }
}