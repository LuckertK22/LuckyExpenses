import { Component, HostListener, input, output } from '@angular/core';

@Component({
  selector: 'app-modal',
  template: `
    @if (open()) {
      <div class="le-modal-backdrop" (click)="close()">
        <div
          class="le-modal"
          role="dialog"
          aria-modal="true"
          [attr.aria-labelledby]="titleId"
          (click)="$event.stopPropagation()"
        >
          <header class="le-modal__header">
            <h2 class="le-modal__title" [id]="titleId">{{ title() }}</h2>
            <button
              class="le-modal__close"
              type="button"
              aria-label="Cerrar"
              (click)="close()"
            >
              <svg viewBox="0 0 16 16" width="16" height="16" aria-hidden="true">
                <path
                  fill="currentColor"
                  d="m4.4 3.4 3.6 3.5 3.6-3.5a.9.9 0 1 1 1.3 1.3L9.3 8.2l3.6 3.5a.9.9 0 1 1-1.3 1.3L8 9.5l-3.6 3.5a.9.9 0 1 1-1.3-1.3l3.6-3.5-3.6-3.5a.9.9 0 1 1 1.3-1.3Z"
                />
              </svg>
            </button>
          </header>
          <div class="le-modal__body">
            <ng-content />
          </div>
        </div>
      </div>
    }
  `,
  styles: [
    `
      :host {
        display: contents;
      }
      .le-modal-backdrop {
        position: fixed;
        inset: 0;
        z-index: var(--le-z-modal-backdrop);
        display: flex;
        align-items: center;
        justify-content: center;
        padding: var(--le-space-4);
        background: rgb(0 0 0 / 0.5);
        animation: le-fade-in 160ms ease-out;
      }
      .le-modal {
        width: min(28rem, 100%);
        max-height: calc(100vh - 2rem);
        display: flex;
        flex-direction: column;
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-lg);
        box-shadow: var(--le-shadow-lg);
        animation: le-pop-in 180ms cubic-bezier(0.4, 0, 0.2, 1);
      }
      .le-modal__header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: var(--le-space-3);
        padding: var(--le-space-4) var(--le-space-5);
        border-bottom: 1px solid var(--le-border);
      }
      .le-modal__title {
        font-size: var(--le-fs-lg);
      }
      .le-modal__close {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 2rem;
        height: 2rem;
        border: none;
        border-radius: var(--le-radius);
        background: transparent;
        color: var(--le-text-subtle);
        cursor: pointer;
        transition:
          color var(--le-transition),
          background-color var(--le-transition);
      }
      .le-modal__close:hover {
        color: var(--le-text);
        background: var(--le-surface-hover);
      }
      .le-modal__body {
        padding: var(--le-space-5);
        overflow-y: auto;
      }
      @keyframes le-fade-in {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }
      @keyframes le-pop-in {
        from {
          opacity: 0;
          transform: scale(0.97);
        }
        to {
          opacity: 1;
          transform: scale(1);
        }
      }
    `,
  ],
})
export class Modal {
  open = input(false);
  title = input<string>('');

  readonly closed = output<void>();

  protected readonly titleId = 'le-modal-title';

  @HostListener('document:keydown.escape')
  onEscape(): void {
    if (this.open()) {
      this.close();
    }
  }

  close(): void {
    this.closed.emit();
  }
}
