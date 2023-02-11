using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace HKH.Mef2.Integration
{
    public class DefaultResolver : ResolverBase
    {
        #region Fields

        private readonly IServiceProvider _container;

        #endregion

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DefaultResolver" /> class.
        /// </summary>
        /// <param name = "container"><see cref = "IServiceProvider" /> instance.</param>
        public DefaultResolver(IServiceProvider container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }


        #endregion

        #region Methods

        public override object Resolve(Type type, string name = "")
        {
            return _container.GetService(type);
        }
        #endregion
    }
}
