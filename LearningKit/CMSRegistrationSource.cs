using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Core;
using Autofac.Builder;

namespace LearningKit
{
    /// <summary>
    ///  Allows registrations to be made on-the-fly when unregistered services are requested.
    /// </summary>
    public class CMSRegistrationSource : IRegistrationSource
    {
        /// <summary>
        /// Gets whether the registrations provided by this source are 1:1 adapters on top of other components (I.e. like Meta, Func or Owned.)
        /// </summary>
        public bool IsAdapterForIndividualComponents => false;


        /// <summary>
        /// Retrieve registrations for an unregistered service, to be used by the container.
        /// </summary>
        /// <param name="service">The service that was requested.</param>
        /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            // There are other registration exists in the container
            if (registrationAccessor(service).Any())
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            var swt = service as IServiceWithType;
            if (swt == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            object instance = CMS.Core.Service.ResolveOptional(swt.ServiceType);

            if (instance == null)
            {
                return Enumerable.Empty<IComponentRegistration>();
            }

            // Register the instance in the container
            return new[] { RegistrationBuilder.ForDelegate(swt.ServiceType, (c, p) => instance).CreateRegistration() };
        }
    }
}