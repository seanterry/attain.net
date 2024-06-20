// An enhanced load allows users to navigate between different pages
// we will keep this and evaluate it vs hx-boost
Blazor.addEventListener("enhancedload", function () {
  // HTMX need to reprocess any htmx tags because of enhanced loading
  htmx.process(document.body);
});
