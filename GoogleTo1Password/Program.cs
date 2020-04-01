using CsvHelper;
using System;
using System.IO;
using System.Net;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Globalization;
using System.Collections.Generic;

namespace GoogleTo1Password
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            RootCommand rootCommand = CreateCommandLineOptions();
            return rootCommand.Invoke(args);
        }

        private static RootCommand CreateCommandLineOptions()
        {
            // Root command and global options
            RootCommand rootCommand =
                new RootCommand("Utility to convert Google Chrome passwords to 1Password passwords.")
                {
                    Handler = CommandHandler.Create<string, string>(ConvertCommand)
                };

            // Google passwords input file
            rootCommand.AddOption(
                new Option<string>("--google")
                {
                    Description = "Path to Google passwords file.",
                    Required = true
                });

            // 1Password passwords output file
            rootCommand.AddOption(
                new Option<string>("--onepassword")
                {
                    Description = "Path to 1Password passwords file.",
                    Required = true
                });

            return rootCommand;
        }

        private static int ConvertCommand(string google, string onepassword)
        {
            Console.WriteLine($"Converting \"{google}\" to \"{onepassword}\".");

            // Read input CSV file
            using StreamReader streamReader = new StreamReader(File.OpenRead(google));
            using CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csvReader.Configuration.RegisterClassMap<LoginMap>();
            IEnumerable<Login> loginRecords = csvReader.GetRecords<Login>();

            // Open CSV output file
            using StreamWriter streamWriter = new StreamWriter(onepassword);
            using CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            csvWriter.Configuration.RegisterClassMap<LoginMap>();

            // Write the header
            csvWriter.WriteHeader<Login>();
            csvWriter.NextRecord();

            // Process all records
            int input = 0, output = 0;
            foreach (Login login in loginRecords)
            {
                input ++;

                // Skip entries for IP addresses
                if (IPAddress.TryParse(login.Name, out IPAddress _))
                    continue;

                // Skip entries where the username contains *** or •••
                if (login.Username.Contains("***", StringComparison.OrdinalIgnoreCase) || login.Username.Contains("•••", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Crack the URI and use only the domain name portion
                Uri uri = new Uri(login.Url);
                login.Url = uri.Host;

                // Write the record
                csvWriter.WriteRecord(login);
                csvWriter.NextRecord();
                output ++;
            }

            // Close
            streamReader.Close();
            streamWriter.Close();

            Console.WriteLine($"Wrote {output} of {input} records.");

            return 0;
        }
    }
}
