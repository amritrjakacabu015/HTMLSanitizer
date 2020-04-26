using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace APISanitizer
{
    public static class Sanitizer
    {
        
            /// <summary>
            /// This HashSet contains HTML tags and attributes to be blacklisted from our application
            /// </summary>
            private static readonly HashSet<string> HtmlBlackList = new HashSet<string>()
            {
                { "script" },
                { "iframe" },
                { "form" },
                { "object" },
                { "embed" },
                { "link" },
                { "head" },
                { "meta" }
            };



            /// <summary>
            /// Cleans up an HTML string and removes HTML tags in blacklist
            /// </summary>
            /// <param name="html"></param>
            /// <param name="blackList"></param>
            /// <returns></returns>
            public static string SanitizeHtml(string html)
            {
                return Sanitize(html);
            }

            /// <summary>
            /// Cleans up an HTML string by removing elements
            /// on the blacklist and all elements that start
            /// with onXXX .
            /// </summary>
            /// <param name="inputHtmlString"></param>
            /// <returns></returns>
            public static string Sanitize(string inputHtmlString)
            {
                string output = null;
                var document = new HtmlDocument();

                document.LoadHtml(inputHtmlString);
                SanitizeHtmlNode(document.DocumentNode);

                // Use an XmlTextWriter to create self-closing tags
                using (var sw = new StringWriter())
                {
                    XmlWriter writer = new XmlTextWriter(sw);
                    document.DocumentNode.WriteTo(writer);
                    output = sw.ToString();

                    // strip off XML doc header
                    if (!string.IsNullOrEmpty(output))
                    {
                        var at = output.IndexOf("?>", StringComparison.Ordinal);
                        output = output.Substring(at + 2);
                    }

                    writer.Close();
                }

                return output;
            }

            private static void SanitizeHtmlNode(HtmlNode node)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    // check for blacklist items and remove
                    if (HtmlBlackList.Contains(node.Name))
                    {
                        node.Remove();
                        return;
                    }

                    // remove CSS Expressions and embedded script links
                    if (node.Name == "style")
                    {
                        if (string.IsNullOrEmpty(node.InnerText))
                        {
                            if (node.InnerHtml.Contains("expression") || node.InnerHtml.Contains("javascript:"))
                                node.ParentNode.RemoveChild(node);
                        }
                    }

                    // remove script attributes
                    if (node.HasAttributes)
                    {
                        for (var i = node.Attributes.Count - 1; i >= 0; i--)
                        {
                            var currentAttribute = node.Attributes[i];

                            var attributeName = currentAttribute.Name.ToLower();
                            var attributeValue = currentAttribute.Value.ToLower();

                            if (attributeName.StartsWith("on"))
                                node.Attributes.Remove(currentAttribute);

                            // remove script links
                            else if ((attributeName == "href" || attributeName == "src" || attributeName == "dynsrc" || attributeName == "lowsrc") && attributeValue.Contains("javascript:"))
                                node.Attributes.Remove(currentAttribute);

                            // Remove CSS Expressions
                            else if (attributeName == "style" && attributeValue.Contains("expression") || attributeValue.Contains("javascript:") || attributeValue.Contains("vbscript:"))
                                node.Attributes.Remove(currentAttribute);
                        }
                    }
                }

                // Look through child nodes recursively
                if (!node.HasChildNodes) return;
                {
                    for (var i = node.ChildNodes.Count - 1; i >= 0; i--)
                    {
                        SanitizeHtmlNode(node.ChildNodes[i]);
                    }
                }
            }
        }
    }
