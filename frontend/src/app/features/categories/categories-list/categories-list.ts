import { Component, computed, inject, resource, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { firstValueFrom } from 'rxjs';

import { ReferenceService } from '../../../core/services/reference.service';
import { Input } from '../../../shared/components/input/input';
import { Pagination } from '../../../shared/components/pagination/pagination';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { EmptyState } from '../../../shared/components/empty-state/empty-state';

@Component({
  selector: 'app-categories-list',
  imports: [Input, Pagination, Spinner, EmptyState, DatePipe],
  template: `
    <div class="categories">
      <header class="categories__header">
        <div>
          <h1 class="categories__title">Categorías</h1>
          <p class="categories__subtitle">Clasifica tus gastos</p>
        </div>
      </header>

      <section class="categories__search" aria-label="Búsqueda">
        <app-input
          type="text"
          label="Buscar"
          placeholder="Buscar por nombre o código"
          [value]="search()"
          (valueChange)="onSearchChange($event)"
        />
      </section>

      @if (isLoading()) {
        <div class="categories__loading">
          <app-spinner size="lg" />
        </div>
      } @else if (hasError()) {
        <app-empty-state
          title="No se pudieron cargar las categorías"
          description="Ocurrió un error al consultar las categorías."
        />
      } @else if (items().length === 0) {
        <app-empty-state
          title="Sin resultados"
          description="No se encontraron categorías con los criterios actuales."
        />
      } @else {
        <section class="categories__table-wrap">
          <table class="categories__table">
            <thead>
              <tr>
                <th>Código</th>
                <th>Nombre</th>
                <th>Creada</th>
              </tr>
            </thead>
            <tbody>
              @for (category of items(); track category.id) {
                <tr>
                  <td>
                    <span class="categories__code">{{ category.code }}</span>
                  </td>
                  <td>{{ category.name }}</td>
                  <td>{{ category.createdAt | date: 'dd MMM yyyy' }}</td>
                </tr>
              }
            </tbody>
          </table>
        </section>

        <div class="categories__cards" aria-label="Categorías">
          @for (category of items(); track category.id) {
            <article class="category-card">
              <div class="category-card__head">
                <span class="category-card__code">{{ category.code }}</span>
                <span class="category-card__name">{{ category.name }}</span>
              </div>
              <span class="category-card__created">
                Creada: {{ category.createdAt | date: 'dd MMM yyyy' }}
              </span>
            </article>
          }
        </div>

        <app-pagination
          [page]="page()"
          [size]="size()"
          [totalItems]="totalItems()"
          (pageChange)="onPageChange($event)"
        />
      }
    </div>
  `,
  styles: [
    `
      .categories {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-5);
      }
      .categories__header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: var(--le-space-4);
      }
      .categories__title {
        font-size: var(--le-fs-2xl);
      }
      .categories__subtitle {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
        margin-top: var(--le-space-1);
      }
      .categories__search {
        max-width: 22rem;
      }
      .categories__loading {
        display: flex;
        justify-content: center;
        padding: var(--le-space-10);
        color: var(--le-primary);
      }

      .categories__table-wrap {
        display: none;
        overflow-x: auto;
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .categories__table {
        width: 100%;
        border-collapse: collapse;
        font-size: var(--le-fs-sm);
      }
      .categories__table th {
        text-align: left;
        padding: var(--le-space-3) var(--le-space-4);
        font-size: var(--le-fs-xs);
        text-transform: uppercase;
        letter-spacing: 0.04em;
        color: var(--le-text-subtle);
        border-bottom: 1px solid var(--le-border);
        white-space: nowrap;
      }
      .categories__table td {
        padding: var(--le-space-3) var(--le-space-4);
        border-bottom: 1px solid var(--le-border);
      }
      .categories__table tbody tr:last-child td {
        border-bottom: none;
      }
      .categories__table tbody tr:hover {
        background: var(--le-surface-hover);
      }
      .categories__code {
        font-family: var(--le-font-mono, monospace);
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }

      .categories__cards {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-3);
      }
      .category-card {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-1);
        padding: var(--le-space-4);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .category-card__head {
        display: flex;
        align-items: center;
        gap: var(--le-space-3);
      }
      .category-card__code {
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }
      .category-card__name {
        font-weight: var(--le-fw-semibold);
        color: var(--le-text);
      }
      .category-card__created {
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }

      @media (min-width: 769px) {
        .categories__table-wrap {
          display: block;
        }
        .categories__cards {
          display: none;
        }
      }
    `,
  ],
})
export class CategoriesList {
  private readonly referenceService = inject(ReferenceService);

  protected readonly search = signal('');
  protected readonly page = signal(1);
  protected readonly size = signal(10);

  protected readonly listResource = resource({
    loader: () =>
      firstValueFrom(
        this.referenceService.getCategories({
          search: this.search() || undefined,
          page: this.page(),
          size: this.size(),
        }),
      ),
  });

  protected readonly items = computed(() => this.listResource.value()?.items ?? []);
  protected readonly totalItems = computed(
    () => this.listResource.value()?.totalItems ?? 0,
  );
  protected readonly hasError = computed(() => !!this.listResource.error());
  protected readonly isLoading = computed(
    () =>
      this.listResource.isLoading() || this.listResource.status() === 'idle',
  );

  protected onSearchChange(value: string): void {
    this.search.set(value);
    this.page.set(1);
  }

  protected onPageChange(page: number): void {
    this.page.set(page);
  }
}
