using System;
using Autofac;

namespace HKH.Mef2.Integration.Autofac
{
    /// <summary>
    ///   Represents an adapter for the container.
    /// </summary>
    public class AutofacResolver : ResolverBase
    {
        #region Fields

        private readonly ILifetimeScope _container;

        #endregion

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "AutofacResolver" /> class.
        /// </summary>
        /// <param name = "container"><see cref = "ILifetimeScope" /> instance.</param>
        public AutofacResolver(ILifetimeScope container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        #endregion

        #region Methods
 
        public override object Resolve(Type type, string name = "")
        {
            return string.IsNullOrEmpty(name) ? _container.Resolve(type) : _container.ResolveNamed(name, type);
        }

        #endregion
    }
}
