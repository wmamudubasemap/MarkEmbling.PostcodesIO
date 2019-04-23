using MarkEmbling.PostcodesIO.Internals;
using MarkEmbling.PostcodesIO.Resources;
using MarkEmbling.PostcodesIO.Results;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkEmbling.PostcodesIO.Tests.Integration
{
    [TestFixture, Explicit("Hits live Postcodes.io API")]
    public class PostcodesBulkReverseGeocodeTests {
        private PostcodesResource _postcodes;
        private ReverseGeocodeQuery[] _lookups;
            
        [SetUp]
        public void Setup() {
            _postcodes = new PostcodesResource(new RequestExecutor("https://api.postcodes.io"));
            _lookups = new[] {
                new ReverseGeocodeQuery {Latitude = 51.2452924089757, Longitude = -0.58231794275613},
                new ReverseGeocodeQuery {Latitude = 51.2571984465953, Longitude = -0.567549033067429}
            };
        }

        [Test]
        public void BulkReverseGeocode_returns_results() {
            var results = _postcodes.BulkReverseGeocode(_lookups);
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public void BulkReverseGeocode_results_contain_original_queries() {
            var results = _postcodes.BulkReverseGeocode(_lookups).ToList();

            Assert.True(results.Any(x => x.Query.Equals(_lookups[0])));
            Assert.True(results.Any(x => x.Query.Equals(_lookups[1])));
        }

        [Test]
        public void BulkReverseGeocode_results_contain_postcode_results() {
            var results = _postcodes.BulkReverseGeocode(_lookups).ToList();

            AssertPostcodeResults(results);
        }

        [Test]
        public async Task BulkReverseGeocodeAsync_returns_results()
        {
            var results = await _postcodes.BulkReverseGeocodeAsync(_lookups);
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public async Task BulkReverseGeocodeAsync_results_contain_original_queries()
        {
            var results = (await _postcodes.BulkReverseGeocodeAsync(_lookups)).ToList();

            Assert.True(results.Any(x => x.Query.Equals(_lookups[0])));
            Assert.True(results.Any(x => x.Query.Equals(_lookups[1])));
        }

        [Test]
        public async Task BulkReverseGeocodeAsync_results_contain_postcode_results()
        {
            var results = (await _postcodes.BulkReverseGeocodeAsync(_lookups)).ToList();

            AssertPostcodeResults(results);
        }

        private void AssertPostcodeResults(List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results)
        {
            Assert.True(results[0].Result.Any());
            Assert.True(results[1].Result.Any());
            Assert.True(results.Single(r => r.Query.Equals(_lookups[0])).Result.Exists(p => p.Postcode == "GU1 1AA"));
        }
    }
}