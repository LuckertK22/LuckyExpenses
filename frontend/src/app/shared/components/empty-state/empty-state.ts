import { Component, input } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  template: `
    <div class="le-empty">
      <div class="le-empty__icon" aria-hidden="true">
        <svg viewBox="0 0 24 24" width="32" height="32">
          <path
            fill="currentColor"
            d="M19 4H5a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2Zm0 14H5V6h14v12Zm-3.5-3.5a5 5 0 0 0-9-2.5l1.5 1.2a3.2 3.2 0 0 1 5.6 1.8h-2L14.8 18l3.2-.8-2.5-.7Zm-3 3.5a5 5 0 0 0 9-2.5l-1.5-1.2a3.2 3.2 0 0 1-5.6-1.8h2L9.2 16l-3.2.8 2.5.7Z"
          />
        </svg>
      </div>
      <h3 class="le-empty__title">{{ title() }}</h3>
      @if (description()) {
        <p class="le-empty__description">{{ description() }}</p>
      }
      <div class="le-empty__actions">
        <ng-content />
      </div>
    </div>
  `,
  styles: [
    `
      .le-empty {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        text-align: center;
        gap: var(--le-space-2);
        padding: var(--le-space-10) var(--le-space-6);
      }
      .le-empty__icon {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 4rem;
        height: 4rem;
        border-radius: var(--le-radius-lg);
        background: var(--le-surface-2);
        color: var(--le-text-subtle);
        margin-bottom: var(--le-space-2);
      }
      .le-empty__title {
        font-size: var(--le-fs-lg);
        color: var(--le-text);
      }
      .le-empty__description {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
        max-width: 24rem;
      }
      .le-empty__actions {
        display: flex;
        gap: var(--le-space-3);
        margin-top: var(--le-space-3);
      }
    `,
  ],
})
export class EmptyState {
  title = input<string>('');
  description = input<string>('');
}
