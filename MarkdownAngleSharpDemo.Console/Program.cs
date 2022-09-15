// See https://aka.ms/new-console-template for more information

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
