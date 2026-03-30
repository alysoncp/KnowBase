# KnowBase V1 Architecture

## Core Platform

The reusable platform owns:

- employee chat experience
- retrieval orchestration
- prompt assembly
- source citation rendering
- audit logging
- admin controls
- security policy enforcement
- connector execution framework
- model gateway abstraction

## Connector Boundary

Connectors are customer-specific and should only be responsible for:

1. Retrieving content and metadata from source systems
2. Retrieving or translating source-system permissions into KnowBase ACL records

## Initial Deployment Shape

- `apps/web` runs as the employee-facing intranet application
- `services/ingest` runs as a background worker for crawling and indexing
- `services/ai` runs as a private internal AI gateway
- the search/index store and operational database are external infrastructure concerns

## Design Rules

- Retrieval must enforce permissions before prompt construction.
- Retrieved content is treated as untrusted data, not executable instruction.
- Every answer should be grounded in cited source snippets.
- The platform should prefer adapters and configuration over customer-specific branching in core logic.
