using GraphQL.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FysioAppUX.Models;
using GraphQL;
using FysioAppUX.Data.ResponseTypes;

namespace FysioAppUX.Data
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
            return response.Data.diagnoses;
        }

        public async Task<List<Diagnose>> GetSomeDiagnoses(int code)
        {
            var query = new GraphQLRequest
            {
                Query = @"query{ some(codeStart : "+code+@") {
                                id
                                code
                                lichaamslocalisatie
                                pathologie
                        }}"
            };
            var response = await _client.SendQueryAsync<ResponseDiagnoseCollectionType>(query);
            return response.Data.some;
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
            return response.Data.get;
        }
    }
}
