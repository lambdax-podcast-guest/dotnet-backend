using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;

namespace Guests.Tests
{
    public class JsonHelper
    {
        /// <summary>Turn any input class into json content for post</summary>
        public static JsonContent CreatePostContent<T>(T genericObject)
        {
            // turn the register input into json and set the request headers
            JsonContent content = JsonContent.Create(genericObject);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        /// <summary>Deserialize bad request object, then deserialize it's errors object, and return the errors</summary>
        public async static Task<Errors> DeserializeResponseAndReturnErrors(HttpResponseMessage response)
        {
            // deserialize the stream into what we expect the output to look like
            CustomBadRequest responseObject = await JsonSerializer.DeserializeAsync<CustomBadRequest>(response.Content.ReadAsStreamAsync().Result);

            // the errors string needs to be deserialized into an object
            return JsonSerializer.Deserialize<Errors>(responseObject.errors.ToString());
        }

        /// <summary>Deserialize response message to a dynamic type, ensure the serialization was successful, and return the serialized content</summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <param name="response">The response to be deserialized</param>
        public async static Task<T> TryDeserializeJson<T>(HttpResponseMessage response)
        {
            // so we will deserialize from a stream
            string responseAsString = await response.Content.ReadAsStringAsync();

            Func<T> deserialize = () => JsonSerializer.Deserialize<T>(responseAsString);

            // ensure the serialization worked 
            deserialize.Should().NotThrow("because we expect the serializer to work if the request was valid. This assertion is made in the JsonHelper");

            // deserialize the stream into what we expect the output to look like
            return deserialize();
        }
    }
}
