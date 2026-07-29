using System;

namespace DumpDNS.Internal;

public class Query
{
    internal List<Item> _items = [];
    internal string _query;

    public Item[] Items { get { return [.. _items]; } }

    public Query()
    {
        _query = "*";
        BuildQuery();
    }

    public Query(string query)
    {
        _query = query;
        BuildQuery();
    }

    public enum ItemType
    {
        All // *
    }

    public class Item
    {
        public ItemType Type;
    }

    internal void BuildQuery()
    {
        if (_query == "*")
        {
            _items.Add(new Item { Type = ItemType.All });
        }
    }
}
