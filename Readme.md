# Containerized .NET 10 Minimal API 
![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker&logoColor=white)

This project shows how to containerize a **.NET 10 Minimal API** using a multi-stage Docker build. This approach ensures that the final image is lightweight and contains only the necessary files to run the application.

## Dockerfile Breakdown

The `Dockerfile` is split into two distinct stages: **Preparation** (Build) and **Runtime**.

### 1. Preparation Stage (SDK)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS preparation
WORKDIR /app
COPY *.csproj .
RUN dotnet restore 
COPY . .
RUN dotnet publish -c Release -o ./out

```

* We use the full **.NET 10 SDK**  to build the app.

* **Layer Optimization:** We copy the `.csproj` and run `dotnet restore` *before* copying the rest of the code. This allows Docker to cache your NuGet packages

* **Publishing:** The `dotnet publish` command compiles the app into a production-ready set of files inside the `/app/out` folder.

### 2. Runtime Stage (ASP.NET)

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=preparation /app/out .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "containerapi.dll"]

```

* **Why:** we use `Multi-stage` building we switch to the **ASP.NET Runtime** image. It is significantly smaller and more secure for production.

* **Configuration:** The `ENV ASPNETCORE_URLS` tells Asp.net app to listen on port `8080`, matching the `EXPOSE` port.

---

## 🚀 How to Build and Run

### 1. Build the Docker Image

Ensure you are on the directory where `Dockerfile` exists and type in terminal:

```bash
docker build -t my-minimal-api .

```

### 2. Start the Container

Run the following command to start the container and map your local port `80` to the container's port `8080`:

```bash
docker run -d -p 80:8080  my-minimal-api

```

---

## ✅ Verifying the Deployment

Once the container is running, you can verify it is working using your local machine:

### 1. Check via Browser

Open your browser and navigate to:
**`http://localhost`**

it should output -> "Hello from .NET Minimal API!,status = running" 

Open your browser and navigate to:
**`http://localhost/tasks`**

it should output -> "Id = 1, Title = "Learn Docker", IsCompleted = true" 


### 2. Check Docker Status

To confirm the container is active and see its logs, use:

```bash
# To see if it's running
docker ps

# To view the application logs
docker logs my-api-instance

```
