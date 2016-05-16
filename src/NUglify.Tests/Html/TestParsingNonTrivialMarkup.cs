// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.



using NUnit.Framework;

namespace NUglify.Tests.Html
{
    /// <summary>
    /// Tests ported from html-minifier https://github.com/kangax/html-minifier/blob/gh-pages/tests/minifier.js
    /// Copyright(c) 2010 - 2016 Juriy "kangax" Zaytsev
    /// MIT License - https://github.com/kangax/html-minifier/blob/gh-pages/LICENSE
    /// </summary>
    [TestFixture]
    public class TestParsingNonTrivialMarkup : TestHtmlParserBase
    {
        [Test]
        public void Test001()
        {
            equal(minify("</td>"), "", "(1,1): warning : Unable to find opening tag for closing tag </td>");
        }

        [Test]
        public void Test002()
        {
            equal(minify("</br>"),
                "<br>", "(1,1): warning : Invalid end tag </br> used instead of self closing tag <br/> or <br>");
        }

        [Test]
        public void Test003()
        {
            equal(minify("<br>x</br>"),
                "<br>x<br>", "(1,6): warning : Invalid end tag </br> used instead of self closing tag <br/> or <br>");
        }

        [Test]
        public void Test004()
        {
            equal(minify("<p title=\"</p>\">x</p>"), "<p title=\"</p>\">x");
        }

        [Test]
        public void Test005()
        {
            equal(minify("<p title=\" <!-- hello world --> \">x</p>"), "<p title=\" <!-- hello world --> \">x");
        }

        [Test]
        public void Test006()
        {
            equal(minify("<p title=\" <![CDATA[ \n\n foobar baz ]]> \">x"),
                "<p title=\" <![CDATA[ \n\n foobar baz ]]> \">x");
        }

        [Test]
        public void Test007()
        {
            equal(minify("<p foo-bar=baz>xxx</p>"), "<p foo-bar=baz>xxx");
            equal(minify("<p foo:bar=baz>xxx</p>"), "<p foo:bar=baz>xxx");
            equal(minify("<p foo.bar=baz>xxx</p>"), "<p foo.bar=baz>xxx");
        }

        [Test]
        public void Test008()
        {
            input = "<div><div><div><div><div><div><div><div><div><div>" +
                    "i\'m 10 levels deep" +
                    "</div></div></div></div></div></div></div></div></div></div>";

            equal(minify(input), input);
        }

        [Test]
        public void Test009()
        {
            // These are going through the minifier by default
            equal(minify("<script>alert(\'<!--\')</script>"), "<script>alert(\"<!--\")</script>");
            equal(minify("<script>alert(\'<!-- foo -->\')</script>"), "<script>alert(\"<!-- foo -->\")</script>");
            equal(minify("<script>alert(\'-->\')</script>"), "<script>alert(\"-->\")</script>");
        }

        [Test]
        public void Test010()
        {
            equal(minify("<a title=\"x\"href=\" \">foo</a>"), "<a title=x href=\"\">foo</a>",
                "(1,13): error : Invalid character 'h' found while parsing <a>. Expecting a whitespace before an attribute");
            equal(minify("<p id=\"\"class=\"\"title=\"\">x"), "<p>x",
                                                              "(1,9): error : Invalid character 'c' found while parsing <p>. Expecting a whitespace before an attribute",
                                                              "(1,17): error : Invalid character 't' found while parsing <p>. Expecting a whitespace before an attribute");
            equal(minify("<p x=\"x\'\"\">x</p>"), "<p x=\"x'\">x",
                "(1,10): error : Invalid character '\"' found while parsing <p>. Expecting a whitespace before an attribute",
                "(1,10): error : Invalid character '\"' found while parsing <p>");
        }

        [Test]
        public void Test011()
        {
            equal(minify("<a href=\"#\"><p>Click me</p></a>"), "<a href=#><p>Click me</p></a>");
            equal(minify("<span><button>Hit me</button></span>"), "<span><button>Hit me</button></span>");
            equal(minify("<object type=\"image/svg+xml\" data=\"image.svg\"><div>[fallback image]</div></object>"),
                "<object type=image/svg+xml data=image.svg><div>[fallback image]</div></object>"
                );

        }

        [Test]
        public void Test012()
        {
            equal(minify("<ng-include src=\"x\"></ng-include>"), "<ng-include src=x></ng-include>");
            equal(minify("<ng:include src=x></ng:include>"), "<ng:include src=x></ng:include>");
            equal(
                minify("<ng-include src=\"\'views/partial-notification.html\'\"></ng-include><div ng-view=\"\"></div>"),
                "<ng-include src=\"\'views/partial-notification.html\'\"></ng-include><div ng-view=\"\"></div>"
                );
        }

        [Test]
        public void Test013()
        {
            // will cause test to time-out if fail
            input = "<p>For more information, read <a href=https://stackoverflow.com/questions/17408815/fieldset-resizes-wrong-appears-to-have-unremovable-min-width-min-content/17863685#17863685>this Stack Overflow answer</a>.</p>";
            output = "<p>For more information, read <a href=https://stackoverflow.com/questions/17408815/fieldset-resizes-wrong-appears-to-have-unremovable-min-width-min-content/17863685#17863685>this Stack Overflow answer</a>.";
            equal(minify(input), output);
        }

        [Test]
        public void Test014()
        {
            input = "<html ⚡></html>";
            equal(minify(input), "<html ⚡>");

            //input = "<$unicorn>";
            //throws(function() {
            //    minify(input);
            //}, 'Invalid tag name");

            input =
                "<begriffs.pagination ng-init=\"perPage=20\" collection=\"logs\" url=\"\'/api/logs?user=-1\'\" per-page=\"perPage\" per-page-presets=\"[10,20,50,100]\" template-url=\"/assets/paginate-anything.html\"></begriffs.pagination>";
            output =
                "<begriffs.pagination ng-init=\"perPage=20\" collection=logs url=\"\'/api/logs?user=-1\'\" per-page=perPage per-page-presets=[10,20,50,100] template-url=/assets/paginate-anything.html></begriffs.pagination>";
            equal(minify(input), output);

            // https://github.com/kangax/html-minifier/issues/41
            equal(minify("<some-tag-1></some-tag-1><some-tag-2></some-tag-2>"),
                "<some-tag-1></some-tag-1><some-tag-2></some-tag-2>"
                );
        }

        [Test]
        public void Test015()
        {
            // https://github.com/kangax/html-minifier/issues/40
            equal(minify("[\'][\"]"), "[\'][\"]");

            // https://github.com/kangax/html-minifier/issues/21
            equal(minify("<a href=\"test.html\"><div>hey</div></a>"), "<a href=test.html><div>hey</div></a>");

            // https://github.com/kangax/html-minifier/issues/17
            equal(minify(":) <a href=\"http://example.com\">link</a>"), ":) <a href=http://example.com>link</a>");

            // https://github.com/kangax/html-minifier/issues/169
            equal(minify("<a href>ok</a>"), "<a href>ok</a>");

            equal(minify("<a onclick></a>"), "<a></a>");

            // https://github.com/kangax/html-minifier/issues/229
            equal(minify("<CUSTOM-TAG></CUSTOM-TAG><div>Hello :)</div>"), "<custom-tag></custom-tag><div>Hello :)</div>");

            //// https://github.com/kangax/html-minifier/issues/507
            input = "<tag v-ref:vm_pv :imgs=\" objpicsurl_ \"></ tag > ";
            equal(minify(input), "<tag v-ref:vm_pv :imgs=\" objpicsurl_ \"></ tag ></tag>"
                , "(1,42): error : Invalid character ' ' following end tag </");

            equal(minify("<tag v-ref:vm_pv :imgs=\" objpicsurl_ \" ss\"123 ></ tag > "),
                "<tag v-ref:vm_pv :imgs=\" objpicsurl_ \" ss 123></ tag ></tag>",
                "(1,42): error : Invalid character '\"' found while parsing <tag>. Expecting a whitespace before an attribute",
                "(1,42): error : Invalid character '\"' found while parsing <tag>",
                "(1,50): error : Invalid character ' ' following end tag </");

            // https://github.com/kangax/html-minifier/issues/512
            input = "<input class=\"form-control\" type=\"text\" style=\"\" id=\"{{vm.formInputName}}\" name=\"{{vm.formInputName}}\"" +
                    " placeholder=\"YYYY-MM-DD\"" +
                    " date-range-picker" +
                    " data-ng-model=\"vm.value\"" +
                    " data-ng-model-options=\"{ debounce: 1000 }\"" +
                    " data-ng-pattern=\"vm.options.format\"" +
                    " data-options=\"vm.datepickerOptions\">";
            output = "<input class=form-control type=text id={{vm.formInputName}} name={{vm.formInputName}}" +
                    " placeholder=YYYY-MM-DD" +
                    " date-range-picker" +
                    " data-ng-model=vm.value" +
                    " data-ng-model-options=\"{ debounce: 1000 }\"" +
                    " data-ng-pattern=vm.options.format" +
                    " data-options=vm.datepickerOptions>";
            equal(minify(input), output);
        }
    }
}