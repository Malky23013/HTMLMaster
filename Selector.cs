using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicode2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new List<string>(); // Ensure Classes is always initialized
        }
        public Selector(string tagName, string id, List<string> classes)
        {
            TagName = tagName;
            Id = id;
            Classes = classes ?? new List<string>();
        }
        public bool Matches(HtmlElement element)
        {
            if (TagName != null && TagName != element.Name)
            {
                return false;
            }

            if (Id != null && Id != element.Id)
            {
                return false;
            }

            if (Classes != null)
            {
                return element.Classes.All(classes => Classes.Contains(classes));
            }

            return true;
        }

        public static Selector ConvertQueryStringToSelector(string queryString)
        {
            // Create the root selector outside the loop.
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;

            string[] parts = queryString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                if (part.StartsWith("#"))
                {
                    currentSelector.Id = part.Substring(1);
                    continue;
                }
                else if (part.StartsWith("."))
                {
                    currentSelector.Classes.Add(part.Substring(1));
                    continue;
                }
                else
                {
                    // Check if it's a valid HTML tag name.
                    if (!IsValidTagName(part))
                    {
                        // Print the invalid tag name before throwing the exception.
                        Console.WriteLine($"Invalid tag name: {part}");
                        //throw new ArgumentException($"Invalid tag name: {part}");
                    }


                    Selector childSelector = new Selector { TagName = part };
                    currentSelector.Child = childSelector;
                    currentSelector = childSelector;
                }
            }

            return rootSelector;
        }

        private static bool IsValidTagName(string tagName)
        {
            // Regular expression for valid HTML tag names:
            // - Starts with a letter (a-z or A-Z)
            // - Can be followed by letters, numbers, hyphens, underscores, or colons
            const string pattern = "^[a-zA-Z][a-zA-Z0-9/-_:]{0,}$";
            return Regex.IsMatch(tagName, pattern);
        }
    }
}




