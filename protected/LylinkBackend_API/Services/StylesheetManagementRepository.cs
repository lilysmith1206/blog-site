using ExCSS;
using LylinkBackend_API.Models;
using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using System.Globalization;
using System.Text;

namespace LylinkBackend_API.Services
{
    public class StylesheetManagementRepository : IStylesheetManagementRepository
    {
        public IEnumerable<PageLink> GetAllStylesheets()
        {
            return Directory.GetFiles(Path.GetFullPath(@".\wwwroot\css"))
                    .Where(stylesheetPath => stylesheetPath.EndsWith(".css"))
                    .Select(stylesheetPath =>
                    {
                        TextInfo textInfo = new CultureInfo("en-us").TextInfo;

                        string stylesheetFileName = new FileInfo(stylesheetPath).Name.Replace(".css", string.Empty);

                        string stylesheetDisplayName = string.Join(" ", stylesheetFileName.Split("_").Select(word => textInfo.ToTitleCase(word)));

                        return new PageLink
                        {
                            Name = stylesheetDisplayName,
                            Slug = stylesheetFileName
                        };
                    });
        }

        public IEnumerable<CssRule> RetrieveStylesheetData(string stylesheetName)
        {
            string relativeCssFilePath = GetRelativeCssFilePath(stylesheetName);

            if (File.Exists(relativeCssFilePath))
            {
                string cssFileText = File.ReadAllText(relativeCssFilePath);

                return ParseCss(cssFileText);
            }

            throw new InvalidDataException($"No stylesheet with name {stylesheetName} exists.");
        }

        public bool UpdateStylesheet(string stylesheetName, IEnumerable<CssRule> rules)
        {
            try
            {
                string relativeCssFilePath = GetRelativeCssFilePath(stylesheetName);
                string stringifiedCssSheet = ParseCss(rules);

                File.WriteAllText(relativeCssFilePath, stringifiedCssSheet);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GetRelativeCssFilePath(string stylesheetName)
        {
            return @$"wwwroot\css\{stylesheetName}.css";
        }

        private static IEnumerable<CssRule> ParseCss(string cssText)
        {
            var parser = new StylesheetParser(true, true, true, true, true, false, false);
            Stylesheet stylesheet = parser.Parse(cssText);

            var rulesList = new List<CssRule>();

            foreach (IStylesheetNode node in stylesheet.Children)
            {
                if (node is StyleRule rule)
                {
                    var selectors = rule.SelectorText
                        .Split(',')
                        .Select(selector => selector.Trim());

                    var declarations = rule.Style
                        .Select(declaration => new CssDeclaration
                        {
                            Property = declaration.Name,
                            Value = declaration.Value
                        });

                    rulesList.Add(new CssRule
                    {
                        Selectors = selectors,
                        Declarations = declarations
                    });
                }
                else if (node is IKeyframeRule keyframeSelector)
                {

                }
            }

            return rulesList;
        }

        private static string ParseCss(IEnumerable<CssRule> cssRules)
        {
            StringBuilder cssStringBuilder = new();

            foreach (CssRule cssRule in cssRules)
            {
                cssStringBuilder.AppendLine(@$"{string.Join(",", cssRule.Selectors)} {{");

                foreach (CssDeclaration cssDeclaration in cssRule.Declarations)
                {
                    cssStringBuilder.AppendLine($"\t{cssDeclaration.Property}: {cssDeclaration.Value};");
                }

                cssStringBuilder.AppendLine("}\n");
            }

            return cssStringBuilder.ToString();
        }
    }
}
