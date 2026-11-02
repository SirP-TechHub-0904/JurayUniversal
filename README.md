# JurayUniversal — Single-Core Multi-Tenant SaaS, CMS & E-Government MIS Engine

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Multi--Tenant%20Clean%20Architecture-0A66C2)](#multi-tenant-system-architecture)
[![Scale](https://img.shields.io/badge/Scale-100%2B%20Institutional%20Tenants-00A859)](#enterprise-capabilities--scale)
[![Database](https://img.shields.io/badge/Database-Database--Per--Tenant%20SQL%20Server-CC2927?logo=microsoftsqlserver)](#multi-tenant-system-architecture)
[![CI/CD](https://img.shields.io/badge/CI%2FCD-Dual%20Pipelines-2088FF?logo=githubactions)](#ci-cd--security-governance)

A flagship **.NET 8 Single-Core, Multi-Tenant Enterprise Web & Management Information System (MIS) Engine** architected to power **100+ government ministries, national institutes (including NIPSS), NGOs, and corporate portals** from a unified, centrally governed codebase—with **strict per-tenant database isolation** and a dedicated **Master SuperAdmin Control Plane (`JurayUniversal.MisCentral`)**.

---

## Enterprise Capabilities & Scale

- **100+ Institutional Websites from a Single Core**: Dynamic `Host` / domain resolution (`TenantScopeMiddleware`, `TenantProvider`) maps each incoming institutional domain to its own isolated SQL Server connection string, theme layout, and feature configuration at runtime.
- **Database-Per-Tenant Data Isolation**: Eliminates cross-tenant data leakage risks—critical for E-Government agencies, public-sector portals, and regulated NGOs—while sharing zero-overhead stateless compute.
- **Centralized Multi-Tenant Control Plane (`MisCentral`)**: Dedicated `JurayUniversal.MisCentral` administration portal enabling super-administrators to monitor, provision, and govern tenant MIS instances across domains.
- **Modular Institutional Suite**:
  - Dynamic Multi-Theme CMS (10+ enterprise layout engines, custom sections, blogs, sliders, document libraries)
  - Membership, Alumni & Cohort MIS (`GroupAlias`, `AlumniAlias`, RBAC dashboards, dues & financial tracking)
  - Training, Assessment & Digital Certificate Issuance
  - Integrated Omnichannel Communications (**Postmark**, **ZeptoMail** with retry & audit logs `MisNewsletterEmailLogs`, **KudiSMS**)
  - Multi-Gateway Collections (**Paystack**, **PixelPay**) & **Amazon AWS S3** object storage.

---

## Multi-Tenant System Architecture

```mermaid
flowchart TB
    subgraph Domains["100+ Live Institutional Domains"]
        GOV["Government Portals (Ministries & Agencies)"]
        INS["National Institutes & Alumni (NIPSS / Research)"]
        NGO["International & Local NGOs / Corporate Tenants"]
        OPS["Platform Super-Admins"]
    end

    subgraph Edge["Linux VPS + Nginx Reverse Proxy"]
        NGX["Nginx Wildcard / Multi-Domain Proxy"]
        WEB["JurayUniversal.Website (Multi-Tenant Runtime Engine)"]
        CTRL["JurayUniversal.MisCentral (Master Control Plane)"]
    end

    subgraph Middleware["Tenant Resolution & Clean Architecture"]
        MW["TenantScopeMiddleware + TenantProvider (Host -> Tenant Scope)"]
        APP["JurayUniversal.Application (Services, DTOs & Use Cases)"]
        DOM["JurayUniversal.Domain (Entities, Enums & Tenant Contracts)"]
        INF["JurayUniversal.Persistence.EF.SQL (Per-Tenant DbContext Factory)"]
    end

    subgraph DataIsolation["Isolated Per-Tenant Persistence & Cloud"]
        MASTER_DB[("Master Domain Registry DB")]
        T1_DB[("Tenant A SQL DB (Gov)")]
        T2_DB[("Tenant B SQL DB (Institute)")]
        TN_DB[("Tenant N SQL DB (NGO)")]
        CLOUD["AWS S3 | Postmark | ZeptoMail | Paystack | KudiSMS"]
    end

    GOV & INS & NGO --> NGX --> WEB --> MW
    OPS --> NGX --> CTRL --> MASTER_DB
    MW --> APP --> DOM
    APP --> INF
    INF --> T1_DB & T2_DB & TN_DB
    APP --> CLOUD
```

---

## Solution Structure

```text
JurayUniversal/
├── .github/workflows/
│   ├── deploy-jurayuniversal.yml              # CI/CD pipeline for Multi-Tenant Website Engine
│   └── deploy-miscentral.yml                  # CI/CD pipeline for Master Control Plane (MisCentral)
├── src/
│   ├── JurayUniversal.Domain/                 # Core multi-tenant entities, MIS models, and enums
│   ├── JurayUniversal.Application/            # Business services, AWS S3, Email/SMS, and DTOs
│   ├── JurayUniversal.Infrastructure/         # TenantProvider, scope management, and cross-cutting concerns
│   ├── JurayUniversal.Persistence.EF.SQL/     # Dynamic EF Core SQL Server persistence & migrations
│   ├── JurayUniversal.Website/                # Multi-tenant Razor Pages / Areas web engine & theme layouts
│   ├── JurayUniversal.MisCentral/             # SuperAdmin multi-tenant orchestration & governance portal
│   └── PostmarkEmailService/                  # Transactional email client library
└── JurayUniversal.sln                         # Visual Studio Solution
```

---

## Technology Stack

- **Platform**: C#, .NET 8, ASP.NET Core Razor Pages & Multi-Area Architecture
- **Multi-Tenancy**: Custom Host-Header `TenantScopeMiddleware`, Scoped `TenantProvider`, Per-Tenant SQL Server Connection Routing
- **ORM & Database**: Entity Framework Core 8, Microsoft SQL Server
- **Security & Observability**: ASP.NET Core Identity (`Profile`, `AppRole`), Serilog Structured Logging, Google reCAPTCHA
- **Cloud, Messaging & Payments**: Amazon S3, Postmark, ZeptoMail, KudiSMS, Paystack, PixelPay
- **CI/CD & Hosting**: GitHub Actions (dual `rsync` pipelines preserving `.env*`), Linux `systemd` (`jurayuniversal` & `juray-miscentral`), Nginx

---

## Local Development Setup

```bash
git clone https://github.com/VS-GitHub-001/JurayUniversal.git
cd JurayUniversal

# Configure environment variables
cp src/JurayUniversal.Website/.env.example src/JurayUniversal.Website/.env
cp src/JurayUniversal.MisCentral/.env.example src/JurayUniversal.MisCentral/.env

# Restore & Build both production applications
dotnet restore JurayUniversal.sln
dotnet build src/JurayUniversal.Website/JurayUniversal.Website.csproj -c Release
dotnet build src/JurayUniversal.MisCentral/JurayUniversal.MisCentral.csproj -c Release
```

---

## Author

**Peter Emeka Onwuka**  
Chief Technology Officer & Enterprise Systems Architect  
- GitHub: [@SirP-TechHub-0904](https://github.com/SirP-TechHub-0904)  
- LinkedIn: [Peter Emeka Onwuka](https://www.linkedin.com/in/onwuka-peter-emeka)

