using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

static class OffsetManager
{
    static readonly HttpClient client = new();

    public static async Task<Dictionary<string, nint>> LoadAsync()
    {
        var urls = new[]
        {
            "https://raw.githubusercontent.com/a2x/cs2-dumper/refs/heads/main/output/offsets.cs",
            "https://raw.githubusercontent.com/a2x/cs2-dumper/refs/heads/main/output/client_dll.cs",
            "https://raw.githubusercontent.com/a2x/cs2-dumper/refs/heads/main/output/buttons.cs"
        };

        var dict = new Dictionary<string, nint>();

        var regex = new Regex(
            @"public\s+const\s+nint\s+(\w+)\s*=\s*0x([0-9A-Fa-f]+)",
            RegexOptions.Compiled
        );

        foreach (var url in urls)
        {
            var cs = await client.GetStringAsync(url);

            foreach (Match m in regex.Matches(cs))
            {
                var name = m.Groups[1].Value;
                var value = nint.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                dict[name] = value;
            }
        }

        return dict;
    }
}
