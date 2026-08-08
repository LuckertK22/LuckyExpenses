import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { ThemeService } from '../../core/services/theme.service';
import { Button } from '../../shared/components/button/button';

interface NavItem {
  label: string;
  route: string;
  icon: string;
}

@Component({
  selector: 'app-authenticated-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, Button],
  template: `
    <div class="app-layout">
      <aside
        class="app-layout__sidebar"
        [class.app-layout__sidebar--open]="sidebarOpen()"
        [class.app-layout__sidebar--hidden]="!sidebarOpen()"
      >
        <div class="sidebar__brand">
          <div class="sidebar__logo" aria-hidden="true">
            <svg viewBox="0 0 24 24" width="22" height="22">
              <path
                fill="currentColor"
                d="M19 4H5a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2Zm0 14H5V6h14v12Zm-3.5-3.5a5 5 0 0 0-9-2.5l1.5 1.2a3.2 3.2 0 0 1 5.6 1.8h-2L14.8 18l3.2-.8-2.5-.7Zm-3 3.5a5 5 0 0 0 9-2.5l-1.5-1.2a3.2 3.2 0 0 1-5.6-1.8h2L9.2 16l-3.2.8 2.5.7Z"
              />
            </svg>
          </div>
          <span class="sidebar__name">LuckyExpenses</span>
        </div>

        <nav class="sidebar__nav" aria-label="Navegación principal">
          @for (item of navItems; track item.route) {
            <a
              class="sidebar__link"
              routerLink="{{ item.route }}"
              routerLinkActive="sidebar__link--active"
              (click)="closeSidebar()"
            >
              <svg class="sidebar__link-icon" viewBox="0 0 24 24" width="18" height="18" aria-hidden="true">
                <path fill="currentColor" [attr.d]="item.icon" />
              </svg>
              <span>{{ item.label }}</span>
            </a>
          }
        </nav>

        <div class="sidebar__footer">
          <div class="sidebar__user">
            <span class="sidebar__user-avatar" aria-hidden="true">
              {{ initials() }}
            </span>
            <span class="sidebar__user-email">{{ userEmail() }}</span>
          </div>
          <app-button
            variant="ghost"
            size="sm"
            (click)="logout()"
            class="sidebar__logout"
          >
            Cerrar sesión
          </app-button>
        </div>
      </aside>

      @if (sidebarOpen()) {
        <div class="app-layout__backdrop" (click)="closeSidebar()"></div>
      }

      <div class="app-layout__main">
        <header class="topbar">
          <button
            class="topbar__menu"
            type="button"
            aria-label="Abrir menú"
            (click)="toggleSidebar()"
          >
            <svg viewBox="0 0 24 24" width="20" height="20" aria-hidden="true">
              <path
                fill="none"
                stroke="currentColor"
                stroke-linecap="round"
                stroke-width="2"
                d="M4 6h16M4 12h16M4 18h16"
              />
            </svg>
          </button>

          <button
            class="topbar__theme"
            type="button"
            [attr.aria-label]="themeLabel()"
            (click)="themeService.toggle()"
          >
            @if (isDark()) {
              <svg viewBox="0 0 24 24" width="18" height="18" aria-hidden="true">
                <path
                  fill="currentColor"
                  d="M12 7a5 5 0 1 0 0 10 5 5 0 0 0 0-10Zm0 8a3 3 0 1 1 0-6 3 3 0 0 1 0 6Zm0-13a1 1 0 0 1 1 1v1a1 1 0 1 1-2 0V3a1 1 0 0 1 1-1Zm0 16a1 1 0 0 1 1 1v1a1 1 0 1 1-2 0v-1a1 1 0 0 1 1-1ZM4 11h1a1 1 0 1 1 0 2H4a1 1 0 1 1 0-2Zm15 0h1a1 1 0 1 1 0 2h-1a1 1 0 1 1 0-2ZM6.3 6.3a1 1 0 0 1 1.4 0l.7.7a1 1 0 1 1-1.4 1.4l-.7-.7a1 1 0 0 1 0-1.4Zm9.9 9.9a1 1 0 0 1 1.4 0l.7.7a1 1 0 1 1-1.4 1.4l-.7-.7a1 1 0 0 1 0-1.4Zm-11.3 1.4.7-.7a1 1 0 1 1 1.4 1.4l-.7.7a1 1 0 0 1-1.4-1.4Zm9.9-9.9.7-.7a1 1 0 1 1 1.4 1.4l-.7.7a1 1 0 1 1-1.4-1.4Z"
                />
              </svg>
            } @else {
              <svg viewBox="0 0 24 24" width="18" height="18" aria-hidden="true">
                <path
                  fill="currentColor"
                  d="M21.6 13.6A9 9 0 0 1 10.4 2.4 9 9 0 1 0 21.6 13.6Z"
                />
              </svg>
            }
          </button>
        </header>

        <main class="app-layout__content">
          <router-outlet />
        </main>
      </div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
      }
      .app-layout {
        display: flex;
        min-height: 100vh;
        background: var(--le-bg);
      }

      /* ===== Sidebar ===== */
      .app-layout__sidebar {
        position: fixed;
        inset-block: 0;
        left: 0;
        z-index: var(--le-z-drawer);
        display: flex;
        flex-direction: column;
        width: var(--le-sidebar-w);
        background: var(--le-surface);
        border-right: 1px solid var(--le-border);
        transform: translateX(-100%);
        transition: transform var(--le-transition-slow);
      }
      .app-layout__sidebar--open {
        transform: translateX(0);
      }
      .app-layout__backdrop {
        position: fixed;
        inset: 0;
        z-index: calc(var(--le-z-drawer) - 1);
        background: rgb(0 0 0 / 0.45);
      }

      .sidebar__brand {
        display: flex;
        align-items: center;
        gap: var(--le-space-3);
        padding: var(--le-space-5) var(--le-space-4);
        border-bottom: 1px solid var(--le-border);
      }
      .sidebar__logo {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 2.25rem;
        height: 2.25rem;
        border-radius: var(--le-radius);
        background: var(--le-primary-soft);
        color: var(--le-primary);
      }
      .sidebar__name {
        font-size: var(--le-fs-md);
        font-weight: var(--le-fw-bold);
        color: var(--le-text);
      }

      .sidebar__nav {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-1);
        padding: var(--le-space-4);
        flex: 1;
      }
      .sidebar__link {
        display: flex;
        align-items: center;
        gap: var(--le-space-3);
        padding: var(--le-space-2) var(--le-space-3);
        border-radius: var(--le-radius);
        color: var(--le-text-muted);
        text-decoration: none;
        font-size: var(--le-fs-base);
        font-weight: var(--le-fw-medium);
        transition:
          background-color var(--le-transition),
          color var(--le-transition);
      }
      .sidebar__link:hover {
        background: var(--le-surface-hover);
        color: var(--le-text);
        text-decoration: none;
      }
      .sidebar__link--active {
        background: var(--le-primary-soft);
        color: var(--le-primary);
        font-weight: var(--le-fw-semibold);
      }
      .sidebar__link-icon {
        flex-shrink: 0;
      }

      .sidebar__footer {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-2);
        padding: var(--le-space-4);
        border-top: 1px solid var(--le-border);
      }
      .sidebar__user {
        display: flex;
        align-items: center;
        gap: var(--le-space-3);
        min-width: 0;
      }
      .sidebar__user-avatar {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 2rem;
        height: 2rem;
        flex-shrink: 0;
        border-radius: 50%;
        background: var(--le-primary);
        color: var(--le-primary-contrast);
        font-size: var(--le-fs-xs);
        font-weight: var(--le-fw-bold);
        text-transform: uppercase;
      }
      .sidebar__user-email {
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
      .sidebar__logout {
        width: 100%;
        justify-content: flex-start;
      }

      /* ===== Main column ===== */
      .app-layout__main {
        flex: 1;
        min-width: 0;
        display: flex;
        flex-direction: column;
        margin-left: 0;
      }

      .topbar {
        position: sticky;
        top: 0;
        z-index: 10;
        display: flex;
        align-items: center;
        justify-content: space-between;
        height: var(--le-topbar-h);
        padding: 0 var(--le-space-4);
        background: color-mix(in srgb, var(--le-surface) 85%, transparent);
        backdrop-filter: blur(8px);
        border-bottom: 1px solid var(--le-border);
      }
      .topbar__menu,
      .topbar__theme {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 2.25rem;
        height: 2.25rem;
        border: none;
        border-radius: var(--le-radius);
        background: transparent;
        color: var(--le-text-muted);
        cursor: pointer;
        transition:
          background-color var(--le-transition),
          color var(--le-transition);
      }
      .topbar__menu:hover,
      .topbar__theme:hover {
        background: var(--le-surface-hover);
        color: var(--le-text);
      }

      .app-layout__content {
        flex: 1;
        width: 100%;
        max-width: var(--le-content-max);
        margin-inline: auto;
        padding: var(--le-space-6) var(--le-space-4);
      }

      /* ===== Desktop ===== */
      @media (min-width: 769px) {
        .app-layout__sidebar {
          position: sticky;
          top: 0;
          height: 100vh;
          flex-shrink: 0;
          transform: none;
        }
        .app-layout__main {
          margin-left: 0;
        }
        .topbar__menu {
          display: none;
        }
      }
    `,
  ],
})
export class AuthenticatedLayout {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  readonly themeService = inject(ThemeService);

  readonly navItems: NavItem[] = [
    {
      label: 'Dashboard',
      route: '/dashboard',
      icon: 'M4 13h6V4H4v9Zm0 7h6v-5H4v5Zm7 0h6v-9h-6v9Zm7 0h2v-5h-2v5Zm0-16v3h2V4h-2Zm-7 3h6v-3h-6v3Z',
    },
    {
      label: 'Gastos',
      route: '/expenses',
      icon: 'M12 2 3 7v10l9 5 9-5V7l-9-5Zm0 2.2 7 3.9v7.8l-7 3.9-7-3.9V8.1l7-3.9Zm-1 4.1v2.2H9V13h2v2.2h2V13h2v-2.5h-2V8.3h-2Z',
    },
    {
      label: 'Categorías',
      route: '/categories',
      icon: 'M12 2 4 7v10l8 5 8-5V7l-8-5Zm0 2.2 6 3.8v8l-6 3.8-6-3.8V8l6-3.8Zm-3 6.3v3l3 1.9 3-1.9v-3l-3-1.9-3 1.9Z',
    },
    {
      label: 'Métodos de pago',
      route: '/payment-methods',
      icon: 'M20 4H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2Zm0 14H4V6h16v12Zm-2-3a2 2 0 0 1-2 2h-4a2 2 0 0 1 0-4h4a2 2 0 0 1 2 2Zm0-8H6v3h12V7Z',
    },
  ];

  protected readonly sidebarOpen = signal(false);

  protected readonly isDark = this.themeService.theme;
  protected readonly themeLabel = () =>
    this.isDark() ? 'Cambiar a tema claro' : 'Cambiar a tema oscuro';

  protected readonly userEmail = () => this.authService.user()?.email ?? '';
  protected readonly initials = () => {
    const email = this.userEmail();
    if (!email) {
      return '';
    }
    const name = email.split('@')[0] ?? '';
    return name.slice(0, 2);
  };

  toggleSidebar(): void {
    this.sidebarOpen.update((open) => !open);
  }

  closeSidebar(): void {
    this.sidebarOpen.set(false);
  }

  logout(): void {
    this.authService.logout();
  }
}
