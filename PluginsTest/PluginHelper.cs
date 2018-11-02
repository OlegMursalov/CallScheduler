using Microsoft.Xrm.Sdk;
using System;

namespace PluginsTest
{
    public static class PluginHelper
    {
        public static IPluginExecutionContext GetContext(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
            if (context == null)
                throw new InvalidPluginExecutionException("IPluginExecutionContext is null");
            return context;
        }

        public static Entity GetTarget(IPluginExecutionContext context)
        {
            return context.InputParameters != null && context.InputParameters.Contains("Target") ? context.InputParameters["Target"] as Entity : null;
        }

        public static T GetTarget<T>(IPluginExecutionContext context) where T: Entity
        {
            if (context.InputParameters == null || !context.InputParameters.Contains("Target"))
                throw new InvalidPluginExecutionException("Target not contained in the dictionary");
            var target = context.InputParameters["Target"] as Entity;
            if (target == null)
                throw new InvalidPluginExecutionException("Target is not entity");
            return target.ToEntity<T>();
        }

        public static EntityReference GetTargetReference(IPluginExecutionContext context)
        {
            return context.InputParameters != null && context.InputParameters.Contains("Target") ? context.InputParameters["Target"] as EntityReference : null;
        }
    }
}