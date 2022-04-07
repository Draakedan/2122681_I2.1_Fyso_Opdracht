using GraphQL.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Models;
using GraphQL;
using InfrastructureAPIHandler.Data.ResponseTypes;

namespace InfrastructureAPIHandler.Data
{
    public class OwnerConsumer
    {
        private readonly IGraphQLClient _client;

        public OwnerConsumer(IGraphQLClient client)
        {
            _client = client;
        }

        public async Task<List<Diagnose>> GetAllDiagnoses()
        {
            var query = new GraphQLRequest
            {
                Query = @"query{ diagnoses {
                                id
                                code
                                lichaamslocalisatie
                                pathologie
                                }}"
            };

            var response = await _client.SendQueryAsync<ResponseDiagnoseCollectionType>(query);
            Console.WriteLine(response.Data);
            return response.Data.Diagnoses;
        }

        public async Task<Diagnose> GetOneDiagnose(int code)
        {
            var query = new GraphQLRequest
            {
                Query = @"query{ get(code: " + code + @") {
                                    id
                                    code
                                    lichaamslocalisatie
                                    pathologie
                           }}"
            };
            var response = await _client.SendQueryAsync<ResponseDiagnoseType>(query);
            Console.WriteLine(response.Data);
            return response.Data.Get;
        }
    }
}
