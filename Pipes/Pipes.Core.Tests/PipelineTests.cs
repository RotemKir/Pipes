using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pipes.Core.Runners;
using System;

namespace Pipes.Core.Tests
{
    [TestClass]
    public class PipelineTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Pipeline_Ctor_NameIsNull_Throws()
        {
            new Pipeline(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pipeline_Ctor_NameIsEmpty_Throws()
        {
            new Pipeline(string.Empty);
        }

        [TestMethod]
        public void Pipeline_Ctor_NameIsNotEmpty_SetsName()
        {
            const string name = "Pipeline";

            var pipeline = new Pipeline(name);

            pipeline.Name.Should().Be(name);
            pipeline.SegmentRunners.Should().BeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Pipeline_AddSegmentRunner_RunnerIsNull_Throws()
        {
            const string name = "Pipeline";
            var pipeline = new Pipeline(name);

            pipeline.AddSegmentRunner(null);
        }

        [TestMethod]
        public void Pipeline_AddSegmentRunner_RunnerIsNotNull_AddsRunner()
        {
            const string name = "Pipeline";
            var pipeline = new Pipeline(name);
            var runner = new Mock<ISegmentRunner>();

            pipeline.AddSegmentRunner(runner.Object);

            pipeline.SegmentRunners.Should().NotBeNull().And.BeEquivalentTo(new[] { runner.Object });
        }

        [TestMethod]
        public void Pipeline_AddSegmentRunner_AddSegmentRunners_AddsRunners()
        {
            const string name = "Pipeline";
            var pipeline = new Pipeline(name);
            var runner1 = new Mock<ISegmentRunner>();
            var runner2 = new Mock<ISegmentRunner>();

            pipeline.AddSegmentRunner(runner1.Object);
            pipeline.AddSegmentRunner(runner2.Object);

            pipeline.SegmentRunners.Should().NotBeNull()
                .And.BeEquivalentTo(new[] { runner1.Object, runner2.Object });
        }
    }
}
