﻿using NUnit.Framework;
using System.Threading.Tasks;

namespace Beamasp.PostcodesIO.Tests.Integration
{
    [TestFixture, Explicit("Hits live Postcodes.io API")]
    public class ValidateTests {
        private PostcodesIOClient _client;

        [SetUp]
        public void Setup() {
            _client = new PostcodesIOClient();
        }

        [Test]
        public void Validate_returns_true_for_valid_postcode() {
            var result = _client.Validate("GU1 1AA");
            Assert.True(result);
        }

        [Test]
        public void Validate_returns_false_for_nonsense_postcode() {
            var result = _client.Validate("FAKE_POSTCODE");
            Assert.False(result);
        }

        [Test]
        public async Task Validate_returns_true_for_valid_postcode_async()
        {
            var result = await _client.ValidateAsync("GU1 1AA");
            Assert.True(result);
        }

        [Test]
        public async Task Validate_returns_false_for_nonsense_postcode_async()
        {
            var result = await _client.ValidateAsync("FAKE_POSTCODE");
            Assert.False(result);
        }
    }
}