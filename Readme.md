# About
This is intended as an example usage of svix webhooks.

## Run Locally 
### Assumption
- The Webhook Service is already running
- docker-compose.yml describes the svix setup

### Prerequisite
#### Macos
1. `brew install dotnet`
#### Other Steps
- Update the Webook server URL svixServerUrl in Program.cs
- Update the authToken in Program.cs
- Run a local server for emulating a webhook target eg: `python server.py` where server.py is https://gist.github.com/mdonkers/63e115cc0c79b4f6b8b3a6b797e485c7
- use `npx localtunnel --port targetPort` to expose this hook
- pass the local tunnel url `dotnet run https://this-is-an-example.loca.lt` 

### Build
`dotnet build`

### Run
`dotnet run url`
