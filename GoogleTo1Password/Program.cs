using CsvHelper;
using CsvHelper.Configuration;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;

namespace GoogleTo1Password
{
    class Program
    {
        // E.g. -InputFile "C:\Users\piete\OneDrive\1Password\Chrome Passwords.csv"
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option("-InputFile")]
        [Required]
        string InputFile { get; }

        class Login
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        class LoginMap : ClassMap<Login>
        {
            public LoginMap()
            {
                AutoMap();
                Map(m => m.Name).Name("name");
                Map(m => m.Url).Name("url");
                Map(m => m.Username).Name("username");
                Map(m => m.Password).Name("password");
            }
        }



        private int OnExecute()
        {
            string inputCsv = InputFile;
            Console.WriteLine($"Input File Name : {inputCsv}");

            // Follow instructions
            // https://support.1password.com/import-chrome/

            // Read input CSV file
            TextReader reader = new StreamReader(inputCsv);
            CsvReader csvReader = new CsvReader(reader);
            csvReader.Configuration.RegisterClassMap<LoginMap>();
            var records = csvReader.GetRecords<Login>();

            // Open CSV output file
            string outputCsv = Path.ChangeExtension(inputCsv, ".conv.csv");
            Console.WriteLine($"Output File Name : {outputCsv}");
            TextWriter writer = new StreamWriter(outputCsv);
            CsvWriter csvWriter = new CsvWriter(writer);
            csvWriter.Configuration.RegisterClassMap<LoginMap>();
            csvWriter.WriteHeader<Login>();
            csvWriter.NextRecord();

            // Process all records
            int input = 0, output = 0;
            foreach (Login login in records)
            {
                input ++;

                // Skip entries for IP addresses
                IPAddress ip;
                if (IPAddress.TryParse(login.Name, out ip))
                    continue;

                // Skip entries where the username or password field contains ***
                if (login.Username.Contains("***") || login.Password.Contains("***"))
                    continue;

                // Crack the URI and use only the domain name
                Uri uri = new Uri(login.Url);
                login.Url = $"{uri.Scheme}://{uri.Host}";

                // Write the record
                csvWriter.WriteRecord(login);
                csvWriter.NextRecord();
                output ++;
            }

            // Close
            reader.Close();
            writer.Close();

            Console.WriteLine($"Wrote {output} of {input} records.");

            return 0;
        }
    }
}
