# Combines HttpClient and serialization
System.Net.Json is a combined HTTP client with embedded serialization. 

JsonClient - the main artifact of System.Net.Json provides GET, POST, PUT, PATCH and DELETE requests of strongly typed objects, via the .NET HttpClient. This enables quicker time for integration rather than plumming.

The latest version 5.0.8 is recoded from a static client to be encaptulting iHttpClientFactory or HttpClient.

## Change log
5.0.8 - Moved to HttpClient. JsonClient is not longer a static class. iApiClient and JsonApiClient is removed from System.Net.Json.API.
