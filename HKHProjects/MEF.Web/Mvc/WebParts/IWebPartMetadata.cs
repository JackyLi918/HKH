/*******************************************************
 * Filename: IWebPartMetadata.cs
 * File description：
 * 
 * Version:	1.0
 * Created:	7/15/2016 2:59:29 PM
 * Author:	JackyLi
 * 
*****************************************************/

using System;
using System.ComponentModel.Composition;

namespace MEF.Mvc.WebParts
{
    /// <summary>
    /// IWebPartMetadata
    /// </summary>
    public interface IWebPartMetadata
    {
        #region Variables

        #endregion

        #region Properties

        string Id { get; }
        string Name { get; }
        string Description { get; }

        #endregion

        #region Methods

        #endregion

        #region Helper

        #endregion
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WebPartMetadataAttribute : ExportAttribute
    {
        public WebPartMetadataAttribute(string id, string name, string description)
            : base(typeof(IWebPartMetadata))
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}