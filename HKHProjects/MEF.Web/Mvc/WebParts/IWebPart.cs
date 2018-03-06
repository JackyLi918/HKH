/*******************************************************
 * Filename: IWebPart.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/15/2016 2:47:08 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System.Web.Optimization;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// IWebPart
    /// </summary>
    public interface IWebPart
    {
        #region Variables

        #endregion

        #region Properties

        //IList<Route> Routes { get; }
        string ViewLocation { get; }
        //IList<WebPartResource> Css { get; }
        //IList<WebPartResource> Scripts { get; }
        //IList<WebPartResource> HeaderScripts { get; }

        bool Enabled { get; set; }

        #endregion

        #region Methods

        string MapPath(string virtualPath);
        void RegisterBundles(BundleCollection bundles);

        #endregion

        #region Helper

        #endregion
    }

    public class WebPartBase : IWebPart
    {
        #region Variables

        protected string virtualViewLocation = string.Empty;

        #endregion

        public WebPartBase()
        {
            ViewLocation = CalcViewLocation();
            virtualViewLocation = "~/" + ViewLocation;

            Enabled = true;
        }

        #region Properties

        public string ViewLocation { get; private set; }

        public bool Enabled { get; set; }

        #endregion

        #region Methods

        public virtual void RegisterBundles(BundleCollection bundles)
        {
        }

        public virtual string MapPath(string virtualPath)
        {
            if (virtualPath.StartsWith(virtualViewLocation))
                return virtualPath;

            return virtualPath.Replace("~", virtualViewLocation);
        }
        #endregion

        #region Helper

        private string CalcViewLocation()
        {
            string assemblyName = this.GetType().Assembly.FullName;
            int i = assemblyName.IndexOf(",");
            if (i > 0)
                assemblyName = assemblyName.Substring(0, i);
            return WebPartsManager.WebPartFolderName + "/" + assemblyName;
        }

        #endregion
    }
}