using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public class NewtonsoftSerializer : IElasticsearchSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public NewtonsoftSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public object Deserialize(Type type, Stream stream)
        {
         
            var json = JsonConvert.DeserializeObject(new JsonTextReader(new StreamReader(stream)).ReadAsString(),type);
            return json;
        }

        public T Deserialize<T>(Stream stream)
        {
            var json = JsonConvert.DeserializeObject<T>(new JsonTextReader(new StreamReader(stream)).ReadAsString());
            return json;
        }

        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<object>();
            var json = JsonConvert.DeserializeObject(new JsonTextReader(new StreamReader(stream)).ReadAsString(), type);
            tcs.SetResult(json);
            return tcs.Task;
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<T>();
            var json = JsonConvert.DeserializeObject<T>(new JsonTextReader(new StreamReader(stream)).ReadAsString());
            tcs.SetResult(json);
            return tcs.Task;
        }

        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None)
        {
            var format = formatting == SerializationFormatting.Indented
                ? Formatting.Indented
                : Formatting.None;
            var json = JsonConvert.SerializeObject(data,format );
            stream.Write( Encoding.UTF8.GetBytes(json));
        }

        public Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.None,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var tcs = new TaskCompletionSource<T>();

            var format = formatting == SerializationFormatting.Indented
                ? Formatting.Indented
                : Formatting.None;
            var json = JsonConvert.SerializeObject(data,format );
            stream.WriteAsync( Encoding.UTF8.GetBytes(json));
            return tcs.Task;
        }
    }
}