using CsvHelper;
using System;
using System.IO;
using System.Net;

namespace GoogleTo1Password
{
    class Program
    {
        class Login
        {
            public string name { get; set; }
            public string url { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }

        static void Main(string[] args)
        {
            // Follow instructions
            // https://support.1password.com/import-chrome/

            // Read input CSV file
            string inputCsv = @"C:\Users\piete\OneDrive\1Password\Chrome Passwords.csv";
            TextReader reader = new StreamReader(inputCsv);
            CsvReader csvReader = new CsvReader(reader);
            var records = csvReader.GetRecords<Login>();

            // Open CSV output file
            string outputCsv = inputCsv + "_filtered.csv";
            TextWriter writer = new StreamWriter(outputCsv);
            CsvWriter csvWriter = new CsvWriter(writer);
            csvWriter.WriteHeader<Login>();
            csvWriter.NextRecord();

            // Process all records
            foreach (Login login in records)
            {
                // Skip entries for IP addresses
                IPAddress ip;
                if (IPAddress.TryParse(login.name, out ip))
                    continue;

                // Skip entries where hte username or password field contains ***
                if (login.username.Contains("***") || login.password.Contains("***"))
                    continue;

                // Crack the URI and use only the domain name
                Uri uri = new Uri(login.url);
                login.url = $"{uri.Scheme}://{uri.Host}";

                // Write the record
                csvWriter.WriteRecord(login);
                csvWriter.NextRecord();
            }

            // Close
            reader.Close();
            writer.Close();
        }
    }
}
