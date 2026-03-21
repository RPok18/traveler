using NUnit.Framework;
using System;
using System.Collections.Generic;
using Traveler.Models;

namespace Traveler.Tests
{
    [TestFixture]
    public class TravelRequestValidationTests
    {
        /// <summary>Builds a known-good request; individual tests mutate one field at a time.</summary>
        private TravelRequest ValidRequest() => new TravelRequest
        {
            Origin = "Bangkok",
            Destination = "Tokyo",
            Budget = 2000m,
            Passengers = 2,
            DurationDays = 10,
            StartDate = DateTime.Today.AddMonths(1)
        };

        [Test]
        public void Validate_ValidRequest_IsValid()
        {
            var result = ValidRequest().Validate();
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public void Validate_EmptyOrigin_ReportsOriginError()
        {
            var req = ValidRequest();
            req.Origin = "";
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Origin"));
        }

        [Test]
        public void Validate_EmptyDestination_ReportsDestinationError()
        {
            var req = ValidRequest();
            req.Destination = "   ";
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Destination"));
        }

        [Test]
        public void Validate_SameOriginAndDestination_ReportsDifferError()
        {
            var req = ValidRequest();
            req.Destination = req.Origin;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("differ"));
        }

        [Test]
        public void Validate_SameOriginAndDestinationCaseInsensitive_ReportsDifferError()
        {
            var req = ValidRequest();
            req.Origin = "Bangkok";
            req.Destination = "BANGKOK";
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("differ"));
        }

        [Test]
        public void Validate_BudgetZero_ReportsBudgetError()
        {
            var req = ValidRequest();
            req.Budget = 0;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Budget"));
        }

        [Test]
        public void Validate_BudgetOverLimit_ReportsBudgetError()
        {
            var req = ValidRequest();
            req.Budget = 1_000_001m;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Budget"));
        }

        [Test]
        public void Validate_PassengersZero_ReportsPassengersError()
        {
            var req = ValidRequest();
            req.Passengers = 0;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Passengers"));
        }

        [Test]
        public void Validate_PassengersOver20_ReportsPassengersError()
        {
            var req = ValidRequest();
            req.Passengers = 21;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Passengers"));
        }

        [Test]
        public void Validate_DurationZero_ReportsDurationError()
        {
            var req = ValidRequest();
            req.DurationDays = 0;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Duration"));
        }

        [Test]
        public void Validate_DurationOver365_ReportsDurationError()
        {
            var req = ValidRequest();
            req.DurationDays = 366;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("Duration"));
        }

        [Test]
        public void Validate_PastStartDate_ReportsDateError()
        {
            var req = ValidRequest();
            req.StartDate = new DateTime(2000, 1, 1);
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Some.Contains("date"));
        }

        [Test]
        public void Validate_NullStartDate_IsAllowed()
        {
            var req = ValidRequest();
            req.StartDate = null;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Validate_MultipleErrors_AllReported()
        {
            var req = new TravelRequest
            {
                Origin = "",
                Destination = "",
                Budget = 0,
                Passengers = 0,
                DurationDays = 0
            };
            var result = req.Validate();

            Assert.That(result.IsValid, Is.False);
            // Expect at least: Origin, Destination, Budget, Passengers, Duration errors
            Assert.That(result.Errors.Count, Is.GreaterThanOrEqualTo(5));
        }

        [Test]
        public void Validate_TodayStartDate_IsAllowed()
        {
            var req = ValidRequest();
            req.StartDate = DateTime.Today;
            var result = req.Validate();

            Assert.That(result.IsValid, Is.True);
        }
    }
}
