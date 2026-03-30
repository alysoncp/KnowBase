# KnowBase

KnowBase is an on-prem knowledge assistant for engineering firms. The platform is designed to index internal project documents, preserve source-system permissions, and let employees ask grounded questions through a chat interface backed by local LLM infrastructure.

## Repository Layout

- `apps/web`: ASP.NET Core intranet web application and orchestration API
- `services/ingest`: Python ingestion worker for connectors, parsing, and indexing jobs
- `services/ai`: Python AI gateway for embeddings, reranking, and local model integration
- `packages/contracts`: Shared contracts and schemas used across services
- `docs`: Architecture and planning documents

## Current Status

This repository contains the initial scaffold only. The web application has been created as hand-written starter files because the local .NET SDK is not installed in this environment yet.

## Next Setup Steps

1. Install the .NET SDK on the development machine.
2. Create and activate Python virtual environments for `services/ingest` and `services/ai`.
3. Decide on the first connector target and index schema.
4. Wire up the first end-to-end vertical slice: auth placeholder, health check, ingestion stub, retrieval stub.
