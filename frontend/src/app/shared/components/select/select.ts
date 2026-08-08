import { Component, computed, forwardRef, input, model, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export interface SelectOption {
  value: number;
  label: string;
}

@Component({
  selector: 'app-select',
  imports: [NgClass],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => Select),
      multi: true,
    },
  ],
  template: `
    <div class="le-field">
      @if (label()) {
        <label class="le-field__label" [for]="selectId()">{{ label() }}</label>
      }
      <select
        class="le-select"
        [ngClass]="{ 'le-select--invalid': hasError() }"
        [id]="selectId()"
        [value]="selected()"
        [disabled]="isDisabled()"
        (change)="onSelect($event)"
      >
        <option [value]="''">{{ placeholder() }}</option>
        @for (option of options(); track option.value) {
          <option [value]="option.value">{{ option.label }}</option>
        }
      </select>
      @if (error()) {
        <span class="le-field__error" role="alert">{{ error() }}</span>
      }
    </div>
  `,
  styles: [
    `
      .le-field {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-1);
      }
      .le-field__label {
        font-size: var(--le-fs-sm);
        font-weight: var(--le-fw-medium);
        color: var(--le-text);
      }
      .le-select {
        width: 100%;
        font-family: inherit;
        font-size: var(--le-fs-base);
        color: var(--le-text);
        background: var(--le-surface);
        border: 1px solid var(--le-border-strong);
        border-radius: var(--le-radius);
        padding: 0.5625rem 0.875rem;
        transition:
          border-color var(--le-transition),
          box-shadow var(--le-transition);
        appearance: none;
        background-image: url("data:image/svg+xml;charset=utf-8,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' viewBox='0 0 24 24' fill='none' stroke='%238a9990' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpath d='m6 9 6 6 6-6'/%3E%3C/svg%3E");
        background-repeat: no-repeat;
        background-position: right 0.75rem center;
        padding-right: 2.25rem;
        cursor: pointer;
      }
      .le-select:hover:not(:disabled) {
        border-color: var(--le-text-subtle);
      }
      .le-select:focus {
        outline: none;
        border-color: var(--le-primary);
        box-shadow: 0 0 0 3px color-mix(in srgb, var(--le-primary) 20%, transparent);
      }
      .le-select:disabled {
        opacity: 0.6;
        cursor: not-allowed;
      }
      .le-select--invalid,
      .le-select--invalid:focus {
        border-color: var(--le-danger);
      }
      .le-field__error {
        font-size: var(--le-fs-xs);
        color: var(--le-danger);
      }
    `,
  ],
})
export class Select implements ControlValueAccessor {
  value = model<number | null>(null);
  options = input<SelectOption[]>([]);
  label = input<string>('');
  placeholder = input<string>('');
  disabled = input(false);
  required = input(false);
  error = input<string>('');

  private static nextId = 0;

  private readonly cvaDisabled = signal(false);
  protected readonly isDisabled = computed(() => this.disabled() || this.cvaDisabled());

  protected readonly selectId = computed(() => `le-select-${++Select.nextId}`);
  protected readonly hasError = computed(() => !!this.error());
  protected readonly selected = computed(() => this.value() ?? '');

  private onChange: (value: number | null) => void = () => undefined;
  private onTouch: () => void = () => undefined;

  writeValue(value: number | null): void {
    this.value.set(value ?? null);
  }

  registerOnChange(fn: (value: number | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouch = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.cvaDisabled.set(isDisabled);
  }

  protected onSelect(event: Event): void {
    const raw = (event.target as HTMLSelectElement).value;
    const next = raw === '' ? null : Number(raw);
    this.value.set(next);
    this.onChange(next);
    this.onTouch();
  }
}
