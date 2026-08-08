import { Component, inject } from '@angular/core';

import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-toast-host',
  template: `
    <div class="le-toast-host" aria-live="polite" aria-atomic="false">
      @for (toast of toastService.toasts(); track toast.id) {
        <div class="le-toast" [class]="'le-toast--' + toast.kind" role="status">
          <span class="le-toast__icon" aria-hidden="true">
            @switch (toast.kind) {
              @case ('success') {
                <svg viewBox="0 0 20 20" width="18" height="18">
                  <path
                    fill="currentColor"
                    d="M10 1a9 9 0 1 0 0 18 9 9 0 0 0 0-18Zm4.2 6.6-4.8 5.4a.9.9 0 0 1-1.3 0l-2.3-2.5a.9.9 0 1 1 1.3-1.2l1.7 1.9 4.1-4.7a.9.9 0 1 1 1.3 1.1Z"
                  />
                </svg>
              }
              @case ('error') {
                <svg viewBox="0 0 20 20" width="18" height="18">
                  <path
                    fill="currentColor"
                    d="M10 1a9 9 0 1 0 0 18 9 9 0 0 0 0-18Zm0 5a1 1 0 0 1 1 1v4a1 1 0 1 1-2 0V7a1 1 0 0 1 1-1Zm0 8.2a1.2 1.2 0 1 1 0-2.4 1.2 1.2 0 0 1 0 2.4Z"
                  />
                </svg>
              }
              @case ('warning') {
                <svg viewBox="0 0 20 20" width="18" height="18">
                  <path
                    fill="currentColor"
                    d="M8.6 2.5a1.9 1.9 0 0 1 2.8 0l6.9 7.7a1.9 1.9 0 0 1-1.4 3.2H3.1a1.9 1.9 0 0 1-1.4-3.2l6.9-7.7ZM10 6.5a1 1 0 0 0-1 1v2.3a1 1 0 1 0 2 0V7.5a1 1 0 0 0-1-1Zm0 5.6a1.1 1.1 0 1 0 0 2.2 1.1 1.1 0 0 0 0-2.2Z"
                  />
                </svg>
              }
              @case ('info') {
                <svg viewBox="0 0 20 20" width="18" height="18">
                  <path
                    fill="currentColor"
                    d="M10 1a9 9 0 1 0 0 18 9 9 0 0 0 0-18Zm0 5.2a1.2 1.2 0 1 1 0 2.4 1.2 1.2 0 0 1 0-2.4Zm1 7.6a1 1 0 1 1-2 0v-3.5a1 1 0 1 1 2 0V13.8Z"
                  />
                </svg>
              }
            }
          </span>
          <p class="le-toast__message">{{ toast.message }}</p>
          <button class="le-toast__close" type="button" (click)="dismiss(toast.id)">
            <span class="sr-only">Cerrar</span>
            <svg viewBox="0 0 16 16" width="14" height="14" aria-hidden="true">
              <path
                fill="currentColor"
                d="m4.4 3.4 3.6 3.5 3.6-3.5a.9.9 0 1 1 1.3 1.3L9.3 8.2l3.6 3.5a.9.9 0 1 1-1.3 1.3L8 9.5l-3.6 3.5a.9.9 0 1 1-1.3-1.3l3.6-3.5-3.6-3.5a.9.9 0 1 1 1.3-1.3Z"
              />
            </svg>
          </button>
        </div>
      }
    </div>
  `,
  styles: [
    `
      :host {
        position: fixed;
        top: var(--le-space-4);
        right: var(--le-space-4);
        z-index: var(--le-z-toast);
      }
      .le-toast-host {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-2);
        width: min(22rem, calc(100vw - 2rem));
      }
      .le-toast {
        display: flex;
        align-items: flex-start;
        gap: var(--le-space-3);
        padding: var(--le-space-3) var(--le-space-4);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-left-width: 3px;
        border-radius: var(--le-radius);
        box-shadow: var(--le-shadow-md);
      }
      .le-toast__icon {
        display: flex;
        flex-shrink: 0;
        margin-top: 1px;
      }
      .le-toast--success {
        border-left-color: var(--le-success);
      }
      .le-toast--success .le-toast__icon {
        color: var(--le-success);
      }
      .le-toast--error {
        border-left-color: var(--le-danger);
      }
      .le-toast--error .le-toast__icon {
        color: var(--le-danger);
      }
      .le-toast--warning {
        border-left-color: var(--le-warning);
      }
      .le-toast--warning .le-toast__icon {
        color: var(--le-warning);
      }
      .le-toast--info {
        border-left-color: var(--le-info);
      }
      .le-toast--info .le-toast__icon {
        color: var(--le-info);
      }
      .le-toast__message {
        flex: 1;
        font-size: var(--le-fs-sm);
        line-height: var(--le-lh-normal);
        color: var(--le-text);
      }
      .le-toast__close {
        display: flex;
        flex-shrink: 0;
        padding: 0.25rem;
        border: none;
        border-radius: var(--le-radius-sm);
        background: transparent;
        color: var(--le-text-subtle);
        cursor: pointer;
        transition:
          color var(--le-transition),
          background-color var(--le-transition);
      }
      .le-toast__close:hover {
        color: var(--le-text);
        background: var(--le-surface-hover);
      }
    `,
  ],
})
export class ToastHost {
  readonly toastService = inject(ToastService);

  dismiss(id: number): void {
    this.toastService.dismiss(id);
  }
}
