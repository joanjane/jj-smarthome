using InfluxDB.Client;
using Microsoft.Extensions.Options;

namespace JJ.SmartHome.Db
{
    public class InfluxDBClientProvider
    {
        private readonly InfluxDbOptions _options;

        public InfluxDBClientProvider(IOptions<InfluxDbOptions> options)
        {
            _options = options.Value;
        }

        public InfluxDBClient Get()
        {
            if (_options.UseV1)
            {
                return InfluxDBClientFactory.CreateV1(
                    _options.Uri,
                    _options.User,
                    _options.Password.ToCharArray(),
                    _options.Bucket,
                    _options.RetentionPolicy
                );
            }
            else
            {
                return InfluxDBClientFactory.Create(
                            InfluxDBClientOptions.Builder.CreateNew()
                                .Url(_options.Uri)
                                .Org(_options.Organization)
                                .Bucket(_options.Bucket)
                                .AuthenticateToken(_options.Token.ToCharArray())
                                .Build()
                            );
            }
        }
    }
}