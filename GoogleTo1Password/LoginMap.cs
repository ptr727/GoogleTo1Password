using CsvHelper.Configuration;

namespace GoogleTo1Password
{
    internal sealed class LoginMap : ClassMap<Login>
    {
        internal LoginMap()
        {
            Map(m => m.Name).Name("name");
            Map(m => m.Url).Name("url");
            Map(m => m.Username).Name("username");
            Map(m => m.Password).Name("password");
        }
    }
}
