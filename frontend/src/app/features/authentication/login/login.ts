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
import { ProblemDetails } from '../../../core/models';
import { Input } from '../../../shared/components/input/input';
import { Button } from '../../../shared/components/button/button';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink, Input, Button],
  template: `
    <form class="auth-form" [formGroup]="form" (ngSubmit)="onSubmit()" novalidate>
      <app-input
        formControlName="email"
        label="Email"
        type="email"
        placeholder="tu@email.com"
        autocomplete="email"
        [error]="emailError()"
      />
      <app-input
        formControlName="password"
        label="Contraseña"
        type="password"
        placeholder="••••••••"
        autocomplete="current-password"
        [error]="passwordError()"
      />
      <app-button
        type="submit"
        [block]="true"
        [loading]="loading()"
        [disabled]="form.invalid"
      >
        Iniciar sesión
      </app-button>
    </form>
    <p class="auth-form__switch">
      ¿No tienes cuenta?
      <a routerLink="/authentication/register">Regístrate</a>
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
export class Login {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  protected readonly loading = signal(false);

  protected readonly emailError = () => this.fieldError('email', {
    required: 'El email es requerido.',
    email: 'Ingresa un email válido.',
  });

  protected readonly passwordError = () => this.fieldError('password', {
    required: 'La contraseña es requerida.',
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { email, password } = this.form.getRawValue();
    this.loading.set(true);

    this.authService
      .login({ email, password })
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: () => this.router.navigate(['/dashboard']),
        error: (error: HttpErrorResponse) => this.applyServerErrors(error),
      });
  }

  private fieldError(
    controlName: 'email' | 'password',
    messages: Record<string, string>,
  ): string {
    const control = this.form.controls[controlName];
    if (!control.touched || !control.errors) {
      return '';
    }
    if (control.hasError('server')) {
      return control.getError('server');
    }
    for (const [key, message] of Object.entries(messages)) {
      if (control.hasError(key)) {
        return message;
      }
    }
    return '';
  }

  private applyServerErrors(error: HttpErrorResponse): void {
    const body = error.error as ProblemDetails | undefined;
    if (!body?.errors) {
      return;
    }
    for (const [field, messages] of Object.entries(body.errors)) {
      const controlName = field.toLowerCase() as 'email' | 'password';
      const control = this.form.controls[controlName];
      if (control && messages.length > 0) {
        control.setErrors({ server: messages[0] });
        control.markAsTouched();
      }
    }
  }
}
