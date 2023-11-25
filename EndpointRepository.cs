using Svix.Abstractions;
using Svix.Api;
using Svix.Client;
using Svix.Model;
using Svix.Models;
using System;

class EndpointRepository
{
    private Svix.SvixClient svix;

    public EndpointRepository(Svix.SvixClient svix)
    {
        this.svix = svix;
    }

    public ApplicationOut FindOrCreateApp(string tenantName, string tenantId)
    {
        var applicationIn = new ApplicationIn(
            name: tenantName,
            uid: tenantId
        ); 

        ApplicationCreateOptions applicationCreateOptions = new ApplicationCreateOptions();
        applicationCreateOptions.GetIfExists = true;
        return this.svix.Application.Create(applicationIn, applicationCreateOptions, tenantId);
    }

    public async Task<String> FindEndpoint(string tenantName, 
        string tenantId,
        string endPointCategory)
    {
        var applicationOut = this.FindOrCreateApp(tenantName, tenantId);

        var endPointId = $"ep-{tenantId}-{endPointCategory}";
        EndpointOut endPoint = await this.svix.Endpoint.GetAsync(tenantId, endPointId);
        return endPoint.Id;
    }


    public async Task<String> CreateEndpoint(string tenantName, 
            string tenantId,
            string endPointCategory,
            string endPointUrl,
            Dictionary<string, string> authHeaders) {
        var applicationOut = this.FindOrCreateApp(tenantName, tenantId);

        var endPointId = $"ep-{tenantId}-{endPointCategory}";
        EndpointOut endPoint;

        var epIn = new EndpointIn( 
            description: "Default Endpoint",
            uid: endPointId,
            url: endPointUrl,
            version: 1
        );
        endPoint = this.svix.Endpoint.Create(applicationOut.Id ,epIn, endPointId);
        await svix.Endpoint.UpdateHeadersAsync(applicationOut.Id, endPoint.Id, new EndpointHeadersIn(
                headers: authHeaders
        ));


        EndpointOut updatedEp = this.svix.Endpoint.GetAsync(applicationOut.Id, endPoint.Id).Result;
        System.Console.WriteLine($"Application: {applicationOut.Id}, Endpoint: {endPoint.Id}, Url: {endPoint.Url}, endPointUid: {endPoint.Uid}");
        System.Console.WriteLine($"Endpoint: {updatedEp.Id}, Url: {updatedEp.Url}, endPointUid: {updatedEp.Uid}");
        return endPoint.Id;
    }
}