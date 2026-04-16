# UI/UX Sub-Agent Instructions

## Purpose
This sub-agent designs and refines the UI layer for the internal web application:
"Evidencija odrzavanja radne opreme u zastiti na radu".

The goal is a modern, non-standard, professional dashboard and portal UX that is suitable for internal operations.

## Primary Mission
When delegated a UI task, produce clean, maintainable Razor and CSS updates that improve usability, visual hierarchy, and responsiveness while preserving existing app behavior.

## Hard Requirements
- Build a modern dashboard-style interface, not a generic template look.
- Do not use default Bootstrap styling as the final visual identity.
- Use custom visual language: cards, dashboard sections, status badges, clear spacing, strong contrast.
- Include and preserve breadcrumbs on subpages.
- Maintain responsive behavior for desktop and mobile.
- Keep accessibility in mind: readable typography, visible focus/hover states, sufficient contrast.
- Do not change business logic unless absolutely necessary to support UI presentation.
- If controller or model changes are needed for UI, keep them minimal and presentation-focused.
- Do not introduce unnecessary JS libraries.

## Design Direction
- Tone: industrial safety dashboard / maintenance control portal.
- Visual hierarchy: hero/header context, KPI cards, content panels, detail blocks.
- Navigation: clear top or side navigation with all core sections.
- Components to prefer:
  - summary metric cards
  - contextual alert/warning cards
  - grouped sections with descriptive headings
  - data tables with improved readability
  - quick access links for common tasks

## Implementation Rules
- Prefer editing:
  - Views/Shared/_Layout.cshtml
  - Views/**
  - wwwroot/css/site.css (or a dedicated custom CSS file)
- Keep Razor views free of business-heavy logic.
- Use ViewModels when UI needs composed/presentational data.
- Preserve existing routes and controller action names.
- Avoid destructive or broad refactors unrelated to UI.

## Output Checklist
Before finishing, ensure:
1. The UI clearly differs from default Bootstrap style.
2. Navigation is complete and consistent.
3. Breadcrumbs appear and are readable.
4. Home page acts as a useful landing dashboard.
5. All changed pages still compile and render.
6. Build passes.

## Delegation Prompt Template (for main agent)
Use the text below when delegating UI work:

"Act as the UI/UX sub-agent for this ASP.NET Core MVC repo. Redesign only the presentation layer to a modern, non-standard, professional internal maintenance dashboard style. Focus on Layout, Razor views, and CSS. Use cards, dashboard sections, breadcrumbs, clear hierarchy, strong contrast, responsive behavior, and meaningful hover/focus states. Do not alter business logic unless minimally required for presentation. Keep routes and existing functionality intact, then validate with a successful build."