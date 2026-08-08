import { Component, computed, inject, resource, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

import { ExpenseService } from '../../../core/services/expense.service';
import { ReferenceService } from '../../../core/services/reference.service';
import {
  ExpenseDto,
  ExpenseWithNamesDto,
  GetExpensesQuery,
} from '../../../core/models';
import { Button } from '../../../shared/components/button/button';
import { Input } from '../../../shared/components/input/input';
import { Select, SelectOption } from '../../../shared/components/select/select';
import { Pagination } from '../../../shared/components/pagination/pagination';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { EmptyState } from '../../../shared/components/empty-state/empty-state';
import { ConfirmDialog } from '../../../shared/components/confirm-dialog/confirm-dialog';
import { ExpenseFormModal } from '../expense-form-modal/expense-form-modal';

@Component({
  selector: 'app-expenses-list',
  imports: [
    Button,
    Input,
    Select,
    Pagination,
    Spinner,
    EmptyState,
    ConfirmDialog,
    ExpenseFormModal,
    CurrencyPipe,
    DatePipe,
  ],
  template: `
    <div class="expenses">
      <header class="expenses__header">
        <div>
          <h1 class="expenses__title">Gastos</h1>
          <p class="expenses__subtitle">Administra tus gastos</p>
        </div>
        <app-button (click)="openCreate()">Nuevo gasto</app-button>
      </header>

      <section class="expenses__filters" aria-label="Filtros">
        <app-input
          class="expenses__filter expenses__filter--title"
          label="Título"
          type="text"
          placeholder="Buscar por título"
          [value]="title()"
          (valueChange)="onTitleChange($event)"
        />
        <app-input
          class="expenses__filter"
          label="Desde"
          type="date"
          [value]="fromDate()"
          (valueChange)="onFromDateChange($event)"
        />
        <app-input
          class="expenses__filter"
          label="Hasta"
          type="date"
          [value]="toDate()"
          (valueChange)="onToDateChange($event)"
        />
        <app-select
          class="expenses__filter"
          label="Categoría"
          placeholder="Todas"
          [options]="categoryOptions()"
          [value]="categoryFilter()"
          (valueChange)="onCategoryChange($event)"
        />
        <app-select
          class="expenses__filter"
          label="Método de pago"
          placeholder="Todos"
          [options]="paymentMethodOptions()"
          [value]="paymentMethodFilter()"
          (valueChange)="onPaymentMethodChange($event)"
        />
        <app-button variant="ghost" (click)="clearFilters()">Limpiar</app-button>
      </section>

      @if (isLoading()) {
        <div class="expenses__loading">
          <app-spinner size="lg" />
        </div>
      } @else if (hasError()) {
        <app-empty-state
          title="No se pudieron cargar los gastos"
          description="Ocurrió un error al consultar los gastos."
        />
      } @else if (items().length === 0) {
        <app-empty-state
          title="Sin gastos"
          description="Crea tu primer gasto o ajusta los filtros."
        >
          <app-button (click)="openCreate()">Nuevo gasto</app-button>
        </app-empty-state>
      } @else {
        <section class="expenses__table-wrap">
          <table class="expenses__table">
            <thead>
              <tr>
                <th>Título</th>
                <th>Categoría</th>
                <th>Método de pago</th>
                <th>Fecha</th>
                <th class="expenses__th-amount">Monto</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              @for (expense of items(); track expense.id) {
                <tr>
                  <td>
                    <span class="expenses__title-cell">{{ expense.title }}</span>
                    @if (expense.description) {
                      <span class="expenses__description">{{ expense.description }}</span>
                    }
                  </td>
                  <td>{{ expense.categoryName }}</td>
                  <td>{{ expense.paymentMethodName ?? '—' }}</td>
                  <td>{{ expense.expenseDate | date: 'dd MMM yyyy' }}</td>
                  <td class="expenses__amount">{{ expense.amount | currency }}</td>
                  <td class="expenses__actions">
                    <button
                      class="expenses__action"
                      type="button"
                      (click)="openEdit(expense)"
                      aria-label="Editar"
                    >
                      <svg viewBox="0 0 24 24" width="16" height="16" aria-hidden="true">
                        <path
                          fill="currentColor"
                          d="M20.7 4.3a1 1 0 0 1 0 1.4l-1.2 1.2-2.4-2.4 1.2-1.2a1 1 0 0 1 1.4 0l1 1ZM4 20h2.4l11.5-11.5-2.4-2.4L4 17.6V20Z"
                        />
                      </svg>
                    </button>
                    <button
                      class="expenses__action expenses__action--danger"
                      type="button"
                      (click)="openDelete(expense)"
                      aria-label="Eliminar"
                    >
                      <svg viewBox="0 0 24 24" width="16" height="16" aria-hidden="true">
                        <path
                          fill="currentColor"
                          d="M9 3v1H4v2h16V4h-5V3H9Zm-3 5v11a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V8h-2v11H8V8H6Zm3 2v7h2v-7H9Zm4 0v7h2v-7h-2Z"
                        />
                      </svg>
                    </button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </section>

        <div class="expenses__cards" aria-label="Gastos">
          @for (expense of items(); track expense.id) {
            <article class="expense-card">
              <div class="expense-card__head">
                <span class="expense-card__title">{{ expense.title }}</span>
                <span class="expense-card__amount">{{ expense.amount | currency }}</span>
              </div>
              <div class="expense-card__meta">
                <span>{{ expense.categoryName }}</span>
                <span>{{ expense.paymentMethodName ?? 'Sin método' }}</span>
                <span>{{ expense.expenseDate | date: 'dd MMM yyyy' }}</span>
              </div>
              <div class="expense-card__actions">
                <app-button variant="secondary" size="sm" (click)="openEdit(expense)">
                  Editar
                </app-button>
                <app-button variant="danger" size="sm" (click)="openDelete(expense)">
                  Eliminar
                </app-button>
              </div>
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

      <app-expense-form-modal
        [open]="formOpen()"
        [expense]="editingExpense()"
        [categories]="categoryOptions()"
        [paymentMethods]="paymentMethodOptions()"
        (saved)="onSaved()"
        (cancelled)="closeForm()"
      />

      <app-confirm-dialog
        [open]="deleteOpen()"
        title="Eliminar gasto"
        message="¿Seguro que quieres eliminar este gasto? Esta acción no se puede deshacer."
        confirmLabel="Eliminar"
        variant="danger"
        [loading]="deleting()"
        (confirmed)="onDeleteConfirmed()"
        (cancelled)="closeDelete()"
      />
    </div>
  `,
  styles: [
    `
      .expenses {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-5);
      }
      .expenses__header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: var(--le-space-4);
      }
      .expenses__title {
        font-size: var(--le-fs-2xl);
      }
      .expenses__subtitle {
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
        margin-top: var(--le-space-1);
      }

      .expenses__filters {
        display: grid;
        grid-template-columns: 1fr;
        gap: var(--le-space-3);
        padding: var(--le-space-4);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .expenses__loading {
        display: flex;
        justify-content: center;
        padding: var(--le-space-10);
        color: var(--le-primary);
      }

      .expenses__table-wrap {
        display: none;
        overflow-x: auto;
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .expenses__table {
        width: 100%;
        border-collapse: collapse;
        font-size: var(--le-fs-sm);
      }
      .expenses__table th {
        text-align: left;
        padding: var(--le-space-3) var(--le-space-4);
        font-size: var(--le-fs-xs);
        text-transform: uppercase;
        letter-spacing: 0.04em;
        color: var(--le-text-subtle);
        border-bottom: 1px solid var(--le-border);
        white-space: nowrap;
      }
      .expenses__table td {
        padding: var(--le-space-3) var(--le-space-4);
        border-bottom: 1px solid var(--le-border);
        vertical-align: middle;
      }
      .expenses__table tbody tr:last-child td {
        border-bottom: none;
      }
      .expenses__table tbody tr:hover {
        background: var(--le-surface-hover);
      }
      .expenses__th-amount {
        text-align: right;
      }
      .expenses__title-cell {
        display: block;
        font-weight: var(--le-fw-medium);
        color: var(--le-text);
      }
      .expenses__description {
        display: block;
        font-size: var(--le-fs-xs);
        color: var(--le-text-subtle);
        margin-top: 2px;
      }
      .expenses__amount {
        text-align: right;
        font-weight: var(--le-fw-semibold);
        color: var(--le-text);
        white-space: nowrap;
      }
      .expenses__actions {
        display: flex;
        justify-content: flex-end;
        gap: var(--le-space-1);
        white-space: nowrap;
      }
      .expenses__action {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 2rem;
        height: 2rem;
        border: none;
        border-radius: var(--le-radius);
        background: transparent;
        color: var(--le-text-muted);
        cursor: pointer;
        transition:
          background-color var(--le-transition),
          color var(--le-transition);
      }
      .expenses__action:hover {
        background: var(--le-surface-hover);
        color: var(--le-text);
      }
      .expenses__action--danger:hover {
        background: var(--le-danger-soft);
        color: var(--le-danger);
      }

      .expenses__cards {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-3);
      }
      .expense-card {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-2);
        padding: var(--le-space-4);
        background: var(--le-surface);
        border: 1px solid var(--le-border);
        border-radius: var(--le-radius-md);
      }
      .expense-card__head {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: var(--le-space-3);
      }
      .expense-card__title {
        font-weight: var(--le-fw-semibold);
        color: var(--le-text);
      }
      .expense-card__amount {
        font-weight: var(--le-fw-bold);
        color: var(--le-text);
        white-space: nowrap;
      }
      .expense-card__meta {
        display: flex;
        flex-wrap: wrap;
        gap: var(--le-space-2);
        font-size: var(--le-fs-xs);
        color: var(--le-text-muted);
      }
      .expense-card__actions {
        display: flex;
        gap: var(--le-space-2);
        margin-top: var(--le-space-1);
      }

      @media (min-width: 769px) {
        .expenses__filters {
          grid-template-columns: repeat(6, 1fr);
          align-items: end;
        }
        .expenses__table-wrap {
          display: block;
        }
        .expenses__cards {
          display: none;
        }
      }
    `,
  ],
})
export class ExpensesList {
  private readonly expenseService = inject(ExpenseService);
  private readonly referenceService = inject(ReferenceService);

  protected readonly title = signal('');
  protected readonly fromDate = signal('');
  protected readonly toDate = signal('');
  protected readonly categoryFilter = signal<number | null>(null);
  protected readonly paymentMethodFilter = signal<number | null>(null);
  protected readonly page = signal(1);
  protected readonly size = signal(10);

  protected readonly categories = this.referenceService.categories;
  protected readonly paymentMethods = this.referenceService.paymentMethods;

  protected readonly categoryOptions = computed<SelectOption[]>(() =>
    this.categories().map((c) => ({ value: c.id, label: c.name })),
  );
  protected readonly paymentMethodOptions = computed<SelectOption[]>(() =>
    this.paymentMethods().map((p) => ({ value: p.id, label: p.name })),
  );

  protected readonly expensesResource = resource({
    params: () => ({
      page: this.page(),
      size: this.size(),
      title: this.title() || undefined,
      fromDate: this.fromDate() || undefined,
      toDate: this.toDate() || undefined,
      categoryId: this.categoryFilter() ?? undefined,
      paymentMethodId: this.paymentMethodFilter() ?? undefined,
    }),
    loader: ({ params }) => firstValueFrom(this.expenseService.getExpenses(params)),
  });

  protected readonly items = computed(() => this.expensesResource.value()?.items ?? []);
  protected readonly totalItems = computed(
    () => this.expensesResource.value()?.totalItems ?? 0,
  );
  protected readonly hasError = computed(() => !!this.expensesResource.error());
  protected readonly isLoading = computed(
    () =>
      this.expensesResource.isLoading() ||
      this.expensesResource.status() === 'idle',
  );

  protected readonly formOpen = signal(false);
  protected readonly editingExpense = signal<ExpenseDto | null>(null);

  protected readonly deleteOpen = signal(false);
  protected readonly deleting = signal(false);
  private pendingDelete: ExpenseWithNamesDto | null = null;

  constructor() {
    this.referenceService.loadCategories().subscribe();
    this.referenceService.loadPaymentMethods().subscribe();
  }

  protected onTitleChange(value: string): void {
    this.title.set(value);
    this.page.set(1);
  }

  protected onFromDateChange(value: string): void {
    this.fromDate.set(value);
    this.page.set(1);
  }

  protected onToDateChange(value: string): void {
    this.toDate.set(value);
    this.page.set(1);
  }

  protected onCategoryChange(value: number | null): void {
    this.categoryFilter.set(value);
    this.page.set(1);
  }

  protected onPaymentMethodChange(value: number | null): void {
    this.paymentMethodFilter.set(value);
    this.page.set(1);
  }

  protected onPageChange(page: number): void {
    this.page.set(page);
  }

  protected clearFilters(): void {
    this.title.set('');
    this.fromDate.set('');
    this.toDate.set('');
    this.categoryFilter.set(null);
    this.paymentMethodFilter.set(null);
    this.page.set(1);
  }

  protected openCreate(): void {
    this.editingExpense.set(null);
    this.formOpen.set(true);
  }

  protected openEdit(expense: ExpenseDto): void {
    this.editingExpense.set(expense);
    this.formOpen.set(true);
  }

  protected closeForm(): void {
    this.formOpen.set(false);
  }

  protected onSaved(): void {
    this.formOpen.set(false);
    this.expensesResource.reload();
  }

  protected openDelete(expense: ExpenseWithNamesDto): void {
    this.pendingDelete = expense;
    this.deleteOpen.set(true);
  }

  protected closeDelete(): void {
    this.deleteOpen.set(false);
    this.pendingDelete = null;
  }

  protected onDeleteConfirmed(): void {
    if (!this.pendingDelete) {
      return;
    }
    this.deleting.set(true);
    this.expenseService.delete({ id: this.pendingDelete.id }).subscribe({
      next: () => {
        this.deleting.set(false);
        this.closeDelete();
        this.expensesResource.reload();
      },
      error: (error: HttpErrorResponse) => {
        this.deleting.set(false);
        this.closeDelete();
      },
    });
  }
}
