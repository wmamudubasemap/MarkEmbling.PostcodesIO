using Beamasp.PostcodesIO;
using Beamasp.PostcodesIO.Results;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integration
{
    [TestFixture, Explicit("Hits live Postcodes.io API")]
    public class BulkLookupLatLonTests
    {
        private PostcodesIOClient _client;
        private ReverseGeocodeQuery[] _lookups;

        [SetUp]
        public void Setup()
        {
            _client = new PostcodesIOClient();
            _lookups = new[] {
                new ReverseGeocodeQuery {Latitude = 51.2452924089757, Longitude = -0.58231794275613},
                new ReverseGeocodeQuery {Latitude = 51.2571984465953, Longitude = -0.567549033067429}
            };
        }

        [Test]
        public void BulkLookupLatLon_returns_results()
        {
            IEnumerable<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results = _client.BulkLookupLatLon(_lookups);
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public void BulkLookupLatLon_results_contain_original_queries()
        {
            List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results = _client.BulkLookupLatLon(_lookups).ToList();

            Assert.True(results.Any(x => x.Query.Equals(_lookups[0])));
            Assert.True(results.Any(x => x.Query.Equals(_lookups[1])));
        }

        [Test]
        public void BulkLookupLatLon_results_contain_postcode_results()
        {
            List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results = _client.BulkLookupLatLon(_lookups).ToList();

            TestBulkLookupLatLon_results_contain_postcode_results(results);
        }

        [Test]
        public async Task BulkLookupLatLon_returns_results_async()
        {
            IEnumerable<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results = await _client.BulkLookupLatLonAsync(_lookups);
            Assert.AreEqual(2, results.Count());
        }

        [Test]
        public async Task BulkLookupLatLon_results_contain_original_queries_async()
        {
            List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results = (await _client.BulkLookupLatLonAsync(_lookups)).ToList();

            Assert.True(results.Any(x => x.Query.Equals(_lookups[0])));
            Assert.True(results.Any(x => x.Query.Equals(_lookups[1])));
        }

        [Test]
        public async Task BulkLookupLatLon_results_contain_postcode_results_async()
        {
            List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results = (await _client.BulkLookupLatLonAsync(_lookups)).ToList();

            TestBulkLookupLatLon_results_contain_postcode_results(results);
        }

        private void TestBulkLookupLatLon_results_contain_postcode_results(List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> results)
        {
            Assert.True(results[0].Result.Any());
            Assert.True(results[1].Result.Any());
            Assert.True(results.Single(r => r.Query.Equals(_lookups[0])).Result.Exists(p => p.Postcode == "GU1 1AA"));
        }
    }
}