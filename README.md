# JsonHttp
## About
This is a simple library to quickly turn JSON responses from API endpoints into useable classes.

##Compatibility
This NuGet package is built for: .NET Framework 4.6+, Windows (Phone) 8, Windows (Phone) 8.1, Windows Phone 8.1 Silverlight, Windows 10

##Features
* Get
* Post
* Put
* Delete
* Autoredirect
* Headers
* Set request as type of JSON

##Examples
###Getting quick results
You only need to use one line of code to do a basic GET request
```
WordOfTheDay wotd = await JsonHttp.Get<WordOfTheDay>(new Uri("http://urban-word-of-the-day.herokuapp.com/today"));
```

###More advanced
You can also add some extra options to the request
```
JsonHttp.Options options = new JsonHttp.Options()
{
    AllowAutoRedirect = true,
    DefaultRequestHeaders = new Dictionary<string, string>(),
    AddMediaTypeWithQualityHeadersJson = true,
    UseLocationHeaderForRedirects = true
};
WordOfTheDay wotd = await JsonHttp.Get<WordOfTheDay>(new Uri("http://urban-word-of-the-day.herokuapp.com/today"), options); 
```

##More info
JsonHttp was made by Glenn Ruysschaert.

It makes use of the HttpClient libraries provided by Microsoft and JSON.Net by James Newton-King

Find more info and other projects on my site [www.glennruysschaert.com](www.glennruysschaert.com)
