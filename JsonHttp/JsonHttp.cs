using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class JsonHttp
{
    private enum RequestType
    {
        GET, POST, PUT, DELETE
    }

    public class Options
    {
        public bool AllowAutoRedirect { get; set; }
        public bool AddMediaTypeWithQualityHeadersJson { get; set; }
        public Dictionary<string, string> DefaultRequestHeaders { get; set; }
        public bool UseLocationHeaderForRedirects { get; set; }
    }

    private static HttpClient CreateClient(Options options = null)
    {
        HttpClientHandler handler = new HttpClientHandler();
        HttpClient httpClient = new HttpClient(handler);

        if (options != null)
        {
            handler.AllowAutoRedirect = options.AllowAutoRedirect;
            if (options.AddMediaTypeWithQualityHeadersJson) 
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            if (options.DefaultRequestHeaders != null)
            {
                foreach (KeyValuePair<string, string> entry in options.DefaultRequestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
            }

        }
        return httpClient;
    }

    private static async Task<T> ProcessResponse<T>(HttpResponseMessage response, RequestType requestType, Options options, HttpContent content)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        if (response.StatusCode == HttpStatusCode.RedirectKeepVerb)
        {
            if (options == null) throw  new Exception("Redirect detected, enable UseLocationHeaderForRedirects to enable automatic redirects.");
            if (options.UseLocationHeaderForRedirects)
            {
                try
                {
                    switch (requestType)
                    {
                        case RequestType.GET:
                        return await Get<T>(response.Headers.Location, options);

                    case RequestType.POST:
                        return await Post<T>(response.Headers.Location, content, options);

                    case RequestType.PUT:
                        return await Put<T>(response.Headers.Location, content, options);

                    case RequestType.DELETE:
                        return await Delete<T>(response.Headers.Location, options);

                    default:
                            throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Redirect detected, but failed: " + ex.Message);
                }
            }

            throw new Exception("Redirect detected, enable UseLocationHeaderForRedirects to enable automatic redirects. (2)");
        }

        if (response.IsSuccessStatusCode)
        {
            string contentResult = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T>(contentResult);
            return obj;
        }
        throw new Exception("Couldn't process the response");
    }

    public static async Task<T> Get<T>(Uri uri, Options options = null)
    {
        try
        {
            HttpClient httpClient = CreateClient(options);
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return await ProcessResponse<T>(response, RequestType.GET, options, null);
        }
        catch (Exception ex)
        {
            throw new Exception("An unkown error occured: " + ex.Message);
        }
    }

    public static async Task<T> Delete<T>(Uri uri, Options options = null)
    {
        try
        {
            HttpClient httpClient = CreateClient(options);
            HttpResponseMessage response = await httpClient.DeleteAsync(uri);
            return await ProcessResponse<T>(response, RequestType.DELETE, options, null);
        }
        catch (Exception ex)
        {
            throw new Exception("An unkown error occured: " + ex.Message);
        }
    }

    public static async Task<T> Post<T>(Uri uri, HttpContent content, Options options = null)
    {
        try
        {
            HttpClient httpClient = CreateClient(options);
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            return await ProcessResponse<T>(response, RequestType.POST, options, content);
        }
        catch (Exception ex)
        {
            throw new Exception("An unkown error occured: " + ex.Message);
        }
    }

    public static async Task<T> Put<T>(Uri uri, HttpContent content, Options options = null)
    {
        try
        {
            HttpClient httpClient = CreateClient(options);
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);
            return await ProcessResponse<T>(response, RequestType.PUT, options, content);
        }
        catch (Exception ex)
        {
            throw new Exception("An unkown error occured: " + ex.Message);
        }
    }
}

