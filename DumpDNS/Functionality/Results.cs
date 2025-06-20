using DnsClient;

namespace DumpDNS.Functionality
{
    public class Results
    {
        public static bool Start(IDnsQueryResponse dump, (int, int) dimensions)
        {
            Program.CanSearch = true;
            Results results = new Results(dump, dimensions);
            Program.ActiveInstructions = Program.BottomInstructions.OptionsWithBackWithExit;
            Program.Render += results.Render;
            Program.Render(null, dimensions);
            
            return results.StartCycle();
        }

        public enum Modes
        {
            Select,
            View
        };

        public Modes Mode = Modes.Select;

        IDnsQueryResponse Dump;
        (int, int) Dimensions;

        public Results(IDnsQueryResponse dump, (int, int) dimensions)
        {
            Dump = dump;
            Dimensions = dimensions;
        }

        /// <summary>
        /// Waits for input before handling it
        /// </summary>
        public bool StartCycle()
        {
            while (true)
            {
                switch (Mode)
                {
                    case Modes.Select:
                        if (!SwitchHandle())
                        {
                            Program.Render -= Render;
                            return true;
                        }
                        break;
                    case Modes.View:
                        if(!ViewHandle())
                        {
                            Mode = Modes.Select;
                            Program.Render(this, Dimensions);
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(10);
            }
        }

        Grid SwitchGrid = new Grid(false);
        Grid ViewGrid = new Grid(true);
        Types.DnsRecordType SelectedType = Types.DnsRecordType.A;

        /// <summary>
        /// Handles the user input for the DNS record type switching
        /// </summary>
        public bool SwitchHandle()
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                int totalRows = Dimensions.Item2 - 3 - 1;
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (SwitchGrid.SelectedIndex == 0) return true;
                    SwitchGrid.SelectedIndex--;
                    // If the selected index is near the top, go up (if it can)
                    if (SwitchGrid.SelectedIndex - SwitchGrid.Position <= SwitchGrid.Position &&
                        SwitchGrid.Position != 0)
                        SwitchGrid.Position--;
                } else if(key.Key == ConsoleKey.DownArrow)
                {
                    if (SwitchGrid.SelectedIndex == SwitchGrid.Rows.Count - 1) return true;
                    SwitchGrid.SelectedIndex++;
                    // If the selected index is near the bottom, go down (if it can)
                    if (SwitchGrid.SelectedIndex >= totalRows + SwitchGrid.Position &&
                        SwitchGrid.Position != SwitchGrid.Rows.Count)
                        SwitchGrid.Position++;
                } else if(key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.LeftArrow)
                {
                    return false;
                } else if(key.Key == ConsoleKey.Enter)
                {
                    Mode = Modes.View;
                    Console.Clear();
                    SelectedType = (Types.DnsRecordType)SwitchGrid.SelectedIndex;
                    Program.Render(this, Dimensions);
                }
                Console.Clear();
                Program.Render(this, Dimensions);
            }

            return true;
        }

        public bool ViewHandle()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                int totalRows = Dimensions.Item2 - 3 - 1;
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (ViewGrid.SelectedIndex == 0) return true;
                    ViewGrid.SelectedIndex--;
                    // If the selected index is near the top, go up (if it can)
                    if (ViewGrid.SelectedIndex - ViewGrid.Position <= ViewGrid.Position &&
                        ViewGrid.Position != 0)
                        ViewGrid.Position--;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (ViewGrid.SelectedIndex == ViewGrid.Rows.Count - 1) return true;
                    ViewGrid.SelectedIndex++;
                    // If the selected index is near the bottom, go down (if it can)
                    if (ViewGrid.SelectedIndex >= totalRows + ViewGrid.Position &&
                        ViewGrid.Position != ViewGrid.Rows.Count)
                        ViewGrid.Position++;
                }
                else if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.LeftArrow)
                {
                    return false;
                }
                Console.Clear();
                Program.Render(this, Dimensions);
            }

            return true;
        }

        /// <summary>
        /// Render it
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="dimensions">Dimensions of the buffer</param>
        public void Render(object? sender, (int, int) dimensions)
        {
            switch (Mode)
            {
                case Modes.Select:
                    RenderSwitch(sender, dimensions);
                    break;
                case Modes.View:
                    RenderView(sender, dimensions);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Render the selection menu that allows the user to select which record type to show
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="dimensions">Dimensions of the buffer</param>
        public void RenderSwitch(object? sender, (int, int) dimensions)
        {
            // Assume that there are more record types than what can fit on the screen
            if(SwitchGrid.Title.Length == 0)
            {
                SwitchGrid.Title = "Which DNS record type would you like to view?";
                SwitchGrid.Headers = new List<string> { "Record Type" };

                for (int i = 0; i < Types.RecordTypes.Length; i++)
                {
                    SwitchGrid.Rows.Add(new List<string> { Types.RecordTypes[i].ToString() });
                }
            }

            SwitchGrid.Render(dimensions);
        }

        public void RenderView(object? sender, (int, int) dimensions)
        {
            ViewGrid.Title = $"Viewing all {Types.IRecords[SelectedType].Rows.Count} {Types.DNSRecordTypeDictionary[SelectedType]} records";
            ViewGrid.Headers = Types.IRecords[SelectedType].Headers;
            ViewGrid.Rows = Types.IRecords[SelectedType].Rows;
            ViewGrid.Render(dimensions);
        }

        public class Scrollbar
        {
            /// <summary>
            /// Initializes the ScrollBar class
            /// </summary>
            /// <param name="total">Total lines</param>
            /// <param name="current">Current lines</param>
            public Scrollbar(int total, int current)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Renders a scrollbar along the right hand side of the screen
            /// </summary>
            /// /// <param name="dimensions">Dimensions of the buffer</param>
            public void Render((int, int) dimensions)
            {
                // Calculate the scrollbar thumb size
                throw new NotImplementedException();
            }
        }

        public class Grid
        {
            public string Title;
            public List<string> Headers;
            public List<int> ColumnWidths;
            public List<List<string>> Rows;

            internal (int, int) LastDimensions;

            public bool HasColumnHeaders = true;
            // Based on HasColumnHeaders
            // 2 = Below title
            // 3 = Below column headers
            internal int StartX = 3;

            internal int Position = 0;
            internal int SelectedIndex = 0;

            public Grid(bool HasColumnHeaders = true)
            {
                Title = "";
                Headers = new List<string>();
                ColumnWidths = new List<int>();
                Rows = new List<List<string>>();

                this.HasColumnHeaders = HasColumnHeaders;
                if (!HasColumnHeaders) StartX = 2;
            }

            /// <summary>
            /// Render it
            /// </summary>
            /// <param name="dimensions">Dimensions of the buffer</param>
            public void Render((int, int) dimensions)
            {
                CalculateWidths(dimensions); // Make sure everything is correct

                Console.ResetColor();
                Console.CursorTop = 1;
                Console.CursorLeft = 0;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"{Title}{new string(' ', dimensions.Item1 - Title.Length)}");
                Console.ResetColor();

                if(HasColumnHeaders)
                {
                    Console.CursorTop = StartX - 1;

                    for (int i = 0; i < Headers.Count; i++)
                    {
                        if (Console.CursorLeft + Headers[i].Length > dimensions.Item1 + 1) break; // Don't continue if there isn't enough space
                        Console.Write($"{Headers[i]}{new string(' ', ColumnWidths[i] - Headers[i].Length)}"); // Show the column header (with padding on the left, and automatic padding on the right)
                    }
                }

                Console.CursorTop = StartX;
                Console.CursorLeft = 0;
                int totalRows = dimensions.Item2 - 3 - 1; // Total rows that can be shown
                if (HasColumnHeaders) totalRows--;

                for (int i = Position; i < Rows.Count; i++)
                {
                    if (i - Position > totalRows) continue;

                    if (SelectedIndex == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    } else
                    {
                        if (i % 2 == 0) Console.BackgroundColor = ConsoleColor.DarkGray;
                        else Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    for (int j = 0; j < Rows[i].Count; j++)
                    {
                        Console.Write($"{Rows[i][j]}{new string(' ', ColumnWidths[j] - Rows[i][j].Length)}");
                    }
                    Console.WriteLine(new string(' ', dimensions.Item1 - Console.CursorLeft - 1));
                }
                Console.ResetColor();
            }

            /// <summary>
            /// Recalculates the width of the columns if needed
            /// (If the dimensions change from the last time they were recorded)
            /// </summary>
            /// <param name="dimensions">Dimensions of the buffer</param>
            internal void CalculateWidths((int, int) dimensions, bool force = false)
            {
                if (dimensions == LastDimensions && !force) return; // No change detected

                // Start with column header widths first, then move down and add more space if needed
                // Make sure to account for 2 padding (1 on each side) - don't add 1 to the left, as that is already added
                // Add a little extra padding just to be sure though
                if (Headers.Count == 1)
                {
                    // If there is only 1 header, assume that it takes the entire column up
                    ColumnWidths = new List<int> { dimensions.Item1 };
                } else
                {
                    int[] widths = new int[Headers.Count];
                    for (int i = 0; i < Headers.Count; i++)
                    {
                        widths[i] = Headers[i].Length + 1 + 2;

                        for (int j = 0; j < Rows.Count; j++)
                        {
                            if (widths[i] - 1 < Rows[j][i].Length) widths[i] = Rows[j][i].Length + 1 + 2;
                        }
                    }

                    ColumnWidths = widths.ToList();
                }
            }
        }
    }
}