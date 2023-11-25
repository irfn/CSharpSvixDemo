using Svix.Abstractions;
using Svix.Api;
using Svix.Client;
using Svix.Model;
using Svix.Models;
using System;

class WebhookPublisher
{
    private Svix.SvixClient svix;

    public WebhookPublisher(Svix.SvixClient svix)
    {
        this.svix = svix;
    }

    public async Task Publish(string applicationId, string eventType, string eventId, Dictionary<string, string> payload)
    {
        payload.Add("id", $"{eventType.Replace(".","_")}_{eventId}");
        await this.svix.Message.CreateAsync(applicationId, new MessageIn(
            eventType: eventType,
            payload: payload,
            eventId: eventId
        ));
    }
}