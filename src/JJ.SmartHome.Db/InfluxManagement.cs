using System.Linq;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JJ.SmartHome.Db
{
    public class InfluxManagement
    {
        private const int _expirationDays = 7;
        private readonly InfluxDbOptions _options;
        public InfluxManagement(IOptions<InfluxDbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> Setup()
        {
            using (var influxDBClient = GetInfluxDBClient())
            {
                var organization = await EnsureOrganizationCreated(influxDBClient);

                var bucket = await EnsureBucketCreated(influxDBClient, organization);
                
                var authorization = await CreateAuthorizationToken(influxDBClient, bucket);

                return authorization.Token;
            }
        }

        private async Task<Authorization> CreateAuthorizationToken(InfluxDBClient influxDBClient, Bucket bucket)
        {
            var resource = new PermissionResource { Id = bucket.Id, OrgID = bucket.OrgID, Type = PermissionResource.TypeEnum.Buckets };

            var read = new Permission(Permission.ActionEnum.Read, resource);
            var write = new Permission(Permission.ActionEnum.Write, resource);

            var authorizations = await influxDBClient.GetAuthorizationsApi().FindAuthorizationsAsync();
            var authorization = authorizations.FirstOrDefault(a => a.OrgID == bucket.OrgID && a.User == _options.User);
            if (authorization == null)
            {
                authorization = await influxDBClient.GetAuthorizationsApi()
                    .CreateAuthorizationAsync(new Authorization(
                        bucket.OrgID,
                        new List<Permission> { read, write }
                    ));
            }

            return authorization;
        }

        private async Task<Bucket> EnsureBucketCreated(InfluxDBClient influxDBClient, Organization organization)
        {
            var bucket = await influxDBClient.GetBucketsApi().FindBucketByNameAsync(_options.Bucket);
            if (bucket == null)
            {
                var retention = new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, 60 * 60 * 24 * _expirationDays);
                bucket = await influxDBClient.GetBucketsApi().CreateBucketAsync(_options.Bucket, retention, organization.Id);
            }

            return bucket;
        }

        private async Task<Organization> EnsureOrganizationCreated(InfluxDBClient influxDBClient)
        {
            var orgs = await influxDBClient.GetOrganizationsApi().FindOrganizationsAsync();
            
            var organization = orgs.FirstOrDefault(r => r.Name == _options.Organization);
            if (organization == null)
            {
                organization = await influxDBClient.GetOrganizationsApi().CreateOrganizationAsync(_options.Organization);
            }

            return organization;
        }

        private InfluxDBClient GetInfluxDBClient()
        {
            return InfluxDBClientFactory.Create(
                        InfluxDBClientOptions.Builder.CreateNew()
                            .Url(_options.Uri)
                            .Authenticate(_options.User, _options.Password.ToCharArray())
                            .Build()
                        );
        }
    }
}