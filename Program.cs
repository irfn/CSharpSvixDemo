using Svix.Abstractions;
using Svix.Api;
using Svix.Client;
using Svix.Model;
using Svix.Models;
using System;
using static EndpointRepository;
using static WebhookPublisher;


// Change this.
// var authToken =  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE3MDA4NzU5MTYsImV4cCI6MjAxNjIzNTkxNiwibmJmIjoxNzAwODc1OTE2LCJpc3MiOiJzdml4LXNlcnZlciIsInN1YiI6Im9yZ18yM3JiOFlkR3FNVDBxSXpwZ0d3ZFhmSGlyTXUifQ.46lcmaaMmued0Ua2wXCakgyf9a-GxSw1ytYA3ioeEkk";
// local docker svix server
// var svixServerUrl = "http://localhost:8071";

var authToken =  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpYXQiOjE2OTkwOTczNDIsImV4cCI6MjAxNDQ1NzM0MiwibmJmIjoxNjk5MDk3MzQyLCJpc3MiOiJzdml4LXNlcnZlciIsInN1YiI6Im9yZ18yM3JiOFlkR3FNVDBxSXpwZ0d3ZFhmSGlyTXUifQ.FlwgXwAkIZCeJMFUlif4sLCyBH_jsnGK-1PjJ-G9ia8";
// local minikube svix server
var svixServerUrl = "http://localhost:9091";
// use npx localtunnel --port targetPort to get a public url for your local server
// This is your tenants callback url. Note this may be purpose specific. And the endpoint catefory.
var endPointUrl = args[0];

var svix = new Svix.SvixClient(authToken, new SvixOptions(svixServerUrl));

var epr = new EndpointRepository(svix);

var authHeaders = new Dictionary<string, string>
{
    { "Authorization", "Bearer top-secret" }
};
Guid tenantGuid = Guid.NewGuid();
var tenantId = $"tenant-{tenantGuid.ToString()}";
var applicationId = epr.FindOrCreateApp("Apple", tenantId).Id;
var endPointId = await epr.CreateEndpoint("Apple", tenantId, "intent-callback", endPointUrl, authHeaders);

System.Console.WriteLine($"Application Id: {applicationId}");
System.Console.WriteLine($"Endpoint Id: {endPointId}");

string eventId = $"evt_{Guid.NewGuid().ToString()}";
await new WebhookPublisher(svix).Publish(applicationId, "upi.payment.callback", eventId, new Dictionary<string, string>
{
    { "status", "blah" },
    { "attempt", "2" }
});