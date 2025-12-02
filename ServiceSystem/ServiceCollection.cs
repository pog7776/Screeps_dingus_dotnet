using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace dingus_net.ServiceSystem
{
public class ServiceCollection : ICollection<Service>, IServiceRunner
{
    protected readonly HashSet<Service> Services = [];

    /// <inheritdoc />
    public IEnumerator<Service> GetEnumerator() => Services.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Add(Service item) => Services.Add(item);

    /// <inheritdoc />
    public void Clear() => Services.Clear();

    /// <inheritdoc />
    public bool Contains(Service item) => Services.Contains(item);

    /// <inheritdoc />
    public void CopyTo(Service[] array, int arrayIndex) => Services.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(Service item) => Services.Remove(item);

    /// <inheritdoc />
    public int Count => Services.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void OnTick()
    {
        foreach(Service service in Services) { service.OnTick(); }
    }

    public Service this[string name] => Services.First(service => service.Name == name);
    public Service this[Type type] => Services.First(service => service.GetType() == type);

    public T? GetService<T>() where T : Service => TryGetService(out T? service) ? service : null;

    public bool TryGetService<T>([MaybeNullWhen(false)] out T service) where T : Service?
        => (service = Services.First(static service => service.GetType() == typeof(T)) as T) != null;

    public IEnumerable<Service> GetServices<T>() where T : Service
        => Services.Where(static service => service.GetType() == typeof(T));
}
}
