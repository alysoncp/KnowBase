namespace KnowBase.Web.Retrieval;

public static class SampleKnowledgeBase
{
    public static IReadOnlyList<RetrievalDocument> Documents { get; } =
    [
        new(
            "project-ev-204-spec",
            "East Valley Pump Station Upgrade Specification",
            @"\\intranet\projects\EastValley\EV-204\specification.pdf",
            "Specification",
            "East Valley Water Authority",
            "EV-204",
            "Upgrade of an aging wastewater pump station with corrosion mitigation requirements and coating standards.",
            "The specification covers duplex pumps, epoxy-coated piping, hydrogen sulfide corrosion mitigation, and replacement of deteriorated wet-well equipment.",
            ["pump station", "wastewater", "corrosion", "coating", "wet well", "duplex pumps"]),
        new(
            "project-ev-204-lessons",
            "East Valley Pump Station Rehabilitation Lessons Learned",
            @"\\intranet\projects\EastValley\EV-204\lessons-learned.docx",
            "Lessons Learned",
            "East Valley Water Authority",
            "EV-204",
            "Construction lessons learned from a pump station rehabilitation completed in an occupied utility corridor.",
            "Crew coordination improved when shutdown sequencing was documented early, coating submittals were reviewed with operations staff, and spare bypass fittings were staged on site.",
            ["shutdown sequencing", "rehabilitation", "operations", "coating submittals", "bypass"]),
        new(
            "project-br-118-memo",
            "County Route 18 Bridge Rehabilitation Memo",
            @"\\intranet\projects\CountyDOT\BR-118\rehabilitation-memo.pdf",
            "Technical Memo",
            "County Department of Transportation",
            "BR-118",
            "Bridge rehabilitation planning memo focused on staged construction, lane closures, and concrete durability.",
            "The memo summarizes traffic-control phasing, deck repair sequencing, and protective coating selection for steel elements exposed to deicing salts.",
            ["bridge", "rehabilitation", "traffic control", "deck repair", "steel coating"]),
        new(
            "project-sw-077-compliance",
            "North Coast Industrial Stormwater Compliance Review",
            @"\\intranet\projects\NorthCoast\SW-077\compliance-review.pdf",
            "Compliance Review",
            "North Coast Manufacturing",
            "SW-077",
            "Stormwater compliance review for an industrial campus expansion with erosion and sediment control recommendations.",
            "The review highlights permit renewal timing, inspection documentation, and corrective actions for exposed stockpiles near drainage inlets.",
            ["stormwater", "industrial", "permit", "erosion control", "inspection"])
    ];
}
