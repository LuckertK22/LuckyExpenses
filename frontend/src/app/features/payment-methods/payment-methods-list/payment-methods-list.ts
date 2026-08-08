import { Component, computed, inject, resource, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { firstValueFrom } from 'rxjs';

import { ReferenceService } from '../../../core/services/reference.service';
import { Input } from '../../../shared/components/input/input';
import { Pagination } from '../../../shared/components/pagination/pagination';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { EmptyState } from '../../../shared/components/empty-state/empty-state';

@Component({
  selector: 'app-payment-methods-list',
  imports: [Input, Pagination, Spinner, EmptyState, DatePipe],
  template: `
    <div class="payment-methods">
      <header class="payment-methods__header">
        <div>
          <h1 class="payment-methods__title">Métodos de pago</h1>
          <p class="payment-methods__subtitle">Formas de pago disponibles</p>
        </div>
      </header>

      <section class="payment-methods__search" aria-label="Búsqueda">
        <app-input
          type="text"
          label="Buscar"
          placeholder="Buscar por nombre o código"
          [value]="search()"
          (valueChange)="onSearchChange($event)"
        />
      </section>

      @if (isLoading()) {
        <div class="payment-methods__loading">
          <app-spinner size="lg" />
        </div>
      } @else if (hasError()) {
        <app-empty-state
          title="No se pudieron cargar los métodos de pago"
          description="Ocurrió un error al consultar los métodos de pago."
        />
      } @else if (items().length === 0) {
        <app-empty-state
          title="Sin resultados"
          description="No se encontraron métodos de pago con los criterios actuales."
        />
      } @else {
        <section class="payment-methods__table-wrap">
          <table class="payment-methods__table">
            <thead>
              <tr>
                <th>Código</th>
                <th>Nombre</th>
                <th>Creada</th>
              </tr>
            </thead>
            <tbody>
              @for (method of items(); track method.id) {
                <tr>
                  <td>
                    <span class="payment-methods__code">{{ method.code }}</span>
                  </td>
                  <td>{{ method.name }}</td>
                  <td>{{ method.createdAt | date: 'dd MMM yyyy' }}</td>
                </tr>
              }
            </tbody>
          </table>
        </section>

        <div class="payment-methods__cards" aria-label="Métodos de pago">
          @for (method of items(); track method.id) {
            <article class="payment-method-card">
              <div class="payment-method-card__head">
                <span class="payment-method-card__code">{{ method.code }}</span>
                <span class="payment-method-card__name">{{ method.name }}</span>
              </div>
              <span class="payment-method-card__created">
                Creada: {{ method.createdAt | date: 'dd MMM yyyy' }}
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
      .payment-methods {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-5);
      }
      .payment-methods__header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: var(--le-space-4);
      }
      .payment-methods__title {
        font-size: var(--le-fs-2xl);
      }
      .payment-methods__subtitle {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
        margin-top: var(--le-space-1);
      }
      .payment-methods__search {
        max-width: 22rem;
      }
      .payment-methods__loading {
        display: flex;
        justify-content: center;
        padding: var(--le-space-10);
        color: var(--le-primary);
      }

      .payment-methods__table-wrap {
        display: none;
        overflow-x: auto;
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .payment-methods__table {
        width: 100%;
        border-collapse: collapse;
        font-size: var(--le-fs-sm);
      }
      .payment-methods__table th {
        text-align: left;
        padding: var(--le-space-3) var(--le-space-4);
        font-size: var(--le-fs-xs);
        text-transform: uppercase;
        letter-spacing: 0.04em;
        color: var(--le-text-subtle);
        border-bottom: 1px solid var(--le-border);
        white-space: nowrap;
      }
      .payment-methods__table td {
        padding: var(--le-space-3) var(--le-space-4);
        border-bottom: 1px solid var(--le-border);
      }
      .payment-methods__table tbody tr:last-child td {
        border-bottom: none;
      }
      .payment-methods__table tbody tr:hover {
        background: var(--le-surface-hover);
      }
      .payment-methods__code {
        font-family: var(--le-font-mono, monospace);
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }

      .payment-methods__cards {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-3);
      }
      .payment-method-card {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-1);
        padding: var(--le-space-4);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .payment-method-card__head {
        display: flex;
        align-items: center;
        gap: var(--le-space-3);
      }
      .payment-method-card__code {
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }
      .payment-method-card__name {
        font-weight: var(--le-fw-semibold);
        color: var(--le-text);
      }
      .payment-method-card__created {
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }

      @media (min-width: 769px) {
        .payment-methods__table-wrap {
          display: block;
        }
        .payment-methods__cards {
          display: none;
        }
      }
    `,
  ],
})
export class PaymentMethodsList {
  private readonly referenceService = inject(ReferenceService);

  protected readonly search = signal('');
  protected readonly page = signal(1);
  protected readonly size = signal(10);

  protected readonly listResource = resource({
    loader: () =>
      firstValueFrom(
        this.referenceService.getPaymentMethods({
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
