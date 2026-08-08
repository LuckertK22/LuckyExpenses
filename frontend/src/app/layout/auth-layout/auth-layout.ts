import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-auth-layout',
  imports: [RouterOutlet],
  template: `
    <div class="auth-layout">
      <div class="auth-layout__card">
        <div class="auth-layout__brand">
          <div class="auth-layout__logo" aria-hidden="true">
            <svg viewBox="0 0 24 24" width="26" height="26">
              <path
                fill="currentColor"
                d="M19 4H5a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2Zm0 14H5V6h14v12Zm-3.5-3.5a5 5 0 0 0-9-2.5l1.5 1.2a3.2 3.2 0 0 1 5.6 1.8h-2L14.8 18l3.2-.8-2.5-.7Zm-3 3.5a5 5 0 0 0 9-2.5l-1.5-1.2a3.2 3.2 0 0 1-5.6-1.8h2L9.2 16l-3.2.8 2.5.7Z"
              />
            </svg>
          </div>
          <h1 class="auth-layout__name">LuckyExpenses</h1>
          <p class="auth-layout__tagline">Controla y visualiza tus gastos</p>
        </div>
        <router-outlet />
      </div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
      }
      .auth-layout {
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: var(--le-space-4);
        background: var(--le-bg);
      }
      .auth-layout__card {
        width: min(26rem, 100%);
        display: flex;
        flex-direction: column;
        gap: var(--le-space-6);
        padding: var(--le-space-8) var(--le-space-7);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-lg);
        box-shadow: var(--le-shadow-md);
      }
      .auth-layout__brand {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: var(--le-space-1);
        text-align: center;
      }
      .auth-layout__logo {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 3rem;
        height: 3rem;
        border-radius: var(--le-radius-lg);
        background: var(--le-primary-soft);
        color: var(--le-primary);
        margin-bottom: var(--le-space-1);
      }
      .auth-layout__name {
        font-size: var(--le-fs-2xl);
        font-weight: var(--le-fw-bold);
        color: var(--le-text);
      }
      .auth-layout__tagline {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
      }
    `,
  ],
})
export class AuthLayout {}
