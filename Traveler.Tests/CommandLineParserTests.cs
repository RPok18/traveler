using NUnit.Framework;
using System;
using System.Collections.Generic;
using Traveler;
using Traveler.Models;

namespace Traveler.Tests
{
    [TestFixture]
    public class CommandLineParserTests
    {
        [Test]
        public void Parse_AllRequiredFlags_PopulatesCorrectly()
        {
            var args = new[] { "plan", "--from", "Bangkok", "--to", "Tokyo", "--budget", "2000", "--days", "10" };
            var req = CommandLineParser.Parse(args);

            Assert.That(req.Origin, Is.EqualTo("Bangkok"));
            Assert.That(req.Destination, Is.EqualTo("Tokyo"));
            Assert.That(req.Budget, Is.EqualTo(2000m));
            Assert.That(req.DurationDays, Is.EqualTo(10));
        }

        [Test]
        public void Parse_Passengers_PopulatesCorrectly()
        {
            var args = new[] { "plan", "--from", "A", "--to", "B", "--budget", "500", "--days", "3", "--passengers", "4" };
            var req = CommandLineParser.Parse(args);

            Assert.That(req.Passengers, Is.EqualTo(4));
        }

        [Test]
        public void Parse_DateFlag_ParsesCorrectly()
        {
            var args = new[] { "plan", "--from", "A", "--to", "B", "--budget", "500", "--days", "3", "--date", "2099-12-31" };
            var req = CommandLineParser.Parse(args);

            Assert.That(req.StartDate, Is.Not.Null);
            Assert.That(req.StartDate!.Value.Year, Is.EqualTo(2099));
            Assert.That(req.StartDate.Value.Month, Is.EqualTo(12));
            Assert.That(req.StartDate.Value.Day, Is.EqualTo(31));
        }

        [Test]
        public void Parse_PreferencesFlag_SplitsIntoList()
        {
            var args = new[] { "plan", "--from", "A", "--to", "B", "--budget", "500", "--days", "3", "--preferences", "beach,culture,food" };
            var req = CommandLineParser.Parse(args);

            Assert.That(req.Preferences, Has.Count.EqualTo(3));
            Assert.That(req.Preferences, Contains.Item("beach"));
            Assert.That(req.Preferences, Contains.Item("culture"));
            Assert.That(req.Preferences, Contains.Item("food"));
        }

        [Test]
        public void Parse_ViaFlag_PopulatesCorrectly()
        {
            var args = new[] { "plan", "--from", "A", "--to", "B", "--budget", "500", "--days", "3", "--via", "Singapore" };
            var req = CommandLineParser.Parse(args);

            Assert.That(req.Via, Is.EqualTo("Singapore"));
        }

        [Test]
        public void Parse_MissingOptionalArgs_DoesNotThrow()
        {
            var args = new[] { "plan", "--from", "X", "--to", "Y" };
            Assert.DoesNotThrow(() => CommandLineParser.Parse(args));
        }

        [Test]
        public void Parse_EmptyArgs_ReturnsDefaultRequest()
        {
            var req = CommandLineParser.Parse(Array.Empty<string>());

            Assert.That(req.Origin, Is.EqualTo(string.Empty));
            Assert.That(req.Destination, Is.EqualTo(string.Empty));
            Assert.That(req.Budget, Is.EqualTo(0m));
            Assert.That(req.Passengers, Is.EqualTo(1)); // default
        }

        [Test]
        public void Parse_PreferencesWithSpaces_TrimsEachEntry()
        {
            var args = new[] { "plan", "--from", "A", "--to", "B", "--budget", "500", "--days", "3", "--preferences", " beach , food " };
            var req = CommandLineParser.Parse(args);

            Assert.That(req.Preferences, Contains.Item("beach"));
            Assert.That(req.Preferences, Contains.Item("food"));
        }
    }
}
