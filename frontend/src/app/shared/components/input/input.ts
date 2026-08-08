import { Component, computed, forwardRef, input, model, signal } from '@angular/core';
import { NgClass } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-input',
  imports: [NgClass],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => Input),
      multi: true,
    },
  ],
  template: `
    <div class="le-field">
      @if (label()) {
        <label class="le-field__label" [for]="inputId()">{{ label() }}</label>
      }
      <input
        class="le-input"
        [ngClass]="{ 'le-input--invalid': hasError() }"
        [id]="inputId()"
        [type]="type()"
        [value]="value()"
        [placeholder]="placeholder()"
        [disabled]="isDisabled()"
        [required]="required()"
        [autocomplete]="autocomplete()"
        [name]="name()"
        (input)="onInput($event)"
        (blur)="onBlur()"
      />
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
      .le-input {
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
      }
      .le-input::placeholder {
        color: var(--le-text-subtle);
      }
      .le-input:hover:not(:disabled) {
        border-color: var(--le-text-subtle);
      }
      .le-input:focus {
        outline: none;
        border-color: var(--le-primary);
        box-shadow: 0 0 0 3px color-mix(in srgb, var(--le-primary) 20%, transparent);
      }
      .le-input:disabled {
        opacity: 0.6;
        cursor: not-allowed;
      }
      .le-input--invalid,
      .le-input--invalid:focus {
        border-color: var(--le-danger);
      }
      .le-field__error {
        font-size: var(--le-fs-xs);
        color: var(--le-danger);
      }
    `,
  ],
})
export class Input implements ControlValueAccessor {
  value = model<string>('');
  label = input<string>('');
  type = input<string>('text');
  placeholder = input<string>('');
  autocomplete = input<string>('');
  name = input<string>('');
  disabled = input(false);
  required = input(false);
  error = input<string>('');
  touched = model<boolean>(false);

  private static nextId = 0;

  private readonly cvaDisabled = signal(false);
  protected readonly isDisabled = computed(() => this.disabled() || this.cvaDisabled());

  protected readonly inputId = computed(() => `le-input-${++Input.nextId}`);
  protected readonly hasError = computed(() => !!this.error());

  private onChange: (value: string) => void = () => undefined;
  private onTouch: () => void = () => undefined;

  writeValue(value: string): void {
    this.value.set(value ?? '');
  }

  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouch = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.cvaDisabled.set(isDisabled);
  }

  protected onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.value.set(value);
    this.onChange(value);
  }

  protected onBlur(): void {
    this.touched.set(true);
    this.onTouch();
  }
}
