using Beamasp.PostcodesIO.Exceptions;
using Beamasp.PostcodesIO.Internals;
using Beamasp.PostcodesIO.Results;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Beamasp.PostcodesIO
{
    public class PostcodesIOClient : IPostcodesIOClient
    {
        private readonly string _endpoint;
        private readonly string _proxyServerUrl;

        public PostcodesIOClient(string endpoint = "https://api.postcodes.io", string proxyServerUrl = null)
        {
            _endpoint = endpoint;
            _proxyServerUrl = proxyServerUrl;
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            RestClient client = new RestClient { BaseUrl = new Uri(_endpoint) };

            if (!string.IsNullOrEmpty(_proxyServerUrl))
            {
                client.Proxy = new WebProxy(_proxyServerUrl, true)
                {
                    Credentials = CredentialCache.DefaultCredentials
                };
            }

            IRestResponse<RawResult<T>> response = client.Execute<RawResult<T>>(request);

            if (response.ErrorException != null)
            {
                throw new PostcodesIOApiException(response.ErrorException);

                /* Unmerged change from project 'Beamasp.PostcodesIO (net452)'
                Before:
                            if (response.Data == null) 
                After:
                            if (response.Data == null)
                */
            }

            return response.Data == null ? throw new PostcodesIOEmptyResponseException(response.StatusCode) : response.Data.Result;
        }

        private async Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            RestClient client = new RestClient { BaseUrl = new Uri(_endpoint) };

            if (!string.IsNullOrEmpty(_proxyServerUrl))
            {
                client.Proxy = new WebProxy(_proxyServerUrl, true)
                {
                    Credentials = CredentialCache.DefaultCredentials
                };
            }

            IRestResponse<RawResult<T>> response = await client.ExecuteTaskAsync<RawResult<T>>(request).ConfigureAwait(false);

            if (response.ErrorException != null)
            {
                throw new PostcodesIOApiException(response.ErrorException);
            }

            return response.Data == null ? throw new PostcodesIOEmptyResponseException(response.StatusCode) : response.Data.Result;
        }

        public PostcodeResult Lookup(string postcode)
        {
            RestRequest request = CreateLookupRequest(postcode);
            return Execute<PostcodeResult>(request);
        }

        public OutwardCodeResult OutwardCodeLookup(string outcode)
        {
            RestRequest request = CreateOutwardCodeLookupRequest(outcode);
            return Execute<OutwardCodeResult>(request);
        }

        public Task<PostcodeResult> LookupAsync(string postcode)
        {
            RestRequest request = CreateLookupRequest(postcode);
            return ExecuteAsync<PostcodeResult>(request);
        }

        public IEnumerable<BulkQueryResult<string, PostcodeResult>> BulkLookup(IEnumerable<string> postcodes)
        {
            RestRequest request = CreateBulkLookupRequest(postcodes);
            return Execute<List<BulkQueryResult<string, PostcodeResult>>>(request);
        }

        public Task<IEnumerable<BulkQueryResult<string, PostcodeResult>>> BulkLookupAsync(IEnumerable<string> postcodes)
        {
            RestRequest request = CreateBulkLookupRequest(postcodes);
            return ExecuteAsync<List<BulkQueryResult<string, PostcodeResult>>>(request).ContinueWith(t => t.Result as IEnumerable<BulkQueryResult<string, PostcodeResult>>, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public IEnumerable<PostcodeResult> Query(string q, int? limit = null)
        {
            RestRequest request = CreateQueryRequest(q, limit);
            return Execute<List<PostcodeResult>>(request);
        }

        public Task<IEnumerable<PostcodeResult>> QueryAsync(string q, int? limit = null)
        {
            RestRequest request = CreateQueryRequest(q, limit);
            return ExecuteAsync<List<PostcodeResult>>(request).ContinueWith(t => t.Result as IEnumerable<PostcodeResult>, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public bool Validate(string postcode)
        {
            RestRequest request = CreateValidateRequest(postcode);
            return Execute<bool>(request);
        }

        public Task<bool> ValidateAsync(string postcode)
        {
            RestRequest request = CreateValidateRequest(postcode);
            return ExecuteAsync<bool>(request);
        }

        public IEnumerable<PostcodeResult> LookupLatLon(ReverseGeocodeQuery query)
        {
            RestRequest request = CreateLookupLocationRequest(query);
            return Execute<List<PostcodeResult>>(request);
        }

        public Task<IEnumerable<PostcodeResult>> LookupLatLonAsync(ReverseGeocodeQuery query)
        {
            RestRequest request = CreateLookupLocationRequest(query);
            return ExecuteAsync<List<PostcodeResult>>(request).ContinueWith(t => t.Result as IEnumerable<PostcodeResult>, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public IEnumerable<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>> BulkLookupLatLon(IEnumerable<ReverseGeocodeQuery> queries)
        {
            RestRequest request = CreateBulkLookupLatLon(queries);
            return Execute<List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>>>(request);
        }

        public Task<IEnumerable<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>>> BulkLookupLatLonAsync(IEnumerable<ReverseGeocodeQuery> queries)
        {
            RestRequest request = CreateBulkLookupLatLon(queries);
            return ExecuteAsync<List<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>>>(request).ContinueWith(t => t.Result as IEnumerable<BulkQueryResult<ReverseGeocodeQuery, List<PostcodeResult>>>, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public IEnumerable<string> Autocomplete(string postcode, int? limit = null)
        {
            RestRequest request = CreateAutocompleteRequest(postcode, limit);
            return Execute<List<string>>(request);
        }

        public Task<IEnumerable<string>> AutocompleteAsync(string postcode, int? limit = null)
        {
            RestRequest request = CreateAutocompleteRequest(postcode, limit);
            return ExecuteAsync<List<string>>(request).ContinueWith(t => t.Result as IEnumerable<string>, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public PostcodeResult Random()
        {
            RestRequest request = CreateRandomRequest();
            return Execute<PostcodeResult>(request);
        }

        public Task<PostcodeResult> RandomAsync()
        {
            RestRequest request = CreateRandomRequest();
            return ExecuteAsync<PostcodeResult>(request);
        }

        public IEnumerable<NearestResult> Nearest(string postcode, int? limit = null, int? radius = null)
        {
            RestRequest request = CreateNearest(postcode, limit, radius);
            return Execute<List<NearestResult>>(request);
        }

        public Task<IEnumerable<NearestResult>> NearestAsync(string postcode, int? limit = null, int? radius = null)
        {
            RestRequest request = CreateNearest(postcode, limit, radius);
            return ExecuteAsync<List<NearestResult>>(request).ContinueWith(t => t.Result as IEnumerable<NearestResult>, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public TerminatedPostcodeResult Terminated(string postcode)
        {
            RestRequest request = CreateTerminatedRequest(postcode);
            return Execute<TerminatedPostcodeResult>(request);
        }

        public Task<TerminatedPostcodeResult> TerminatedAsync(string postcode)
        {
            RestRequest request = CreateTerminatedRequest(postcode);
            return ExecuteAsync<TerminatedPostcodeResult>(request);
        }

        private static RestRequest CreateBulkLookupRequest(IEnumerable<string> postcodes)
        {
            RestRequest request = new RestRequest("postcodes", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            _ = request.AddJsonBody(new { postcodes });
            return request;
        }

        private static RestRequest CreateQueryRequest(string q, int? limit)
        {
            RestRequest request = new RestRequest("postcodes", Method.GET);
            _ = request.AddQueryParameter("q", q);
            if (limit.HasValue)
            {
                _ = request.AddParameter("limit", limit);
            }

            return request;
        }

        private static RestRequest CreateValidateRequest(string postcode)
        {
            RestRequest request = new RestRequest(string.Format("postcodes/{0}/validate", postcode), Method.GET);
            return request;
        }

        private static RestRequest CreateLookupLocationRequest(ReverseGeocodeQuery query)
        {
            RestRequest request = new RestRequest("postcodes", Method.GET);
            _ = request.AddParameter("lat", query.Latitude);
            _ = request.AddParameter("lon", query.Longitude);
            if (query.Limit.HasValue)
            {
                _ = request.AddParameter("limit", query.Limit);
            }

            return request;
        }

        private static RestRequest CreateLookupRequest(string postcode)
        {
            return new RestRequest(string.Format("postcodes/{0}", postcode), Method.GET);
        }

        private static RestRequest CreateOutwardCodeLookupRequest(string outcode)
        {
            return new RestRequest(string.Format("outcodes/{0}", outcode), Method.GET);
        }

        private static RestRequest CreateBulkLookupLatLon(IEnumerable<ReverseGeocodeQuery> queries)
        {
            RestRequest request = new RestRequest("postcodes", Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new JsonDotNetSerializer()
            };
            _ = request.AddJsonBody(new { geolocations = queries });
            return request;
        }

        private static RestRequest CreateAutocompleteRequest(string postcode, int? limit)
        {
            RestRequest request = new RestRequest(string.Format("postcodes/{0}/autocomplete", postcode), Method.GET);
            if (limit.HasValue)
            {
                _ = request.AddParameter("limit", limit);
            }

            return request;
        }

        private static RestRequest CreateRandomRequest()
        {
            RestRequest request = new RestRequest("random/postcodes", Method.GET);
            return request;
        }

        private static RestRequest CreateNearest(string postcode, int? limit, int? radius)
        {
            RestRequest request = new RestRequest(string.Format("postcodes/{0}/nearest", postcode), Method.GET);
            if (limit.HasValue)
            {
                _ = request.AddParameter("limit", limit);
            }

            if (radius.HasValue)
            {
                _ = request.AddParameter("radius", radius);
            }

            return request;
        }

        private static RestRequest CreateTerminatedRequest(string postcode)
        {
            return new RestRequest(string.Format("terminated_postcodes/{0}", postcode), Method.GET);
        }
    }
}