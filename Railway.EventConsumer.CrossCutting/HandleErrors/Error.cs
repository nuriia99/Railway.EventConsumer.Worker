using System.Net;

namespace Railway.EventConsumer.CrossCutting.HandleErrors
{
    public sealed record Error(HttpStatusCode Code, string Description) { }
}
