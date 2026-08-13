/**
 * XYZ Web Application - Core UI Interactivity Script
 * Provides pure client-side interactivity (sidebar toggles, dropdowns, scroll widget)
 * compatible with ASP.NET Core Razor Pages / MVC and standard HTML.
 */

document.addEventListener('DOMContentLoaded', () => {
  initApp();
});

function initApp() {
  const sidebar = document.getElementById('sidebar');
  const topbar = document.getElementById('topbar');
  const mainContent = document.getElementById('main-content');
  const sidebarBackdrop = document.getElementById('sidebar-backdrop');
  const sidebarToggleBtn = document.getElementById('sidebar-toggle-btn');

  // 1. Mobile & Desktop Sidebar Toggles
  let isMobileOpen = false;
  let isDesktopCollapsed = false;

  function updateSidebarState() {
    const isDesktop = window.innerWidth >= 1024;
    
    if (isDesktop) {
      // Clear mobile classes
      if (sidebar) sidebar.classList.remove('-translate-x-full', 'translate-x-0');
      if (sidebarBackdrop) sidebarBackdrop.classList.add('hidden');
      isMobileOpen = false;

      // Apply desktop collapsed/expanded state
      if (isDesktopCollapsed) {
        if (sidebar) sidebar.classList.add('sidebar-collapsed');
        if (topbar) topbar.classList.add('sidebar-collapsed');
        if (mainContent) mainContent.classList.add('sidebar-collapsed');
      } else {
        if (sidebar) sidebar.classList.remove('sidebar-collapsed');
        if (topbar) topbar.classList.remove('sidebar-collapsed');
        if (mainContent) mainContent.classList.remove('sidebar-collapsed');
      }
    } else {
      // Clear desktop collapsed state on mobile screens
      if (sidebar) sidebar.classList.remove('sidebar-collapsed');
      if (topbar) topbar.classList.remove('sidebar-collapsed');
      if (mainContent) mainContent.classList.remove('sidebar-collapsed');

      // Apply mobile drawer state
      if (isMobileOpen) {
        if (sidebar) {
          sidebar.classList.remove('-translate-x-full');
          sidebar.classList.add('translate-x-0');
        }
        if (sidebarBackdrop) sidebarBackdrop.classList.remove('hidden');
      } else {
        if (sidebar) {
          sidebar.classList.remove('translate-x-0');
          sidebar.classList.add('-translate-x-full');
        }
        if (sidebarBackdrop) sidebarBackdrop.classList.add('hidden');
      }
    }
  }

  if (sidebarToggleBtn) {
    sidebarToggleBtn.addEventListener('click', (e) => {
      e.stopPropagation();
      const isDesktop = window.innerWidth >= 1024;
      if (isDesktop) {
        isDesktopCollapsed = !isDesktopCollapsed;
      } else {
        isMobileOpen = !isMobileOpen;
      }
      updateSidebarState();
    });
  }

  if (sidebarBackdrop) {
    sidebarBackdrop.addEventListener('click', () => {
      isMobileOpen = false;
      updateSidebarState();
    });
  }

  window.addEventListener('resize', () => {
    updateSidebarState();
  });

  // 2. Submenu Accordion Toggles
  if (sidebar) {
    const dropdownButtons = sidebar.querySelectorAll('li > div > button');
    dropdownButtons.forEach((btn) => {
      btn.addEventListener('click', (e) => {
        const container = btn.closest('li');
        if (!container) return;

        const arrowSvg = btn.querySelector('.chevron-icon') || btn.querySelector('svg:last-child');
        let submenu = container.querySelector('.submenu-list');

        // Dynamic fallback creation for standard navigation items if missing in markup
        if (!submenu) {
          const textSpan = btn.querySelector('span');
          const menuTitle = textSpan ? textSpan.innerText.trim() : '';

          if (['Business Management', 'Product Management', 'Category Management', 'Warehouse Management', 'Logistics Partners', 'Rider Management'].includes(menuTitle)) {
            submenu = document.createElement('ul');
            submenu.className = 'submenu-list mt-1 ml-6 pl-4 border-l border-gray-200 space-y-0.5 hidden transition-all duration-200';

            const eyeIcon = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" color="currentColor" class="flex-shrink-0 text-gray-400 group-hover:text-black"><path d="M2 12C2 12 5 5 12 5C19 5 22 12 22 12C22 12 19 19 12 19C5 19 2 12 2 12Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/><circle cx="12" cy="12" r="3" stroke="currentColor" stroke-width="1.5"/></svg>`;
            const plusIcon = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" color="currentColor" class="flex-shrink-0 text-gray-400 group-hover:text-black"><path d="M12 5V19M5 12H19" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/></svg>`;

            let items = [
              { label: `View ${menuTitle.split(' ')[0]}s`, icon: eyeIcon },
              { label: `Add ${menuTitle.split(' ')[0]}`, icon: plusIcon, badge: '<span class="inline-flex items-center px-1.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-600 ml-auto">New</span>' }
            ];

            items.forEach(item => {
              const subLi = document.createElement('li');
              subLi.innerHTML = `
                <a href="#" class="flex items-center px-3 py-2 rounded-md transition-all duration-200 group border border-transparent text-sm text-gray-600 hover:bg-gray-50 hover:text-black hover:border-gray-100">
                  ${item.icon}
                  <span class="ml-2 flex-1">${item.label}</span>
                  ${item.badge || ''}
                </a>`;
              submenu.appendChild(subLi);
            });
            container.appendChild(submenu);
          }
        }

        if (submenu) {
          const isOpen = !submenu.classList.contains('hidden');
          if (isOpen) {
            submenu.classList.add('hidden');
            if (arrowSvg) arrowSvg.style.transform = 'rotate(0deg)';
          } else {
            submenu.classList.remove('hidden');
            if (arrowSvg) arrowSvg.style.transform = 'rotate(180deg)';
          }
        }
      });
    });
  }

  // 3. User Profile Dropdown Toggle
  const profileMenuBtn = document.getElementById('profile-menu-btn');
  const profileMenuDropdown = document.getElementById('profile-menu-dropdown');
  if (profileMenuBtn && profileMenuDropdown) {
    profileMenuBtn.addEventListener('click', (e) => {
      e.stopPropagation();
      const isHidden = profileMenuDropdown.classList.contains('hidden');
      closeAllDropdowns();
      if (isHidden) {
        profileMenuDropdown.classList.remove('hidden');
      }
    });
  }

  // 4. Select Business Dropdown Toggles
  const bizWrappers = document.querySelectorAll('.business-dropdown-wrapper');
  bizWrappers.forEach(wrapper => {
    const btn = wrapper.querySelector('.biz-dropdown-btn');
    const menu = wrapper.querySelector('.biz-dropdown-menu');
    const chevron = wrapper.querySelector('.chevron-icon');

    if (btn && menu) {
      btn.addEventListener('click', (e) => {
        e.stopPropagation();
        const isHidden = menu.classList.contains('hidden');
        closeAllDropdowns();
        if (isHidden) {
          menu.classList.remove('hidden');
          if (chevron) chevron.style.transform = 'rotate(180deg)';
        }
      });

      const items = menu.querySelectorAll('.biz-item');
      items.forEach(item => {
        item.addEventListener('click', (e) => {
          e.preventDefault();
          e.stopPropagation();
          const selectedText = item.innerText.trim();

          document.querySelectorAll('.current-biz-name').forEach(s => s.innerText = selectedText);

          items.forEach(i => {
            i.classList.remove('text-[#0069A1]', 'bg-blue-50/60', 'font-semibold');
            i.classList.add('text-gray-700', 'font-medium');
          });
          item.classList.remove('text-gray-700', 'font-medium');
          item.classList.add('text-[#0069A1]', 'bg-blue-50/60', 'font-semibold');

          menu.classList.add('hidden');
          if (chevron) chevron.style.transform = 'rotate(0deg)';
        });
      });
    }
  });

  // Global Close Dropdowns when clicking outside
  function closeAllDropdowns() {
    if (profileMenuDropdown) profileMenuDropdown.classList.add('hidden');
    document.querySelectorAll('.biz-dropdown-menu').forEach(m => m.classList.add('hidden'));
    document.querySelectorAll('.chevron-icon').forEach(c => c.style.transform = 'rotate(0deg)');
  }

  document.addEventListener('click', () => {
    closeAllDropdowns();
  });

  // 5. Scroll Widget Interactivity
  const scrollWidget = document.querySelector('.fixed.right-3');
  if (scrollWidget) {
    scrollWidget.classList.remove('opacity-0', 'translate-x-4', 'pointer-events-none');
    scrollWidget.classList.add('opacity-100', 'translate-x-0');

    const scrollTopBtn = scrollWidget.querySelector('button[title="Scroll to top"]');
    const scrollBottomBtn = scrollWidget.querySelector('button[title="Scroll to bottom"]');
    const progressBar = scrollWidget.querySelector('.bg-\\[\\#0069A1\\]');

    if (scrollTopBtn) {
      scrollTopBtn.addEventListener('click', () => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
      });
    }

    if (scrollBottomBtn) {
      scrollBottomBtn.addEventListener('click', () => {
        window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
      });
    }

    window.addEventListener('scroll', () => {
      const totalScroll = document.documentElement.scrollHeight - window.innerHeight;
      if (totalScroll > 0 && progressBar) {
        const progress = (window.scrollY / totalScroll) * 100;
        progressBar.style.height = `${Math.min(100, Math.max(0, progress))}%`;
      }
    });
  }
}
