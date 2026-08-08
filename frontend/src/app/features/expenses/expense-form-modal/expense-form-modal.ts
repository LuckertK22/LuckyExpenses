import { Component, computed, inject, input, output, signal } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';

import { ExpenseService } from '../../../core/services/expense.service';
import { ToastService } from '../../../core/services/toast.service';
import {
  CreateExpenseCommand,
  ExpenseDto,
  ProblemDetails,
  UpdateExpenseCommand,
} from '../../../core/models';
import { Modal } from '../../../shared/components/modal/modal';
import { Input } from '../../../shared/components/input/input';
import { Select, SelectOption } from '../../../shared/components/select/select';
import { Button } from '../../../shared/components/button/button';

@Component({
  selector: 'app-expense-form-modal',
  imports: [ReactiveFormsModule, Modal, Input, Select, Button],
  template: `
    <app-modal
      [open]="open()"
      [title]="isEditing() ? 'Editar gasto' : 'Nuevo gasto'"
      (closed)="cancel()"
    >
      <form class="expense-form" [formGroup]="form" (ngSubmit)="onSubmit()" novalidate>
        <app-input
          formControlName="title"
          label="Título"
          type="text"
          placeholder="Ej. Supermercado"
          [error]="fieldError('title')"
        />
        <app-input
          formControlName="description"
          label="Descripción"
          type="text"
          placeholder="Opcional"
          [error]="fieldError('description')"
        />
        <app-select
          formControlName="categoryId"
          label="Categoría"
          [options]="categoryOptions()"
          [error]="fieldError('categoryId')"
        />
        <app-select
          formControlName="paymentMethodId"
          label="Método de pago"
          placeholder="Sin método"
          [options]="paymentMethodOptions()"
          [error]="fieldError('paymentMethodId')"
        />
        <div class="expense-form__row">
          <app-input
            formControlName="amount"
            label="Monto"
            type="number"
            min="0"
            step="0.01"
            placeholder="0.00"
            [error]="fieldError('amount')"
          />
          <app-input
            formControlName="expenseDate"
            label="Fecha"
            type="date"
            [error]="fieldError('expenseDate')"
          />
        </div>
        <div class="expense-form__actions">
          <app-button type="button" variant="ghost" (click)="cancel()">
            Cancelar
          </app-button>
          <app-button
            type="submit"
            [loading]="saving()"
            [disabled]="form.invalid"
          >
            {{ isEditing() ? 'Guardar cambios' : 'Crear gasto' }}
          </app-button>
        </div>
      </form>
    </app-modal>
  `,
  styles: [
    `
      .expense-form {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-4);
      }
      .expense-form__row {
        display: grid;
        grid-template-columns: 1fr;
        gap: var(--le-space-4);
      }
      .expense-form__actions {
        display: flex;
        justify-content: flex-end;
        gap: var(--le-space-3);
        margin-top: var(--le-space-2);
      }
      @media (min-width: 769px) {
        .expense-form__row {
          grid-template-columns: 1fr 1fr;
        }
      }
    `,
  ],
})
export class ExpenseFormModal {
  private readonly fb = inject(FormBuilder);
  private readonly expenseService = inject(ExpenseService);
  private readonly toast = inject(ToastService);

  open = input(false);
  expense = input<ExpenseDto | null>(null);
  categories = input<SelectOption[]>([]);
  paymentMethods = input<SelectOption[]>([]);

  readonly saved = output<ExpenseDto>();
  readonly cancelled = output<void>();

  protected readonly saving = signal(false);
  protected readonly isEditing = computed(() => !!this.expense());

  protected readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.maxLength(1000)]],
    categoryId: [null as number | null, [Validators.required]],
    paymentMethodId: [null as number | null],
    amount: [null as number | null, [Validators.required, Validators.min(0.01)]],
    expenseDate: ['', [Validators.required]],
  });

  protected readonly categoryOptions = computed(() => this.categories());
  protected readonly paymentMethodOptions = computed(() => this.paymentMethods());

  protected fieldError(
    controlName: keyof typeof this.form.controls,
  ): string {
    const control = this.form.controls[controlName];
    if (!control.touched || !control.errors) {
      return '';
    }
    if (control.hasError('server')) {
      return control.getError('server');
    }
    if (control.hasError('required')) {
      return 'Este campo es requerido.';
    }
    if (control.hasError('min')) {
      return 'Debe ser mayor a 0.';
    }
    return '';
  }

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const editing = this.expense();

    if (editing) {
      const command: UpdateExpenseCommand = {
        id: editing.id,
        title: raw.title,
        description: raw.description || null,
        categoryId: raw.categoryId!,
        paymentMethodId: raw.paymentMethodId,
        amount: raw.amount!,
        expenseDate: raw.expenseDate,
      };
      this.saveRequest(this.expenseService.update(command), 'Gasto actualizado.');
    } else {
      const command: CreateExpenseCommand = {
        title: raw.title,
        description: raw.description || null,
        categoryId: raw.categoryId!,
        paymentMethodId: raw.paymentMethodId,
        amount: raw.amount!,
        expenseDate: raw.expenseDate,
      };
      this.saveRequest(this.expenseService.create(command), 'Gasto creado.');
    }
  }

  private saveRequest(request: ReturnType<ExpenseService['create']>, message: string): void {
    this.saving.set(true);
    request
      .pipe(finalize(() => this.saving.set(false)))
      .subscribe({
        next: (expense) => {
          this.toast.success(message);
          this.saved.emit(expense);
        },
        error: (error: HttpErrorResponse) => this.applyServerErrors(error),
      });
  }

  cancel(): void {
    this.cancelled.emit();
  }

  private applyServerErrors(error: HttpErrorResponse): void {
    const body = error.error as ProblemDetails | undefined;
    if (!body?.errors) {
      return;
    }
    for (const [field, messages] of Object.entries(body.errors)) {
      const control = this.form.get(field);
      if (control && messages.length > 0) {
        control.setErrors({ server: messages[0] });
        control.markAsTouched();
      }
    }
  }
}
