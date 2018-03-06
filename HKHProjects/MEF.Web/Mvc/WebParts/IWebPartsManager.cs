/*******************************************************
 * Filename: IWebPartManager.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/15/2016 7:42:04 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// IWebPartManager
    /// </summary>
    public interface IWebPartsManager
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Methods

        IEnumerable<Lazy<IWebPart, IWebPartMetadata>> WebParts { get; set; }

        IWebPart GetWebPart(string id);

        #endregion

        #region Helper

        #endregion
    }

    public class WebPartsManager : IWebPartsManager
    {
        public static readonly string WebPartFolderName = "WebParts";

        [ImportMany]
        public IEnumerable<Lazy<IWebPart, IWebPartMetadata>> WebParts { get; set; }

        public IWebPart GetWebPart(string id)
        {
            foreach (Lazy<IWebPart, IWebPartMetadata> webPart in WebParts)
            {
                if (webPart.Metadata.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return webPart.Value;
            }
            return null;
        }

        public static void RegisterWebParts(BundleCollection bundles)
        {
            foreach (IWebPart webPart in DependencyResolver.Current.GetServices<IWebPart>())
            {
                webPart.RegisterBundles(bundles);
            }
        }
    }
}