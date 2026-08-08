import { Component, HostListener, input, output } from '@angular/core';

import { Button } from '../button/button';

@Component({
  selector: 'app-confirm-dialog',
  imports: [Button],
  template: `
    @if (open()) {
      <div class="le-dialog-backdrop" (click)="cancel()">
        <div
          class="le-dialog"
          role="dialog"
          aria-modal="true"
          [attr.aria-labelledby]="titleId"
          (click)="$event.stopPropagation()"
        >
          <div class="le-dialog__icon" [class]="'le-dialog__icon--' + variant()" aria-hidden="true">
            @if (variant() === 'danger') {
              <svg viewBox="0 0 24 24" width="24" height="24">
                <path
                  fill="currentColor"
                  d="M12 2 1 21h22L12 2Zm0 6a1.3 1.3 0 0 1 1.3 1.3l-.5 5.2a.8.8 0 0 1-1.6 0l-.5-5.2A1.3 1.3 0 0 1 12 8Zm0 9.5a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3Z"
                />
              </svg>
            } @else {
              <svg viewBox="0 0 24 24" width="24" height="24">
                <path
                  fill="currentColor"
                  d="M12 2a10 10 0 1 0 0 20 10 10 0 0 0 0-20Zm1 15h-2v-6h2v6Zm0-8h-2V7h2v2Z"
                />
              </svg>
            }
          </div>
          <h2 class="le-dialog__title" [id]="titleId">{{ title() }}</h2>
          @if (message()) {
            <p class="le-dialog__message">{{ message() }}</p>
          }
          <div class="le-dialog__actions">
            <app-button variant="ghost" (click)="cancel()">{{ cancelLabel() }}</app-button>
            <app-button [variant]="variant()" [loading]="loading()" (click)="confirm()">
              {{ confirmLabel() }}
            </app-button>
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
      .le-dialog-backdrop {
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
      .le-dialog {
        width: min(24rem, 100%);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-lg);
        box-shadow: var(--le-shadow-lg);
        padding: var(--le-space-6);
        display: flex;
        flex-direction: column;
        align-items: center;
        text-align: center;
        gap: var(--le-space-3);
        animation: le-pop-in 180ms cubic-bezier(0.4, 0, 0.2, 1);
      }
      .le-dialog__icon {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 3rem;
        height: 3rem;
        border-radius: 50%;
      }
      .le-dialog__icon--danger {
        color: var(--le-danger);
        background: var(--le-danger-soft);
      }
      .le-dialog__icon--primary {
        color: var(--le-primary);
        background: var(--le-primary-soft);
      }
      .le-dialog__title {
        font-size: var(--le-fs-lg);
      }
      .le-dialog__message {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
      }
      .le-dialog__actions {
        display: flex;
        gap: var(--le-space-3);
        width: 100%;
        margin-top: var(--le-space-2);
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
          transform: scale(0.96);
        }
        to {
          opacity: 1;
          transform: scale(1);
        }
      }
    `,
  ],
})
export class ConfirmDialog {
  open = input(false);
  title = input<string>('Confirmar');
  message = input<string>('');
  confirmLabel = input<string>('Confirmar');
  cancelLabel = input<string>('Cancelar');
  variant = input<'primary' | 'danger'>('primary');
  loading = input(false);

  readonly confirmed = output<void>();
  readonly cancelled = output<void>();

  protected readonly titleId = 'le-dialog-title';

  @HostListener('document:keydown.escape')
  onEscape(): void {
    if (this.open()) {
      this.cancel();
    }
  }

  confirm(): void {
    this.confirmed.emit();
  }

  cancel(): void {
    this.cancelled.emit();
  }
}
