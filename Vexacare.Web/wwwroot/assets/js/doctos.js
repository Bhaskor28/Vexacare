(function () {
  const grid = document.getElementById("doctors-grid");

  // Replace this image URL with your AllImages.MedicalExperts1 path
  const placeholderImage = "~/assets/images/MedicalExperts1.png";

  // Sample doctors data (you can replace with dynamic data)
  const doctors = [
    {
      name: "Dr. Sarah Johnson",
      role: "Chief Medical Officer",
      img: placeholderImage,
    },
    {
      name: "Dr. Michael Lee",
      role: "Gastroenterologist",
      img: placeholderImage,
    },
    {
      name: "Dr. Priya Nair",
      role: "Clinical Nutritionist",
      img: placeholderImage,
    },
    {
      name: "Dr. David Chen",
      role: "Microbiome Researcher",
      img: placeholderImage,
    },
  ];

  // If you want exactly 8 cards even with less data, you can loop that many times
  const count = 4;

  for (let i = 0; i < count; i++) {
    // choose doc data if available, otherwise fallback to first element
    const d = doctors[i] || doctors[0];

    const col = document.createElement("div");
    col.className = "col-span-3"; // default class; responsive CSS will adapt
    col.innerHTML = `
          <article class="doctor-card" role="article" aria-label="${escapeHtml(
            d.name
          )}">
            <div class="doc-image" aria-hidden="true">
              <img src="${escapeHtml(d.img)}" alt="${escapeHtml(d.name)}">
            </div>
            <div class="doc-body">
              <h3 class="doc-name">${escapeHtml(d.name)}</h3>
              <p class="doc-role">${escapeHtml(d.role)}</p>
              <button class="btn-book" type="button" data-doctor="${escapeHtml(
                d.name
              )}">Book Consultation</button>
            </div>
          </article>
        `;
    grid.appendChild(col);
  }

  // Delegate click for bookings
  grid.addEventListener("click", function (e) {
    const btn = e.target.closest(".btn-book");
    if (!btn) return;
    const docName = btn.getAttribute("data-doctor") || "this expert";
    // Put your navigation/modal opening logic here
    alert("Booking clicked for " + docName + ". Replace with real flow.");
  });

  // View All button action
  document
    .getElementById("view-all-btn")
    .addEventListener("click", function () {
      // Example: navigate to /doctors or /consultations
      // location.href = '/doctors';
      alert('View All clicked — wire this to your "all doctors" page.');
    });

  // Simple safe HTML escape for text inserted into HTML (avoids injection)
  function escapeHtml(str) {
    return String(str).replace(/[&<>"']/g, function (m) {
      return {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;",
      }[m];
    });
  }
})();
