using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class PipelineContextTests
    {
        [TestMethod]
        public void PipelineContext_MaxVersion_DefaultIsMaxInt()
        {
            var context = new PipelineContext();

            context.MaxVersion.Should().Be(int.MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipelineContext_GetValue_NameIsNull_Throws()
        {
            var context = new PipelineContext();

            context.GetValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PipelineContext_GetValue_NameIsEmpty_Throws()
        {
            var context = new PipelineContext();

            context.GetValue(string.Empty);
        }

        [TestMethod]
        public void PipelineContext_SetValue_DataIsNull_DoesNothing()
        {
            var context = new PipelineContext();

            context.SetValue(null);
        }

        [TestMethod]
        public void PipelineContext_SetValue_DataIsRegularObject_DoesNothing()
        {
            var data = new object();
            var context = new PipelineContext();

            context.SetValue(data);
        }

        [TestMethod]
        public void PipelineContext_SetValue_DataIsAnonymousObject_AddsPropertiesToContext()
        {
            var data = new { a = 1, b = true, c = "Hello" };
            var context = new PipelineContext();

            context.SetValue(data);

            context.GetValue("a").Should().Be(data.a);
            context.GetValue("b").Should().Be(data.b);
            context.GetValue("c").Should().Be(data.c);
        }

        [TestMethod]
        public void PipelineContext_GetValue_NameInUpperCase_ReturnsValue()
        {
            var data = new { value = "Hello" };
            var context = new PipelineContext();

            context.SetValue(data);

            context.GetValue("VALUE").Should().Be(data.value);
        }

        [TestMethod]
        public void PipelineContext_GetValue_NameInLowerCase_ReturnsValue()
        {
            var data = new { value = "Hello" };
            var context = new PipelineContext();

            context.SetValue(data);

            context.GetValue("value").Should().Be(data.value);
        }

        [TestMethod]
        public void PipelineContext_GetValue_NameInPascalCase_ReturnsValue()
        {
            var data = new { value = "Hello" };
            var context = new PipelineContext();

            context.SetValue(data);

            context.GetValue("Value").Should().Be(data.value);
        }

        [TestMethod]
        public void PipelineContext_GetValue_NameInMixedCase_ReturnsValue()
        {
            var data = new { value = "Hello" };
            var context = new PipelineContext();

            context.SetValue(data);

            context.GetValue("VaLuE").Should().Be(data.value);
        }
    }
}
