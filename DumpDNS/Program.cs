using DnsClient;
using System.Text;

namespace DumpDNS
{
    internal class Program
    {
        static int LastW = 0;
        static int LastH = 0;
        static Task? ResizeTask;
        static void Main(string[] args)
        {
            Console.Title = "DumpDNS";
            Console.Clear();
            Console.WriteLine("Working...");
            SizeChanged += (object? sender, (int, int) dimensions) =>
            {
                Console.ResetColor();
                Console.Clear(); // Wipe it
                LastW = dimensions.Item1;
                LastH = dimensions.Item2;
                if (Render != null) Render.Invoke(sender, dimensions); // Call render if it is defined
            };

            ResizeTask = Task.Factory.StartNew(() =>
            {
                while(true)
                {
                    if (Console.BufferWidth != LastW || Console.BufferHeight != LastH)
                    {
                        SizeChanged(null, (Console.BufferWidth, Console.BufferHeight));
                    }
                    Thread.Sleep(100); // Wait, so that it doesn't freeze
                }
            });

            Render += (object? sender, (int, int) dimensions) =>
            {
                RenderTop();
                RenderBottom(ActiveInstructions);
            };

            UpdateBottom += (object? sender, EventArgs e) =>
            {
                Render(sender, (Console.BufferWidth, Console.BufferHeight));
            };

            SizeChanged(null, (Console.BufferWidth, Console.BufferHeight));

            Functionality.Version.StartCheck();

            while (true)
            {
                For = "";
                CanDump = false;
                Domain = null;
                Console.Clear();

                // First stage, select a domain
                Functionality.DomainSelection DomainSelection = Functionality.DomainSelection.Start((LastW, LastH));
                if (DomainSelection.Success == false) return;
                For = DomainSelection.Domain.ToString();
                Domain = DomainSelection.Domain.ToString();

                Console.Clear();

                // Second stage, dump DNS records
                IDnsQueryResponse dump = Functionality.Dump.Start(DomainSelection.Domain.ToString(), (LastW, LastH));

                Console.Clear();

                // Last stage, show the results
                CanDump = true;
                bool exit = !Functionality.Results.Start(dump, (LastW, LastH));

                if (exit) return;
            }
        }

        /// <summary>
        /// You MUST deregister the event after the section has finished (So it no longer needs to be rendered)
        /// It WILL render over everything else and mess things up
        /// </summary>
        public static EventHandler<(int, int)>? Render; // Render whatever is being displayed, needs to be changed
        public static EventHandler<(int, int)>? SizeChanged;
        public static EventHandler? UpdateBottom; // Updates specifically the bottom bar

        public static string? Domain;

        public static string For;
        public static BottomInstructions ActiveInstructions;

        public enum BottomInstructions
        {
            Text,                                               // "Enter: Finish | Ctrl+R: Clear"
            Options,                                            // "Up/Down/Enter: Select"
            OptionsWithBack,                                    // "Up/Down/Enter: Select | Escape/Left: Back"
            OptionsWithBackSecondary,                           // "Up/Down/Enter: Select | Left: Back"
            Processing,                                         // "Escape: Cancel"
            ProcessingNoCancel,                                 // "No options available"

            // With exit
            TextWithExit,                                       // "Return: Finish | Shift+R: Clear | Ctrl+C: Exit"
            OptionsWithExit,                                    // "Up/Down/Enter: Select | Ctrl+C: Exit"
            OptionsWithBackWithExit,                            // "Up/Down/Enter: Select | Escape/Left: Back | Ctrl+C: Exit"
            OptionsWithBackSecondaryWithExit,                   // "Up/Down/Enter: Select | Left: Back | Ctrl+C: Exit"
            ProcessingWithExit,                                 // "Escape: Cancel | Ctrl+C: Exit"
            ProcessingNoCancelWithExit,                         // "Ctrl+C: Exit"

            // Search specific
            Search                                              // "Escape: Back"
        }

        public static Dictionary<BottomInstructions, string> BottomInstructionsDictionary =
        new()
        {
            [BottomInstructions.Text]                               = "Enter: Finish | Ctrl+R: Clear",
            [BottomInstructions.Options]                            = "Up/Down/Enter: Select",
            [BottomInstructions.OptionsWithBack]                    = "Up/Down/Enter: Select | Escape/Left: Back",
            [BottomInstructions.OptionsWithBackSecondary]           = "Up/Down/Enter: Select | Left: Back",
            [BottomInstructions.Processing]                         = "Escape: Cancel",
            [BottomInstructions.ProcessingNoCancel]                 = "No options available",

            // With exit
            [BottomInstructions.TextWithExit]                       = "Enter: Finish | Ctrl+R: Clear | Ctrl+C: Exit",
            [BottomInstructions.OptionsWithExit]                    = "Up/Down/Enter: Select | Ctrl+C: Exit",
            [BottomInstructions.OptionsWithBackWithExit]            = "Up/Down/Enter: Select | Escape/Left: Back | Ctrl+C Exit",
            [BottomInstructions.OptionsWithBackSecondaryWithExit]   = "Up/Down/Enter: Select | Left: Back | Ctrl+C: Exit",
            [BottomInstructions.ProcessingWithExit]                 = "Escape: Cancel | Ctrl+C: Exit",
            [BottomInstructions.ProcessingNoCancelWithExit]         = "Ctrl+C: Exit",

            // Search specific
            [BottomInstructions.Search]                             = "Escape: Back"
        };

        public class SearchBar
        {
            public static char[] AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.=_:; ".ToCharArray();

            public StringBuilder Term;
            public int Cursor;

            public Functionality.Search Search;
            public Functionality.SearchResults Results;

            public SearchBar()
            {
                Term = new StringBuilder();
                Cursor = 0;

                Search = new Functionality.Search();
                Results = new Functionality.SearchResults();
            }

            public bool StartCycle()
            {
                Program.Render += Render;
                while(true)
                {
                    Thread.Sleep(10);
                    return false; // Temporary
                }
            }

            public void Render(object? sender, (int, int) dimensions)
            {

            }
        }

        public static SearchBar searchBar;
        public static bool IsSearchBar = false; // Whether or not to display the search bar
        public static bool CanSearch = false;
        public static bool CanDump = false;

        public static bool EnableSearchBar()
        {
            searchBar = new SearchBar();
            IsSearchBar = true;

            return searchBar.StartCycle();
        }

        /// <summary>
        /// Starts the dump file process
        /// </summary>
        public static bool StartDump()
        {
            return new Functionality.DumpFile(Domain ?? "NULL", (Console.BufferWidth, Console.BufferHeight)).StartCycle();
        }

        /// <summary>
        /// Render the top bar
        /// </summary>
        static void RenderTop()
        {
            if (IsSearchBar && searchBar != null) return; // Handled elsewhere

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            string text = "DumpDNS";
            if (For != null && For.Length > 0)
            {
                text += " for " + For;
            }
            Console.Write(text + new string(' ', LastW - text.Length));
            Console.ResetColor();
        }

        /// <summary>
        /// Render the bottom bar
        /// </summary>
        /// <param name="instructions">The set of instructions to show</param>
        /// <see cref="BottomInstructionsDictionary"/>
        static void RenderBottom(BottomInstructions instructions)
        {
            Console.CursorTop = LastH - 1;
            Console.CursorLeft = 0;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            string instruction = BottomInstructionsDictionary[instructions];
            if (CanSearch && false) instruction += " | Ctrl+F: Search"; // Disabled for now
            if (CanDump) instruction += " | Ctrl+D: Dump";
            Console.Write(instruction + new string(' ', LastW - instruction.Length));
            Console.ResetColor();

            // Render the version string, if enabled
            if(Functionality.Version.IsVisible)
            {
                Console.CursorLeft = Console.BufferWidth - Functionality.Version.VersionString.Length;
                if(Functionality.Version.IsNewVersionAvailable)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                } else if(Functionality.Version.Unreleased)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                } else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(Functionality.Version.VersionString);
            }
            Console.ResetColor();
        }
    }
}
