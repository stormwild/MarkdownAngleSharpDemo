# Markdig AngleSharp Demo

```sh
dotnet new sln -o MarkdownAngleSharpDemo
cd MarkdownAngleSharpDemo/
dotnet new console -o MarkdownAngleSharpDemo.Console
dotnet sln ../MarkdownAngleSharpDemo.sln add MarkdownAngleSharpDemo.Console.csproj
dotnet add package Markdig --version 0.30.3
dotnet add package AngleSharp --version 0.17.1
dotnet run
```

`Program.cs`
```csharp
using AngleSharp;
using Markdig;
using MarkdownAngleSharpDemo.Console;

Console.WriteLine("Hello, World!");

var data = @"
| Id       | Name                    |
|----------|-------------------------|
| 1        | Mercury                 |
| 2        | Venus                   |
| 3        | Earth                   |
";

var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
var parsed = Markdown.ToHtml(data, pipeline);

Console.WriteLine(parsed);

var config = Configuration.Default;

using var context = BrowsingContext.New(config);
using var doc = await context.OpenAsync(req => req.Content(parsed));

var rows = doc.QuerySelectorAll("tbody > tr");

var planets = rows.Select((row, index) =>
{
    var parsed = Int32.TryParse(row.QuerySelector("td:nth-child(1)")?.TextContent ?? $"{index}", out int id);
    var name = row.QuerySelector("td:nth-child(2)")?.TextContent ?? string.Empty;

    return new Planet
    {
        Id = id,
        Name = name
    };
});

foreach (var planet in planets)
{
    Console.WriteLine($"{planet.Id}: {planet.Name}");
}

```

Sample Output

```sh
Hello, World!

<tr>
<td>1</td>
<td>Mercury</td>
</tr>
<tr>
<td>2</td>
<td>Venus</td>
</tr>
<tr>
<td>3</td>
<td>Earth</td>
</tr>
</tbody>
</table>

1: Mercury
2: Venus
3: Earth
```