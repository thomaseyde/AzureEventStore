using System;
using System.Linq;
using AzureEventStore;

namespace ExampleDomain.Persistence
{
    public class TypeResolver : IResolveTypes
    {
        public string GetTypename(object e)
        {
            return e.GetType().Name;
        }

        public Type GetType(string name, Type owner)
        {
	        try
	        {
		        var types = owner.Assembly.GetTypes();
		        var relevantTypes = types.Where(info => info.Namespace != null && info.Namespace.StartsWith(owner.Namespace));

		        return relevantTypes.Single(info => info.Name == name);
	        }
	        catch (InvalidOperationException)
	        {
		        throw new ArgumentException($"Ambiguous type name '{name}'");
	        }
        }
    }
}