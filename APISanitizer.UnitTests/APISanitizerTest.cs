using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace APISanitizer.UnitTests
{
    [TestClass]
    public class ApiSanitizerTest
    {
        [TestMethod]
        public void ImageTagTest()
        {
            const string testData = "<img alt=\"\" src=\"x\" onerror=\"alert(document.cookie)\" />";
            var result = Sanitizer.SanitizeHtml(testData);
            Assert.AreEqual("<img alt=\"\" src=\"x\" />", result);
        }

        [TestMethod]
        public void CustomHtmlTagTest()
        {
            const string testData = "<tq-form-field><tq-form><div>Hello World!</p></tq-form></tq-form-field>";
            var result = Sanitizer.SanitizeHtml(testData);
            Assert.AreEqual("<tq-form-field><tq-form><div>Hello World!</div></tq-form></tq-form-field>", result);
        }

        [TestMethod]
        public void AutoCloseTagTest()
        {
            const string testData = "<tq-form-field><tq-form><div>Hello World!</p>";
            var result = Sanitizer.SanitizeHtml(testData);
            Assert.AreEqual("<tq-form-field><tq-form><div>Hello World!</div></tq-form></tq-form-field>", result);
        }
    }
}
