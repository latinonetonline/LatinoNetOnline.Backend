using GraphQL;
using GraphQL.Client.Http;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Modules.Events.Core.Managers
{
    interface IGraphQLManager
    {
        Task<T> ExceuteMutationAsync<T>(Uri endpoint, string graphQLQueryType, string completeQueryString, object variables, string accessToken);
        Task<T> ExceuteQueryAsync<T>(Uri endpoint, string graphQLQueryType, string completeQueryString, object variables, string accessToken);
        Task<IEnumerable<T>> ExceuteQueryReturnListAsync<T>(Uri endpoint, string graphQLQueryType, string completeQueryString, object variables, string accessToken);
    }

    class GraphQLManager : IGraphQLManager
    {
        protected readonly GraphQLHttpClient graphQLHttpClient;

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };


        public GraphQLManager()
        {
            var graphQLOptions = new GraphQLHttpClientOptions
            {

            };

            graphQLHttpClient = new GraphQLHttpClient(graphQLOptions, new GraphQL.Client.Serializer.SystemTextJson.SystemTextJsonSerializer());
        }

        public async Task<T> ExceuteQueryAsync<T>(Uri endpoint, string graphQLQueryType, string completeQueryString, object variables, string accessToken)
        {
            try
            {
                graphQLHttpClient.Options.EndPoint = endpoint;
                graphQLHttpClient.HttpClient.DefaultRequestHeaders.Remove("Authorization");

                if (!string.IsNullOrWhiteSpace(accessToken))
                    graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {accessToken}");

                var request = new GraphQLRequest
                {
                    Query = completeQueryString
                };

                if (variables is not null)
                {
                    request.Variables = variables;
                }

                var response = await graphQLHttpClient.SendQueryAsync<object>(request);

                var stringResult = response.Data.ToString();
                stringResult = stringResult!.Replace($"\"{graphQLQueryType}\":", string.Empty);
                stringResult = stringResult.Remove(0, 1);
                stringResult = stringResult.Remove(stringResult.Length - 1, 1);

                var result = JsonSerializer.Deserialize<T>(stringResult, _jsonOptions);

                return result!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> ExceuteQueryReturnListAsync<T>(Uri endpoint, string graphQLQueryType, string completeQueryString, object variables, string accessToken)
        {
            try
            {
                graphQLHttpClient.Options.EndPoint = endpoint;
                graphQLHttpClient.HttpClient.DefaultRequestHeaders.Remove("Authorization");

                if (!string.IsNullOrWhiteSpace(accessToken))
                    graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {accessToken}");

                var request = new GraphQLRequest
                {
                    Query = completeQueryString
                };

                if (variables is not null)
                {
                    request.Variables = variables;
                }

                var response = await graphQLHttpClient.SendQueryAsync<object>(request);

                var stringResult = response.Data.ToString();
                stringResult = stringResult!.Replace($"\"{graphQLQueryType}\":", string.Empty);
                stringResult = stringResult.Remove(0, 1);
                stringResult = stringResult.Remove(stringResult.Length - 1, 1);

                var result = JsonSerializer.Deserialize<List<T>>(stringResult, _jsonOptions);

                return result!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> ExceuteMutationAsync<T>(Uri endpoint, string graphQLQueryType, string completeQueryString, object variables, string accessToken)
        {
            try
            {
                graphQLHttpClient.Options.EndPoint = endpoint;
                graphQLHttpClient.HttpClient.DefaultRequestHeaders.Remove("Authorization");

                if (!string.IsNullOrWhiteSpace(accessToken))
                    graphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {accessToken}");

                var request = new GraphQLRequest
                {
                    Query = completeQueryString
                };

                if (variables is not null)
                {
                    request.Variables = variables;
                }

                var response = await graphQLHttpClient.SendMutationAsync<object>(request);

                var stringResult = response.Data.ToString();
                stringResult = stringResult!.Replace($"\"{graphQLQueryType}\":", string.Empty);
                stringResult = stringResult.Remove(0, 1);
                stringResult = stringResult.Remove(stringResult.Length - 1, 1);

                var result = JsonSerializer.Deserialize<T>(stringResult, _jsonOptions);

                return result!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
