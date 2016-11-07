﻿using NUnit.Framework;
using Roadkill.Core.DependencyResolution.StructureMap.Registries;
using Roadkill.Core.Plugins;
using Roadkill.Core.Text.CustomTokens;
using Roadkill.Core.Text.Parsers;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.Sanitizer;
using Roadkill.Core.Text.TextMiddleware;
using StructureMap;

namespace Roadkill.Tests.Unit.DependencyResolution
{
	[TestFixture]
	[Category("Unit")]
	public class TextRegistryTests : RegistryTestsBase
    {
		[Test]
		public void should_construct_builder_and_parse_basic_markup()
		{
			// Arrange
			IContainer container = Container;

			// Act
		    var builder = container.GetInstance<TextMiddlewareBuilder>();

            // Assert
            Assert.That(builder, Is.Not.Null);

		    string html = builder.Execute("**markdown**");
            Assert.That(html, Is.EqualTo("<p><strong>markdown</strong></p>\n")); // a basic smoke test of the middleware chain
        }

        [Test]
		public void should_register_types_with_instances()
		{
			// Arrange + Act + Assert
			AssertDefaultType<IPluginFactory, PluginFactory>();
            AssertDefaultType<IMarkupParser, MarkdigParser>();
            AssertDefaultType<IHtmlSanitizerFactory, HtmlSanitizerFactory>();
            AssertDefaultType<CustomTokenParser, CustomTokenParser>();
        }

        [Test]
        public void should_register_middleware_in_the_correct_order()
        {
            // Arrange
            IContainer container = Container;

            // Act
            var builder = container.GetInstance<TextMiddlewareBuilder>();

            // Assert
            Assert.That(builder, Is.Not.Null);
            Assert.That(builder.MiddlewareItems[0], Is.TypeOf<TextPluginBeforeParseMiddleware>());
            Assert.That(builder.MiddlewareItems[1], Is.TypeOf<MarkupParserMiddleware>());
            Assert.That(builder.MiddlewareItems[2], Is.TypeOf<HarmfulTagMiddleware>());
            Assert.That(builder.MiddlewareItems[3], Is.TypeOf<CustomTokenMiddleware>());
            Assert.That(builder.MiddlewareItems[4], Is.TypeOf<TextPluginAfterParseMiddleware>());
        }

		[Test]
		public void should_use_linktagprovider()
		{
			// Arrange

			// Act

			// Assert
		}

		[Test]
		public void should_use_imagetagprovider()
		{
			// Arrange

			// Act

			// Assert
		}

		[Test]
		public void should1()
		{
			// Arrange

			// Act

			// Assert
		}

		[Test]
		public void should2()
		{
			// Arrange

			// Act

			// Assert
		}

		[Test]
		[TestCase("http://i223.photobucket.com/albums/dd45/wally2603/91e7840f.jpg")]
		[TestCase("https://i223.photobucket.com/albums/dd45/wally2603/91e7840f.jpg")]
		public void x1_should_Not_Rewrite_Images_As_Internal_That_Start_With_Known_Prefixes(string imageUrl)
		{
			// Arrange
			IContainer container = Container;

			// Act
			var builder = container.GetInstance<TextMiddlewareBuilder>();

			// Assert
			Assert.That(builder, Is.Not.Null);

			string html = builder.Execute("![Image title](" + imageUrl + ")");
			// assert image was called/html
			Assert.That(html, Is.EqualTo("<p><strong>markdown</strong></p>\n"));
		}

		[Test]
		public void x1_should_remove_script_link_iframe_frameset_frame_applet_tags_from_text()
		{
			// Arrange
			string markdown = " some text <script type=\"text/html\">while(true)alert('lolz');</script>" +
				"<iframe src=\"google.com\"></iframe><frame>blah</frame> <applet code=\"MyApplet.class\" width=100 height=140></applet>" +
				"<frameset src='new.html'></frameset>";
			string expectedHtml = "<p>some text blah </p>\n";

			IContainer container = Container;
			var builder = container.GetInstance<TextMiddlewareBuilder>();

			// Act
			string actualHtml = builder.Execute(markdown);

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void x1_links_starting_with_hash_or_https_or_hash_are_not_rewritten_as_internal()
		{
			// Arrange
			string expectedHtml = "<p><a href=\"#myanchortag\">hello world</a> <a href=\"https://www.google.com/\" class=\"external-link\" rel=\"nofollow\">google</a></p>\n";
			string markdown = "[hello world](#myanchortag) [google](https://www.google.com)";

			IContainer container = Container;
			var builder = container.GetInstance<TextMiddlewareBuilder>();

			// Act
			string actualHtml = builder.Execute(markdown);

			// Assert
			Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}
	}
}