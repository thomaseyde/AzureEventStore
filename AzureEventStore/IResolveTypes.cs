using System;

namespace AzureEventStore
{
    public interface IResolveTypes
    {
        string GetTypename(object e);
        Type GetType(string name, Type owner);
    }
}