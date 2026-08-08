import { Component, computed, input } from '@angular/core';
import { NgClass } from '@angular/common';

import { Spinner } from '../spinner/spinner';

export type ButtonVariant = 'primary' | 'secondary' | 'ghost' | 'danger';
export type ButtonSize = 'sm' | 'md' | 'lg';

@Component({
  selector: 'app-button',
  imports: [NgClass, Spinner],
  template: `
    <button
      class="le-btn"
      [ngClass]="['le-btn--' + variant(), 'le-btn--' + size()]"
      [class.le-btn--block]="block()"
      [attr.type]="type()"
      [disabled]="disabled() || loading()"
    >
      @if (loading()) {
        <app-spinner class="le-btn__spinner" size="sm" />
      }
      <ng-content />
    </button>
  `,
  styles: [
    `
      .le-btn {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        gap: var(--le-space-2);
        font-family: inherit;
        font-weight: var(--le-fw-semibold);
        line-height: var(--le-lh-tight);
        border-radius: var(--le-radius);
        border: 1px solid transparent;
        cursor: pointer;
        transition:
          background-color var(--le-transition),
          border-color var(--le-transition),
          color var(--le-transition),
          box-shadow var(--le-transition);
      }
      .le-btn:disabled {
        opacity: 0.55;
        cursor: not-allowed;
      }
      .le-btn__spinner {
        color: currentColor;
      }

      .le-btn--sm {
        font-size: var(--le-fs-sm);
        padding: 0.375rem 0.75rem;
      }
      .le-btn--md {
        font-size: var(--le-fs-base);
        padding: 0.5625rem 1.25rem;
      }
      .le-btn--lg {
        font-size: var(--le-fs-md);
        padding: 0.75rem 1.75rem;
      }
      .le-btn--block {
        width: 100%;
      }

      .le-btn--primary {
        background: var(--le-primary);
        color: var(--le-primary-contrast);
      }
      .le-btn--primary:hover:not(:disabled) {
        background: var(--le-primary-hover);
      }
      .le-btn--primary:active:not(:disabled) {
        background: var(--le-primary-active);
      }

      .le-btn--secondary {
        background: var(--le-surface);
        color: var(--le-text);
        border-color: var(--le-border-strong);
      }
      .le-btn--secondary:hover:not(:disabled) {
        background: var(--le-surface-hover);
        border-color: var(--le-text-subtle);
      }

      .le-btn--ghost {
        background: transparent;
        color: var(--le-text-muted);
      }
      .le-btn--ghost:hover:not(:disabled) {
        background: var(--le-surface-hover);
        color: var(--le-text);
      }

      .le-btn--danger {
        background: var(--le-danger);
        color: #fff;
      }
      .le-btn--danger:hover:not(:disabled) {
        filter: brightness(1.08);
      }
    `,
  ],
})
export class Button {
  variant = input<ButtonVariant>('primary');
  size = input<ButtonSize>('md');
  type = input<'button' | 'submit' | 'reset'>('button');
  loading = input(false);
  disabled = input(false);
  block = input(false);
}
