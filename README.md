# polyglot-apps

A monorepo showng GitHub Actions patterns across different languages: Go, Node.js, Python, .NET, and Java Spring Boot.

## What this showcases

| Pattern | Where |
|---|---|
| **Path filtering** | Each `ci-*.yml` only triggers on changes within its service directory |
| **Per-language caching** | Go modules, npm, pip, NuGet, Maven — all cached to speed up CI |
| **Matrix builds** | Node tests across v18/v20/v22; Python across 3.11/3.12 |
| **Reusable workflow** | `reusable-service-release.yml` called by all 5 service pipelines |
| **Docker build & push** | Multi-stage Dockerfiles pushed to GHCR with layer caching |
| **Central CI actions** | Versioning, tagging, changelog, and releases via [`mmastersvz/central-ci`](https://github.com/mmastersvz/central-ci) |
| **Concurrency control** | `cancel-in-progress: true` kills stale runs on rapid pushes |
| **Independent versioning** | Each service is tagged separately (e.g. `go-service/v1.2.3`) |

## Repository structure

```
polyglot-actions-showcase/
├── .github/
│   └── workflows/
│       ├── ci-go.yml                    # Go: path filter, module cache, tests
│       ├── ci-node.yml                  # Node: path filter, npm cache, matrix
│       ├── ci-python.yml                # Python: path filter, pip cache, matrix
│       ├── ci-dotnet.yml                # .NET: path filter, NuGet cache, tests
│       ├── ci-java.yml                  # Java: path filter, Maven cache, tests
│       └── reusable-service-release.yml # Shared: version → docker → release
└── services/
    ├── go-service/          # Go 1.22 HTTP server
    ├── node-service/        # Express + Jest
    ├── python-service/      # FastAPI + pytest
    ├── dotnet-service/      # .NET 8 Minimal API + xUnit
    └── java-service/        # Spring Boot 3 + MockMvc
```

## How it works

### Path filtering
Each workflow only runs when its service directory changes:

```yaml
on:
  push:
    paths:
      - "services/go-service/**"
```

Pushing a change to `services/python-service/` will trigger **only** `ci-python.yml` — the other four workflows stay silent.

### Reusable workflow
All 5 CI workflows call the same release workflow via `workflow_call`:

```yaml
release:
  needs: test
  uses: ./.github/workflows/reusable-service-release.yml
  with:
    service: go-service
    image-name: go-service
    working-directory: services/go-service
    is-release: ${{ github.ref == 'refs/heads/main' }}
  secrets:
    GHCR_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

The reusable workflow handles: version resolution → Docker build/push → git tag → changelog → GitHub Release.

### Independent versioning via central-ci
Each service uses the `service:` input on `next-version` to scope its tags:

| Service | Tag format |
|---|---|
| go-service | `go-service/v1.2.3` |
| node-service | `node-service/v2.0.1` |
| python-service | `python-service/v1.5.0` |
| dotnet-service | `dotnet-service/v1.0.0` |
| java-service | `java-service/v3.1.2` |

Version bumps are driven by [Conventional Commits](https://www.conventionalcommits.org/):
- `feat:` → minor bump
- `fix:` → patch bump
- `feat!:` / `BREAKING CHANGE` → major bump

### Docker images
Images are pushed to GHCR (`ghcr.io/<owner>/<service>`):
- **PR builds** → tagged `pr-<number>`
- **Main builds** → tagged with semver + `latest`
- Docker layer cache stored in GitHub Actions cache (`type=gha`) per service scope

## Prerequisites

### Secrets
| Secret | Description |
|---|---|
| `GITHUB_TOKEN` | Built-in, used for GHCR push and releases |

No additional secrets needed — `GITHUB_TOKEN` covers everything when packages are scoped to the repo owner.

### GHCR permissions
Ensure your repo has **write** access to GitHub Packages. In repo Settings → Actions → General → Workflow permissions, set to **Read and write**.

## Running locally

Each service can be run independently:

```bash
# Go
cd services/go-service
go run .

# Node
cd services/node-service
npm install
npm start

# Python
cd services/python-service
# python3 -m venv venv
# source venv/bin/activate
pip install -r requirements.txt
uvicorn main:app --port 8080
# deactivate

# .NET
cd services/dotnet-service
# dotnet build src/DotnetService
dotnet publish src/DotnetService/DotnetService.csproj -c Release -o ./publish --no-self-contained
# dotnet run --project src/DotnetService

# Java
cd services/java-service
mvn spring-boot:run
```

All services expose `GET /` and `GET /health` on port 8080.

## Running tests locally

```bash
# Go
cd services/go-service && go test ./...

# Node
cd services/node-service && npm ci && npm test

# Python
cd services/python-service && pip install -r requirements.txt && pytest

# .NET
cd services/dotnet-service && dotnet test

# Java
cd services/java-service && mvn test
```
