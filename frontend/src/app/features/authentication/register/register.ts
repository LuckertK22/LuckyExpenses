import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs';

import { AuthService } from '../../../core/services/auth.service';
import { ToastService } from '../../../core/services/toast.service';
import { ProblemDetails } from '../../../core/models';
import { Input } from '../../../shared/components/input/input';
import { Button } from '../../../shared/components/button/button';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink, Input, Button],
  template: `
    <form class="auth-form" [formGroup]="form" (ngSubmit)="onSubmit()" novalidate>
      <app-input
        formControlName="firstName"
        label="Nombre"
        type="text"
        placeholder="Ana"
        autocomplete="given-name"
        [error]="fieldError('firstName')"
      />
      <app-input
        formControlName="lastName"
        label="Apellido"
        type="text"
        placeholder="García"
        autocomplete="family-name"
        [error]="fieldError('lastName')"
      />
      <app-input
        formControlName="email"
        label="Email"
        type="email"
        placeholder="tu@email.com"
        autocomplete="email"
        [error]="fieldError('email')"
      />
      <app-input
        formControlName="password"
        label="Contraseña"
        type="password"
        placeholder="Mínimo 6 caracteres"
        autocomplete="new-password"
        [error]="fieldError('password')"
      />
      <app-button
        type="submit"
        [block]="true"
        [loading]="loading()"
        [disabled]="form.invalid"
      >
        Crear cuenta
      </app-button>
    </form>
    <p class="auth-form__switch">
      ¿Ya tienes cuenta?
      <a routerLink="/authentication/login">Inicia sesión</a>
    </p>
  `,
  styles: [
    `
      :host {
        display: block;
      }
      .auth-form {
        display: flex;
        flex-direction: column;
        gap: var(--le-space-4);
      }
      .auth-form__switch {
        margin-top: var(--le-space-5);
        text-align: center;
        font-size: var(--le-fs-sm);
        color: var(--le-text-muted);
      }
    `,
  ],
})
export class Register {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly toast = inject(ToastService);
  private readonly router = inject(Router);

  protected readonly form = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
    password: [
      '',
      [Validators.required, Validators.minLength(6), Validators.maxLength(100)],
    ],
  });

  protected readonly loading = signal(false);

  protected fieldError(controlName: keyof typeof this.form.controls): string {
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
    if (control.hasError('email')) {
      return 'Ingresa un email válido.';
    }
    if (control.hasError('minlength')) {
      return `Mínimo ${control.getError('minlength').requiredLength} caracteres.`;
    }
    if (control.hasError('maxlength')) {
      return `Máximo ${control.getError('maxlength').requiredLength} caracteres.`;
    }
    return '';
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const command = this.form.getRawValue();
    this.loading.set(true);

    this.authService
      .register(command)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => {
          this.toast.success('Cuenta creada. Inicia sesión.');
          this.router.navigate(['/authentication/login']);
        },
        error: (error: HttpErrorResponse) => this.applyServerErrors(error),
      });
  }

  private applyServerErrors(error: HttpErrorResponse): void {
    const body = error.error as ProblemDetails | undefined;
    if (!body?.errors) {
      return;
    }
    for (const [field, messages] of Object.entries(body.errors)) {
      const controlName = field.toLowerCase() as
        | 'firstName'
        | 'lastName'
        | 'email'
        | 'password';
      const control = this.form.controls[controlName];
      if (control && messages.length > 0) {
        control.setErrors({ server: messages[0] });
        control.markAsTouched();
      }
    }
  }
}
