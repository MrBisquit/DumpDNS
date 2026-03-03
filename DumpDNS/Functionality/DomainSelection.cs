using System.Text;

namespace DumpDNS.Functionality
{
    public class DomainSelection
    {
        public static DomainSelection Start((int, int) dimensions)
        {
            DomainSelection ds = new DomainSelection();
            Program.ActiveInstructions = Program.BottomInstructions.TextWithExit;
            Program.Render += ds.Render;
            Program.Render(ds, dimensions); // Cause it to render immediately
            ds.Dimensions = dimensions;
            ds.StartCycle();

            return ds;
        }

        public static char[] AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.1234567890".ToCharArray();

        public StringBuilder Domain;
        public int Cursor;
        public (int, int) Dimensions;

        public bool Success = false;
        public bool DomainValid = false;

        public StringBuilder Dns;
        public int DnsCursor;
        public bool DnsValid = false;

        public bool DnsSelected = false;

        public DomainSelection()
        {
            Domain = new StringBuilder();
            Dns = new StringBuilder();
            Cursor = 0;
            DnsCursor = 0;
            CheckValidity();
            CheckDNSValidity();
        }

        /// <summary>
        /// Waits for input before handling it
        /// </summary>
        public void StartCycle()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    // Ctrl+C should already be handled by the shell, so there is no point in creating it here
                    if (key.Key == ConsoleKey.R && key.Modifiers == ConsoleModifiers.Control)
                    {
                        Domain.Clear();
                        if (DnsSelected)
                            DnsCursor = 0;
                        else
                            Cursor = 0;
                    } else if(key.Key == ConsoleKey.Escape)
                    {
                        Program.Render -= Render;
                        break;
                    } else if(key.Key == ConsoleKey.LeftArrow)
                    {
                        if (Cursor == 0) continue;
                        if (DnsSelected)
                            DnsCursor--;
                        else
                            Cursor--;
                    } else if(key.Key == ConsoleKey.RightArrow)
                    {
                        if (Cursor == Domain.Length) continue;
                        if (DnsSelected)
                            DnsCursor++;
                        else
                            Cursor++;
                    } else if(key.Key == ConsoleKey.Backspace)
                    {
                        if (Domain.Length == 0) continue;
                        if(DnsSelected)
                        {
                            Dns.Remove(DnsCursor - 1, 1);
                            DnsCursor--;
                        } else
                        {
                            Domain.Remove(Cursor - 1, 1);
                            Cursor--;
                        }
                    } else if(key.Key == ConsoleKey.Delete)
                    {
                        if (Cursor == Domain.Length) continue;
                        if (DnsSelected)
                            Dns.Remove(DnsCursor, 1);
                        else
                            Domain.Remove(Cursor, 1);
                    } else if(key.Key == ConsoleKey.Enter)
                    {
                        if(CheckValidity() && CheckDNSValidity())
                        {
                            Success = true;
                            Program.Render -= Render;
                            break;
                        }
                    } else if(key.Key == ConsoleKey.Tab)
                    {
                        DnsSelected = !DnsSelected;
                    } else
                    {
                        if(DnsSelected)
                        {
                            Dns.Insert(DnsCursor, key.KeyChar);
                            DnsCursor++;
                        } else
                        {
                            if (Domain.Length == 256 || Domain.Length == Dimensions.Item1) continue;
                            if (AllowedChars.Contains(key.KeyChar))
                            {
                                Domain.Insert(Cursor, key.KeyChar);
                                Cursor++;
                            }
                        }
                    }

                    CheckValidity();
                    CheckDNSValidity();
                    Render(this, Dimensions);
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Really bad check, needs improvement
        /// </summary>
        /// <returns>If the domain is valid or not</returns>
        public bool CheckValidity()
        {
            DomainValid = true;
            string[] split = Domain.ToString().Split('.');
            if (split.Length <= 1) { DomainValid = false; }
            foreach (string s in split) if(s.Length == 0) { DomainValid = false; break; }
            return DomainValid;
        }

        public bool CheckDNSValidity()
        {
            DnsValid = true;
            if (Dns.Length == 0)
                return DnsValid;

            return DnsValid;
        }

        /// <summary>
        /// Render it
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="dimensions">Dimensions of the buffer</param>
        public void Render(object? sender, (int, int) dimensions)
        {
            Dimensions = dimensions;
            Console.CursorTop = 2;
            Console.CursorLeft = 0;

            Console.ResetColor();
            Console.WriteLine("Type the domain you would like to dump the DNS of (E.g. wtdawson.info, google.com, microsoft.com)");

            if (Domain.Length > dimensions.Item1)
            {
                Domain.Clear();
                Cursor = 0;
            }

            Console.BackgroundColor = !DnsSelected ? ConsoleColor.Gray : ConsoleColor.DarkGray;
            Console.WriteLine(new string(' ', dimensions.Item1));
            Console.CursorTop--;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(Domain.ToString());

            Console.CursorLeft = 0;
            if (DomainValid)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Domain valid" + new string(' ', dimensions.Item1 - "Domain valid".Length));
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Invalid domain" + new string(' ', dimensions.Item1 - "Invalid domain".Length));
            }

            // DNS server
            Console.CursorTop++;

            Console.ResetColor();
            Console.WriteLine("Type the DNS server you would like to use (leave blank to use your network's DNS server)");

            if(Dns.Length > dimensions.Item1)
            {
                Dns.Clear();
                Cursor = 0;
            }

            Console.BackgroundColor = DnsSelected ? ConsoleColor.Gray : ConsoleColor.DarkGray;
            Console.WriteLine(new string(' ', dimensions.Item1));
            Console.CursorTop--;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(Dns.ToString());

            Console.CursorLeft = 0;
            if (DnsValid)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Valid IP/DNS" + new string(' ', dimensions.Item1 - "Valid IP/DNS".Length));
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Invalid IP/DNS" + new string(' ', dimensions.Item1 - "Invalid IP/DNS".Length));
            }

            Console.CursorTop -= DnsSelected ? 2 : 6; // 2
            Console.CursorLeft = DnsSelected ? DnsCursor : Cursor;
            Console.ResetColor();
        }
    }
}
