## Exploration: port-php-panel-parity

### Current State
The .NET SDK (`SDK-tecnofact-net`) currently contains data models (`Cfdi4Request`, `Emisor`, `Receptor`, `Concepto`, etc.), custom exceptions, enums, configuration settings, and an initial HTTP client (`TecnoFactHttpClient`). However, it lacks the full service orchestration layer, XML building logic (`CfdiXmlBuilder`), authentication service (`AuthService`), cancellation, consultation, reporting, and validation services present in the PHP SDK (`SDK-Tecnofact-php` v1.1.0).

### Affected Areas
- `TecnoFact.SDK/Services/AuthService.cs` — New authentication service for token management.
- `TecnoFact.SDK/Services/CfdiService.cs` — New CFDI timbrado and retrieval service.
- `TecnoFact.SDK/Xml/CfdiXmlBuilder.cs` — XML generation builder for CFDI 4.0.
- `TecnoFact.SDK/Responses/ResultadoTimbrado.cs` — Typed response object for timbrado results.

### Approaches
1. **Full-stack parity port** — Port all 6 services (`AuthService`, `CfdiService`, `CancelacionService`, `ConsultasService`, `ReportesService`, `ValidacionesService`) and all response types/builders in a single massive change.
   - Pros: Complete feature parity immediately.
   - Cons: Massively exceeds the 400-line review budget, high risk, difficult review.
   - Effort: High

2. **Bounded Incremental Parity Slice (Recommended)** — Port core authentication (`AuthService`) and basic timbrado (`CfdiService` + `CfdiXmlBuilder` + `ResultadoTimbrado`) as the first slice.
   - Pros: Fits well within the 400-line review budget, establishes clean architectural patterns for subsequent services, testable end-to-end.
   - Cons: Does not cover cancellation, consultation, or reporting yet.
   - Effort: Medium

### Recommendation
Adopt Approach 2 (Bounded Incremental Parity Slice). Port `AuthService`, `CfdiService` (timbrar), `CfdiXmlBuilder`, and `ResultadoTimbrado` first. This establishes the exact pipeline used by the PHP SDK v1.1.0 while keeping PR sizes reviewable and disciplined.

### Risks
- XML formatting and namespace correctness matching the SAT CFDI 4.0 schema required by the TecnoFact panel.
- Token lifecycle management consistency between `TecnoFactConfig` and `AuthService`.

### Ready for Proposal
Yes — proceed with creating the proposal and specs for `port-php-panel-parity` targeting the first bounded slice (`AuthService` + `CfdiService` timbrado).
