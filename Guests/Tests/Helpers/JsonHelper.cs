using System;
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
        /// <typeparam name="T"> The type of the object to be created into JsonContent</typeparam>
        /// <param name="genericObject">The object instance to be created into JsonContent</param>
        public static JsonContent CreatePostContent<T>(T genericObject)
        {
            // turn the input into json and set the request headers
            JsonContent content = JsonContent.Create(genericObject);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        /// <summary>Deserialize bad request object, then deserialize it's errors object, and return the errors</summary>
        /// <param name="response">HttpResponseMessage that can be deserialized into a CustomBadRequest object</param>
        public async static Task<Errors> DeserializeResponseAndReturnErrors(HttpResponseMessage response)
        {
            // deserialize the stream into what we expect the output to look like
            CustomBadRequest responseObject = await TryDeserializeJson<CustomBadRequest>(response);

            // the errors string needs to be deserialized into an object
            // the deserialization may have worked even if errors weren't present on the response
            // so if errors is null, this deserialize call will throw
            // we will use fluent assertions here to make sure we get a clean error message
            Func<Errors> deserializeErrors = () => JsonSerializer.Deserialize<Errors>(responseObject.errors.ToString());

            // use fluent assertions to clearly show if something is wrong with this call to the serializer
            deserializeErrors.Should().NotThrow("because we deserialized a bad request object and we expect there to be an errors field with a value present on that bad resquest. This assertion is made in the JsonHelper.");

            return deserializeErrors();
        }

        /// <summary>Deserialize response message to a dynamic type, ensure the serialization was successful, and return the serialized content</summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <param name="response">The response to be deserialized</param>
        public async static Task<T> TryDeserializeJson<T>(HttpResponseMessage response)
        {
            // await the response as a string in order to deserialize it
            string responseAsString = await response.Content.ReadAsStringAsync();

            // create a func that targets the deserialize method from JsonDeserializer
            // this will allow us to use fluent assertions to create a clear error message if there is something wrong with the response or the type we are trying to deserialize to
            Func<T> deserializeResponse = () => JsonSerializer.Deserialize<T>(responseAsString);

            // ensure the serialization worked using fluent assertions
            // not sure if it is best practice to have an assertion out here in a helper, but we are going to use this deserialization a lot
            deserializeResponse.Should().NotThrow("because we expect the serializer to work if the request was valid. This assertion is made in the JsonHelper.");

            // deserialize the stream into what we expect the output to look like
            return deserializeResponse();
        }
    }
}
