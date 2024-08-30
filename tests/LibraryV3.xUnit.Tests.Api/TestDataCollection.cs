using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryV3.xUnit.Tests.Api;

[CollectionDefinition("LibraryV3 Collection")]
public class TestDataCollection : ICollection<WebApplicationFactory<IApiMarker>>
{
    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(WebApplicationFactory<IApiMarker> item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(WebApplicationFactory<IApiMarker> item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(WebApplicationFactory<IApiMarker>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<WebApplicationFactory<IApiMarker>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool Remove(WebApplicationFactory<IApiMarker> item)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
