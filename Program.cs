// See https://aka.ms/new-console-template for more information

using HtmlAgilityPack;
using practicode2;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

var html = await Load("https://learn.malkabruk.co.il");
var cleanHtml = new Regex("\\s").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
Console.WriteLine();


static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
 var response = await client.GetAsync(url);
  var html = await response.Content.ReadAsStringAsync();
   return html; 
}




var content = File.ReadAllText("AllTags.json");
Console.WriteLine(content);
//works perfectly!!!!!!!!!
// המרת נתוני הקובץ JSON למערך באמצעות JsonSerailizer
string[] allTags = JsonSerializer.Deserialize<string[]>(content);

// הדפסת המערך
foreach (var tag in allTags)
{
    Console.WriteLine(tag);
}

////Singleton

foreach (var item in HtmlHelper.Instance.AllHtmlTags)
{
    Console.WriteLine(item);
}
//Load all tags and self-closing tags
// Load all tags and self-closing tags
static void PrintHtmlTree(HtmlElement element, int depth)
{
    if (element == null)
    {
        return;
    }

    Console.WriteLine($"{new string(' ', depth * 2)}{element.Name}");

    foreach (var child in element.Children)
    {
        PrintHtmlTree(child, depth + 1);
    }
}
var allTags1 = HtmlHelper.Instance.AllHtmlTags;
var selfClosingTags = HtmlHelper.Instance.SelfClosingHtmlTags;

// Load HTML content
//html = await Load("https://www.w3schools.com");
 html = await Load("https://www.example.net/");
// Parse HTML
if (html != null)
{
    var rootElement = HtmlHelper.Instance.ParseHtml(HtmlHelper.Instance.ExtractHtmlLines(HtmlHelper.Instance.CleanHtml(html)));
    PrintHtmlTree(rootElement, 0);
}


////HTML QUERY---------------------------------------------------------------------------

// קריאת HTML ממקובץ או מאתר

// קריאת HTML ממקובץ או מאתר
static void PrintElements(IEnumerable<HtmlNode> elements)
{
    foreach (var element in elements)
    {
        Console.WriteLine(element.OuterHtml);
    }
}

// קריאת HTML ממקובץ או מאתר
static List<HtmlNode> QueryHtmlElements(string htmlContent, string selector)
{
    var htmlDocument = new HtmlDocument();
    htmlDocument.LoadHtml(htmlContent);

    // פילטור רווחים מיותרים
    selector = selector.Replace(" ", "");

    // פיצוץ ה-Selector לחלקיו
    var selectors = SplitSelectors(selector);

    // חיפוש על פי חלקי ה-Selector
    var currentElements = new List<HtmlNode> { htmlDocument.DocumentNode };

    foreach (var currentSelector in selectors)
    {
        var newElements = new List<HtmlNode>();

        foreach (var element in currentElements)
        {
            newElements.AddRange(FindElementsWithSelector(element, currentSelector));
        }

        currentElements = newElements;
    }

    return currentElements;
}

static IEnumerable<HtmlNode> FindElementsWithSelector(HtmlNode element, string selector)
{
    var elements = new List<HtmlNode>();

    // בדיקה האם יש ID, CLASS או תגית ב-Selector
    var idMatch = Regex.Match(selector, @"#(\w+)");
    var classMatch = Regex.Match(selector, @"\.(w+)");
    var tagMatch = Regex.Match(selector, @"(\w+)");

    if (idMatch.Success)
    {
        var idValue = idMatch.Groups[1].Value;
        var foundElement = element.SelectSingleNode($"//*[@id='{idValue}']");
        if (foundElement != null)
        {
            elements.Add(foundElement);
        }
    }
    else if (classMatch.Success)
    {
        var classValue = classMatch.Groups[1].Value;
        elements.AddRange(element.SelectNodes($".//*[contains(@class, '{classValue}')]") ?? Enumerable.Empty<HtmlNode>());
    }
    else if (tagMatch.Success)
    {
        var tagName = tagMatch.Groups[1].Value;
        elements.AddRange(element.SelectNodes($"//{tagName}") ?? Enumerable.Empty<HtmlNode>());
    }

    return elements;
}

// פצוץ את ה-selector לחלקיו
static List<string> SplitSelectors(string selector)
{
    var selectors = selector.Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(s => s.Trim())
                           .ToList();

    return selectors;
}

static async Task<string> GetHtmlContentAsync(string url)
{
    using (var httpClient = new HttpClient())
    {
        return await httpClient.GetStringAsync(url);
    }
}

// קריאת HTML מאתר
string htmlContent = await GetHtmlContentAsync("https://www.w3schools.com/html/default.asp");

// שאילתות עם חיפוש לפי סלקטור
//string selector1 = "div#main.w3-col l10 m12";
//string selector2 = "div#getdiploma.w3-panel";
//string selector3 = "div#certified_html_css_links_list";

//var matchingElements1 = QueryHtmlElements(htmlContent, selector1);
//var matchingElements2 = QueryHtmlElements(htmlContent, selector2);
//var matchingElements3 = QueryHtmlElements(htmlContent, selector3);

//// הדפסת התוצאות
//Console.WriteLine("Matching elements for selector1:");
//PrintElements(matchingElements1);

//Console.WriteLine("\nMatching elements for selector2:");
//PrintElements(matchingElements2);

//Console.WriteLine("\nMatching elements for selector3:");
//PrintElements(matchingElements3);

//SELECTOR


static void PrintSelectorHierarchy(Selector selector)
{
    Console.WriteLine("Selector Hierarchy:");
    Selector current = selector;
    while (current != null)
    {
        Console.WriteLine($"Tag: {current.TagName}, Id: {current.Id}, Classes: {string.Join(", ", current.Classes)}");
        current = current.Child;
    }
    Console.WriteLine();
}
string queryString = "div #container .content p #content";
Selector selector1 = Selector.ConvertQueryStringToSelector(queryString);
PrintSelectorHierarchy(selector1);

queryString = "input #username";
selector1 = Selector.ConvertQueryStringToSelector(queryString);
PrintSelectorHierarchy(selector1);

queryString = "h1 .title";
selector1 = Selector.ConvertQueryStringToSelector(queryString);
PrintSelectorHierarchy(selector1);


////פונקציות---------------------------------------------------------------------------------------------------------
//// יצירת עץ של HtmlElement


var root = new HtmlElement { Name = "div" };
var child1 = new HtmlElement { Name = "span", Parent = root };
var child2 = new HtmlElement { Name = "p", Parent = root };
var grandchild = new HtmlElement { Name = "a", Parent = child1 };

root.Children.Add(child1);
root.Children.Add(child2);
child1.Children.Add(grandchild);

// Perform the Descendants function
Console.WriteLine("Descendants:");
foreach (var element in root.Descendants())
{
    Console.WriteLine(element.Name);
}

// Perform the Ancestors function
Console.WriteLine("\nAncestors:");
foreach (var element in grandchild.Ancestors())
{
    Console.WriteLine(element.Name);
}

// Perform the FindElements function
Console.WriteLine("\nFindElements:");
var elements = HtmlElement.FindElements(root, e => e.Name == "a");
foreach (var element in elements)
{
    Console.WriteLine(element.Name);
}
