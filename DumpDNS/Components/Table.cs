using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DumpDNS.Components;

public class Table : IDisposable
{
    internal ObservableCollection<string> _headers { get; set; } = [];
    internal ObservableCollection<string[]> _rows { get; set; } = [];

    public string? Title { get; set; }
    public string[] Headers { get { return [.. _headers]; } set { _headers = new(value); } }
    public string[][] Rows { get { return [.. _rows]; } set { _rows = new(value); } }
    internal List<List<List<string>>> ActualRows = [];

    internal Types.SizeAndPos lastDimensions = new((0, 0));

    /// <summary>
    /// The currently selected OBSERVABLE row, this is NOT
    /// the same as SelectedIndex
    /// </summary>
    internal int position = 0;

    /// <summary>
    /// The ACTUAL selected row, may not be observable
    /// </summary>
    internal int selectedIndex = 0;

    internal int oStart = 0;
    internal int oEnd = 0;

    public Table()
    {
        _headers.CollectionChanged += Changed;
        _rows.CollectionChanged += Changed;
    }

    public Table(string[] Headers, string[,]? Rows)
    {
        _headers = new(Headers);
        if (Rows != null) _rows = new(Internal.Utils.Arr2DTo2Arr(Rows));

        _headers.CollectionChanged += Changed;
        _rows.CollectionChanged += Changed;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        int cx = Console.CursorLeft, cy = Console.CursorTop;
        ConsoleColor cbg = Console.BackgroundColor;
        Console.BackgroundColor = ConsoleColor.Black;

        Console.SetCursorPosition(lastDimensions.X, lastDimensions.Y);
        for (int y = 0; y < lastDimensions.Height; y++)
        {
            Console.Write(new string(' ', lastDimensions.Width));
            Console.SetCursorPosition(lastDimensions.X, lastDimensions.Y + y);
        }

        Console.BackgroundColor = cbg;
        Console.SetCursorPosition(cx, cy);
        GC.ReRegisterForFinalize(this);
    }

    internal void Changed(object? sender, NotifyCollectionChangedEventArgs e)
    {

    }

    internal void CalculateSize(Types.SizeAndPos dimensions, bool force = false)
    {
        if (dimensions == lastDimensions && !force) return;

        /*
            When calculating
        */
    }

    internal void CalculateWidths(Types.SizeAndPos dimensions, bool force = false)
    {
        if (dimensions == lastDimensions && !force) return;

        /*
            Start of by calculating the widths of the column header first, columns
            can expand past the old limit of its even chunk of the full width available.
            
            We add 1 extra padding on the end of each column, except the very last column.
            This was also a limitation of the previous method, where it would add padding
            no matter what, so there would always be a 2 character gap, between them,
            which is just wasting space.

            Before we do anything, we need to check if there are any headers, and if so,
            we check if there is only 1, in that case, we let it take the full width. If
            there are no headers, we need to instead just calculate it based on the even
            split of the columns, work out the empty space left over, and then split it
            between any columns that require expanding.

            We then start by calculating the width by the headers, and then work out how
            much empty space there is left. There will almost always be a column that
            wants to take up more space (like TXT record value columsn for example), so
            we need to account for them.
        */

        if (Headers.Count == 0)
        {

        }
    }

    /// <summary>
    /// Adds empty missing headers when there are extra columsn in a row
    /// </summary>
    internal void FixHeaders()
    {

    }

    internal void GenerateActualRows()
    {

    }
}
