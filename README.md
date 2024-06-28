# Doc Planner Challenge by Abel

## Introducción

This repository contains the Doc Planner challenge done in C#.

To complete this challenge, I relied somewhat on the information obtained through questions I asked via email. I developed middleware as a REST API in a web app that will act as a communicator between client and server through the HTTP protocol.

For this, I based it on a vertical slice architecture using MediatR in a conventional API.

Here, I had the dilemma of whether to use a minimal API or not. In the end, I opted for the normal API because it offers easier scalability. However, since it is decoupled by handlers, if in the future it needs to be changed, there would be no problem other than changing the entry points (controllers).

For the HTTP client, I followed Microsoft's recommendations, which suggest using IHttpFactory as the framework itself manages the HTTP manager. This way, in case of a heavy overload, we will avoid blocking all TCP/IP ports.

For authentication, I used a configuration file since it is not specified in the document that the user and password need to be entered. This added complexity does not contribute anything, as for system-to-system communication it is better to do it through configuration (using key vault), and if middleware authentication is required, there are other tools available.

I also had the dilemma of whether or not to use caching. I decided not to because I have no way of notifying if there have been changes from the source side. If caching were to be implemented, I would recommend a two-level cache using local memory and another using Redis. This cache would be used in the DocPlannerService to save the HTTP call.

## Content 

- `Api.csproj`: Here is where the features is done
- `VerticalSlice.csproj`: Helpers to make easier create new controllers reusing the same scafold. I am based on MediatR for it and fluentvalidations to validate models.
- `DocPlanner.Client`: Project where I had develop an API Client for DocPlanner.
- `*.Test`: All projecs has their own test project related with the same name. 
## Config and deploy

You can just run the deploy.ps1 in powershell as administrator and use this url in your browser:

http://localhost:9091/swagger/index.html

### Requirements

- Docker
- PowerShell (if you are using Windows)

### Steps Build and run

1. **Clone the repository:**

    ```sh
    git clone https://github.com/tu-usuario/doc-planner-challenge.git
    cd doc-planner-challenge
    ```

2. **Edit file `local.setting.json` with your own user/password and base url if it is needed:**

    ```json
    {
          "DocPlannerOptions:BaseUrl": "https://draliatest.azurewebsites.net",
          "DocPlannerOptions:User": "user",
          "DocPlannerOptions:Password": "password"
    }


3. **Build and execute docker image using the Powershell script:**

    ```ps1
    .\docker.ps1
    ```
    Use the next url
http://localhost:9091/swagger/index.html

**Observations:**


Since the user interface is not being evaluated, I have based the UI directly on Swagger.

You can use the following examples:

GET - WeeklyAvailability

http://localhost:9091/api/v1/WeeklyAvailability/20240624

POST - TakeSlot

http://localhost:9091/api/v1/TakeSlot

```json
{
    "FacilityId": "90c9f71c-685f-48e7-a6d5-7898775209ce",
    "Start": "2024-06-24 10:00",
    "End": "2024-06-24 10:10",
    "Comments": "comment",
    "Patient": {
        "Name": "Patient Name",
        "SecondName": "Patient Second Name",
        "Email": "test@gmail.com",
        "Phone": "611040456"
    }
}



