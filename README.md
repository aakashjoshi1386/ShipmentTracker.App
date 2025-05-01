# ShipmentTracker Application

## Overview

This repository implements a Clean Architecture-based .NET 8 solution following Domain-Driven Design (DDD) principles with the CQRS pattern via MediatR, and a frontend built with Next.js (React + TypeScript + MUI).

---

## 🔧 Solution Structure

ShipmentTracker.sln │ ├── ShipmentTracker.App.API --> ASP.NET Core Web API ├── ShipmentTracker.App.Application --> Application Layer (CQRS, Validators, MediatR) ├── ShipmentTracker.App.Infrastructure --> EF Core, DI, Configuration ├── ShipmentTracker.App.Domain --> Domain Entities & Interfaces └── shipment-tracker-frontend --> Next.js React App (TypeScript + MUI)

---

## ⚙️ Project Creation & Dependencies

### 🧱 Create Blank Solution

```bash
dotnet new sln -n ShipmentTracker

1️⃣ API Project – ShipmentTracker.App.API

dotnet new webapi -n ShipmentTracker.App.API
cd ShipmentTracker.App.API
dotnet add package AspNetCoreRateLimit
dotnet add package FluentValidation.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.Design
cd ..
dotnet sln add ShipmentTracker.App.API

2️⃣ Application Layer – ShipmentTracker.App.Application

dotnet new classlib -n ShipmentTracker.App.Application
cd ShipmentTracker.App.Application
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
dotnet add package MediatR
dotnet add package Microsoft.Extensions.DependencyInjection
cd ..
dotnet sln add ShipmentTracker.App.Application

3️⃣ Infrastructure Layer – ShipmentTracker.App.Infrastructure

dotnet new classlib -n ShipmentTracker.App.Infrastructure
cd ShipmentTracker.App.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.Extensions.Configuration.Abstractions
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add package Microsoft.Extensions.Options
cd ..
dotnet sln add ShipmentTracker.App.Infrastructure


4️⃣ Domain Layer – ShipmentTracker.App.Domain

dotnet new classlib -n ShipmentTracker.App.Domain
dotnet sln add ShipmentTracker.App.Domain

🎨 Frontend – shipment-tracker-frontend

npx create-next-app@latest shipment-tracker-frontend --typescript
cd shipment-tracker-frontend

npm install @mui/material @mui/icons-material @emotion/react @emotion/styled
npm install axios @mui/material-nextjs @emotion/cache

🚀 Deployment Instructions
📌 Backend API – Using Visual Studio 2022
Right-click ShipmentTracker.App.API → Publish

Create a new Publish Profile

Select Azure App Service (Windows):

Target: Existing Resource Group

Create or select App Service Plan

Confirm App Service deployment

Publish → Confirm success from output logs

(Optional) Add APIM integration from Azure Portal under your App Service settings

🔄 Backend CI/CD – Using GitHub Actions

# .github/workflows/dotnet-api-deploy.yml
name: Deploy .NET API

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Publish
        run: dotnet publish ShipmentTracker.App.API -c Release -o publish
      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: '<Your-AppService-Name>'
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
          package: ./publish

**Note: Replace <Your-AppService-Name> and set AZURE_PUBLISH_PROFILE in GitHub Secrets.

🌐 Frontend Deployment – VS Code + Azure
1. Build Frontend

npm run build
(*) Ensure .env.production has the correct API base URLs

2. SPA Routing (web.config for Azure)
  Create web.config in out/ or dist/ folder:

  <configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="SPA" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>

3. Deploy via Azure Static Web App or App Service:
  (*) Use Azure Static Web App deployment from VS Code Azure extension
  (*) Or publish out/ folder to App Service via FTP/Zip deploy

4. Register App in Azure AD
  (*) Go to Azure Portal → Azure Active Directory > App Registrations
  (*) Register the frontend app:
        1. Redirect URI: https://<your-app>.azurewebsites.net
        2. Save Application (Client) ID
  (*) Configure necessary API permissions & CORS settings

🧪 Environment Configuration
Use a .env.local or .env.production file:

NEXT_PUBLIC_API_BASE_URL=https://<your-api-endpoint>
NEXT_PUBLIC_AUTH_CLIENT_ID=<AzureAppClientId>

const api = process.env.NEXT_PUBLIC_API_BASE_URL;

✅ Summary
This setup enables a robust full-stack environment following Clean Architecture for the backend and modern MUI-powered frontend with
CI/CD and Azure deployment capabilities.
