using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicode2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public void PrintTree(int depth = 0)
        {
            string indent = new string(' ', depth * 2);
            Console.WriteLine($"{indent}<{Name} {(Id != null ? $"id='{Id}'" : "")} {(Classes.Count > 0 ? $"class='{string.Join(" ", Classes)}'" : "")}>");

            foreach (var child in Children)
            {
                child.PrintTree(depth + 1);
            }

            Console.WriteLine($"{indent}</{Name}>");
        }
        // קונסטרקטור ברירת מחדל
        public HtmlElement()
        {
            Attributes = new List<string>();
            Children = new List<HtmlElement>();
        }
    
        // קונסטרקטור עם פרמטרים
        public HtmlElement(string name, HtmlElement parent)
        {
            Name = name;
            Parent = parent;
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        // קונסטרקטור עם פרמטרים
        public HtmlElement(string name, string id, List<string> classes)
        {
            Name = name;
            Id = id;
            Classes = classes ?? new List<string>();
            Attributes = new List<string>();
            Children = new List<HtmlElement>();
        }

        // פונקציה Descendants מממשה
        // Descendants function
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            HashSet<HtmlElement> visitedElements = new HashSet<HtmlElement>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                if (visitedElements.Add(element))
                {
                    yield return element;

                    foreach (var child in element.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }
        // Ancestors function
        public IEnumerable<HtmlElement> Ancestors()
        {
            var parent = this.Parent;

            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

        // FindElements function
        public static List<HtmlElement> FindElements(HtmlElement element, Func<HtmlElement, bool> predicate)
        {
            List<HtmlElement> result = new List<HtmlElement>();
            HashSet<HtmlElement> visitedElements = new HashSet<HtmlElement>();
            FindElementsRecursive(element, predicate, result, visitedElements);
            return result;
        }

        private static void FindElementsRecursive(HtmlElement element, Func<HtmlElement, bool> predicate, List<HtmlElement> result, HashSet<HtmlElement> visitedElements)
        {
            if (visitedElements.Add(element) && predicate(element))
            {
                result.Add(element);
            }

            var descendants = element.Descendants();

            foreach (var descendant in descendants)
            {
                if (visitedElements.Add(descendant) && predicate(descendant))
                {
                    result.Add(descendant);
                }
            }
        }
    }


}

