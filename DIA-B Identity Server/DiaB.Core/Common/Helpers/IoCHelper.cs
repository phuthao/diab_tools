using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DiaB.Core.Common.Helpers
{
    public class IoCHelper
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetInstance<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public static IEnumerable<T> GetInstances<T>() where T : class
        {
            return _serviceProvider.GetServices<T>();
        }
    }
}
