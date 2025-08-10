document.addEventListener("DOMContentLoaded", function () {
  // Navbar JS
  (function () {
    const navbar = document.getElementById("site-navbar");
    const accountToggle = document.getElementById("account-toggle");
    const dropdownMenu = document.getElementById("dropdown-menu");
    const accountDropdown = document.getElementById("account-dropdown");
    const navLinks = document.querySelectorAll("#nav-links a");
    const cartCountEl = document.getElementById("cart-count");

    // Scroll shadow
    function onScroll() {
      if (window.scrollY > 0) {
        navbar.classList.add("shadow");
      } else {
        navbar.classList.remove("shadow");
      }
    }
    window.addEventListener("scroll", onScroll, { passive: true });
    onScroll();

    // Dropdown toggle
    function closeDropdown() {
      dropdownMenu.classList.remove("show");
      accountToggle.setAttribute("aria-expanded", "false");
      dropdownMenu.setAttribute("aria-hidden", "true");
    }
    function openDropdown() {
      dropdownMenu.classList.add("show");
      accountToggle.setAttribute("aria-expanded", "true");
      dropdownMenu.setAttribute("aria-hidden", "false");
    }

    accountToggle.addEventListener("click", function (e) {
      e.stopPropagation();
      const isOpen = dropdownMenu.classList.contains("show");
      if (isOpen) closeDropdown();
      else openDropdown();
    });

    // Close dropdown when clicking outside
    document.addEventListener("click", function (e) {
      if (!accountDropdown.contains(e.target)) {
        closeDropdown();
      }
    });

    // Keyboard: close on Escape
    document.addEventListener("keydown", function (e) {
      if (e.key === "Escape") closeDropdown();
    });

    // Active nav link detection by pathname
    function setActiveNav() {
      const path = window.location.pathname.replace(/\/+$/, "") || "/";
      navLinks.forEach((a) => {
        const target = a.getAttribute("data-path") || a.getAttribute("href");
        // match exact path for simple routing; you can enhance with startsWith if needed
        if (target === path) {
          a.classList.add("active");
        } else {
          a.classList.remove("active");
        }
      });
    }
    setActiveNav();

    // Demo: set cart count (replace with dynamic logic)
    // Example: read from localStorage or API. We'll set static 0 as original.
    function updateCartCount(n) {
      cartCountEl.textContent =
        typeof n === "number" && n > 0 ? String(n) : "0";
    }
    updateCartCount(0);

    // If you want SPA style navigation without page reload (for demo only):
    // intercept clicks on nav links and update history + active state
    const isFileProtocol = location.protocol === "file:";
    if (!isFileProtocol) {
      // don't hijack when testing via file:// (it will 404)
      navLinks.forEach((a) => {
        a.addEventListener("click", function (e) {
          const href = a.getAttribute("href");
          // only intercept same-origin internal links
          if (href && href.startsWith("/")) {
            e.preventDefault();
            history.pushState({}, "", href);
            setActiveNav();
            // optionally scroll to top
            window.scrollTo({ top: 0, behavior: "smooth" });
          }
        });
      });
      window.addEventListener("popstate", setActiveNav);
    }
  })();

  // Profile picture upload
  (function () {
    document.querySelector(".upload-circle").addEventListener("click", () => {
      document.getElementById("profilePic").click();
    });
  })();
});
