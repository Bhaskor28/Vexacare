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

        // ASP.NET compatible active nav link detection
        function setActiveNav() {
            const path = window.location.pathname.toLowerCase();
            const search = window.location.search.toLowerCase();
            const fullPath = path + search;

            navLinks.forEach((a) => {
                let target = a.getAttribute("href").toLowerCase();

                // Handle ASP.NET's URL patterns
                if (target.startsWith("/home") || target === "/") {
                    if (path === "/" || path.startsWith("/home")) {
                        a.classList.add("active");
                    } else {
                        a.classList.remove("active");
                    }
                }
                // Handle query strings
                else if (fullPath.startsWith(target)) {
                    a.classList.add("active");
                }
                // Exact match for other pages
                else if (path === target || path.startsWith(target + "/")) {
                    a.classList.add("active");
                } else {
                    a.classList.remove("active");
                }
            });
        }

        // Initialize and listen for navigation changes
        setActiveNav();
        window.addEventListener("popstate", setActiveNav);

        // Update active nav when clicking links (for SPA-like behavior)
        navLinks.forEach(link => {
            link.addEventListener("click", function () {
                setTimeout(setActiveNav, 50); // Small delay to allow URL change
            });
        });

        // Demo: set cart count (replace with dynamic logic)
        function updateCartCount(n) {
            cartCountEl.textContent =
                typeof n === "number" && n > 0 ? String(n) : "0";
            cartCountEl.style.display = n > 0 ? "flex" : "none";
        }
        updateCartCount(0);
    })();

    // Profile picture upload
    (function () {
        const uploadCircle = document.querySelector(".upload-circle");
        if (uploadCircle) {
            uploadCircle.addEventListener("click", () => {
                document.getElementById("profilePic").click();
            });
        }
    })();

    // Debugging helper (can be removed in production)
    console.log("Main.js initialized");
    console.log("Current path:", window.location.pathname);
});


