using System.Text;
using DnsClient;

namespace DumpDNS
{
    internal class Program
    {
        static int LastW = 0;
        static int LastH = 0;
        static Task? ResizeTask;
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                return CLI.CLI.Run(args);
            }

            return CLI.CLI.Run(["-?"]);

            /*Console.Title = "DumpDNS";
            Console.CursorVisible = false;
            Console.Clear();
            SizeChanged += static (sender, dimensions) =>
            {
                Console.ResetColor();
                Console.Clear(); // Wipe it
                LastW = dimensions.Item1;
                LastH = dimensions.Item2;
                Render?.Invoke(sender, dimensions); // Call render if it is defined
                RenderList.Add(Components.TopBar.Render);
                RenderList.Add(Components.StatusBar.Render);
                RenderList.Add(Components.BottomBar.Render);
            };

            ResizeTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    Components.StatusBar.CheckRender();
                    await Internal.ITask.StartQueue();
                    RenderList.Render(new((LastW, LastH)));
                    if (Console.BufferWidth != LastW || Console.BufferHeight != LastH)
                    {
                        SizeChanged(null, (Console.BufferWidth, Console.BufferHeight));
                    }
                    //Thread.Sleep(100); // Wait, so that it doesn't freeze
                    await Task.Delay(100);
                }
            });

            Render += static async (sender, dimensions) =>
            {
                RenderTop();
                RenderBottom(ActiveInstructions);
                RenderList.Render(new(dimensions));
            };

            UpdateBottom += static (object? sender, EventArgs e) =>
            {
                Render(sender, (Console.BufferWidth, Console.BufferHeight));
            };

            SizeChanged(null, (Console.BufferWidth, Console.BufferHeight));

            Functionality.Version.StartCheck();

            Internal.ITask.Enqueue(new Internal.Tasks.Version());
            
            RenderList.Add(Components.TopBar.Render);
            RenderList.Add(Components.StatusBar.Render);
            RenderList.Add(Components.BottomBar.Render);

            while (true)
            {
                For = "";
                CanDump = false;
                Domain = null;
                Console.Clear();

                Internal.ITask.StartQueue();

                // First stage, select a domain
                Functionality.DomainSelection DomainSelection = Functionality.DomainSelection.Start((LastW, LastH));
                if (DomainSelection.Success == false) return 1;
                For = DomainSelection.Domain.ToString();
                Domain = DomainSelection.Domain.ToString();

                Console.Clear();

                // Second stage, dump DNS records
                IDnsQueryResponse dump = Functionality.Dump.Start(DomainSelection.Domain.ToString(), (LastW, LastH), DomainSelection.Dns.ToString());

                Console.Clear();

                // Last stage, show the results
                CanDump = true;
                bool exit = !Functionality.Results.Start(dump, (LastW, LastH));

                if (exit) return 0;
            }*/
        }

        /// <summary>
        /// You MUST deregister the event after the section has finished (So it no longer needs to be rendered)
        /// It WILL render over everything else and mess things up
        /// </summary>
        public static EventHandler<(int, int)>? Render; // Render whatever is being displayed, needs to be changed
        public static EventHandler<(int, int)>? SizeChanged;
        public static EventHandler? UpdateBottom; // Updates specifically the bottom bar

        public static string? Domain;

        public static string For = string.Empty;
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
            [BottomInstructions.Text] = "Enter: Finish | Ctrl+R: Clear",
            [BottomInstructions.Options] = "Up/Down/Enter: Select",
            [BottomInstructions.OptionsWithBack] = "Up/Down/Enter: Select | Escape/Left: Back",
            [BottomInstructions.OptionsWithBackSecondary] = "Up/Down/Enter: Select | Left: Back",
            [BottomInstructions.Processing] = "Escape: Cancel",
            [BottomInstructions.ProcessingNoCancel] = "No options available",

            // With exit
            [BottomInstructions.TextWithExit] = "Enter: Finish | Tab: Switch | Ctrl+R: Clear | Ctrl+C: Exit",
            [BottomInstructions.OptionsWithExit] = "Up/Down/Enter: Select | Ctrl+C: Exit",
            [BottomInstructions.OptionsWithBackWithExit] = "Up/Down/Enter: Select | Escape/Left: Back | Ctrl+C Exit",
            [BottomInstructions.OptionsWithBackSecondaryWithExit] = "Up/Down/Enter: Select | Left: Back | Ctrl+C: Exit",
            [BottomInstructions.ProcessingWithExit] = "Escape: Cancel | Ctrl+C: Exit",
            [BottomInstructions.ProcessingNoCancelWithExit] = "Ctrl+C: Exit",

            // Search specific
            [BottomInstructions.Search] = "Escape: Back"
        };

        public static bool IsSearchBar = false; // Whether or not to display the search bar
        public static bool CanSearch = false;
        public static bool CanDump = false;

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
            if (IsSearchBar) return; // Handled elsewhere

            /*Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            string text = "DumpDNS";
            if (For != null && For.Length > 0)
            {
                text += " for " + For;
            }
            Console.Write(text + new string(' ', LastW - text.Length));
            Console.ResetColor();*/
        }

        /// <summary>
        /// Render the bottom bar
        /// </summary>
        /// <param name="instructions">The set of instructions to show</param>
        /// <see cref="BottomInstructionsDictionary"/>
        static void RenderBottom(BottomInstructions instructions)
        {
            /*Console.CursorTop = LastH - 1;
            Console.CursorLeft = 0;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            string instruction = BottomInstructionsDictionary[instructions];
            if (CanSearch && false) instruction += " | Ctrl+F: Search"; // Disabled for now
            if (CanDump) instruction += " | Ctrl+D: Dump";
            Console.Write(instruction + new string(' ', LastW - instruction.Length));
            Console.ResetColor();

            // Render the version string, if enabled
            if (Functionality.Version.IsVisible)
            {
                Console.CursorLeft = Console.BufferWidth - Functionality.Version.VersionString.Length;
                if (Functionality.Version.IsNewVersionAvailable)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                else if (Functionality.Version.Unreleased)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(Functionality.Version.VersionString);
            }
            Console.ResetColor();*/
        }
    }
}
