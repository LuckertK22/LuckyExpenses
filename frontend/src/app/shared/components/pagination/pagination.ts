import { Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  template: `
    @if (totalItems() > 0) {
      <nav class="le-pagination" aria-label="Paginación">
        <p class="le-pagination__info">{{ info() }}</p>
        <div class="le-pagination__controls">
          <button
            class="le-pagination__btn"
            type="button"
            aria-label="Página anterior"
            [disabled]="page() <= 1"
            (click)="goTo(page() - 1)"
          >
            &lsaquo;
          </button>
          @for (item of pages(); track item) {
            @if (item < 0) {
              <span class="le-pagination__ellipsis">&hellip;</span>
            } @else {
              <button
                class="le-pagination__btn"
                [class.le-pagination__btn--active]="item === page()"
                type="button"
                [attr.aria-current]="item === page() ? 'page' : null"
                (click)="goTo(item)"
              >
                {{ item }}
              </button>
            }
          }
          <button
            class="le-pagination__btn"
            type="button"
            aria-label="Página siguiente"
            [disabled]="page() >= totalPages()"
            (click)="goTo(page() + 1)"
          >
            &rsaquo;
          </button>
        </div>
      </nav>
    }
  `,
  styles: [
    `
      .le-pagination {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: var(--le-space-3);
        padding: var(--le-space-4) 0;
      }
      .le-pagination__info {
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }
      .le-pagination__controls {
        display: flex;
        align-items: center;
        gap: var(--le-space-1);
        flex-wrap: wrap;
      }
      .le-pagination__btn {
        min-width: 2.25rem;
        height: 2.25rem;
        padding: 0 var(--le-space-2);
        display: inline-flex;
        align-items: center;
        justify-content: center;
        font-family: inherit;
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
        background: transparent;
        border: 1px solid transparent;
        border-radius: var(--le-radius);
        cursor: pointer;
        transition:
          background-color var(--le-transition),
          color var(--le-transition),
          border-color var(--le-transition);
      }
      .le-pagination__btn:hover:not(:disabled):not(.le-pagination__btn--active) {
        background: var(--le-surface-hover);
        color: var(--le-text);
      }
      .le-pagination__btn--active {
        background: var(--le-primary);
        color: var(--le-primary-contrast);
        font-weight: var(--le-fw-semibold);
      }
      .le-pagination__btn:disabled {
        opacity: 0.4;
        cursor: not-allowed;
      }
      .le-pagination__ellipsis {
        min-width: 1.5rem;
        text-align: center;
        color: var(--le-text-subtle);
        user-select: none;
      }
    `,
  ],
})
export class Pagination {
  page = input<number>(1);
  size = input<number>(10);
  totalItems = input<number>(0);

  readonly pageChange = output<number>();

  protected readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalItems() / Math.max(1, this.size()))),
  );

  protected readonly info = computed(() => {
    const total = this.totalItems();
    if (total === 0) {
      return '';
    }
    const first = (this.page() - 1) * this.size() + 1;
    const last = Math.min(this.page() * this.size(), total);
    return `${first}–${last} de ${total}`;
  });

  protected readonly pages = computed(() => {
    const current = this.page();
    const total = this.totalPages();
    const window = 1;

    if (total <= 7) {
      return Array.from({ length: total }, (_, i) => i + 1);
    }

    const set = new Set<number>([1, total]);
    for (let p = current - window; p <= current + window; p++) {
      if (p > 1 && p < total) {
        set.add(p);
      }
    }

    const sorted = [...set].sort((a, b) => a - b);
    const result: number[] = [];
    let prev = 0;
    for (const p of sorted) {
      if (prev !== 0 && p - prev > 1) {
        result.push(-1);
      }
      result.push(p);
      prev = p;
    }
    return result;
  });

  protected goTo(page: number): void {
    if (page < 1 || page > this.totalPages() || page === this.page()) {
      return;
    }
    this.pageChange.emit(page);
  }
}
