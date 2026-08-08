import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, catchError, map, of, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { TokenService } from './token.service';
import {
  ApiResponse,
  AuthUser,
  LoginCommand,
  LoginResponse,
  LogoutCommand,
  RefreshCommand,
  RegisterCommand,
} from '../models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenService = inject(TokenService);
  private readonly router = inject(Router);

  private readonly _user = signal<AuthUser | null>(null);
  readonly user = this._user.asReadonly();

  readonly isAuthenticated = signal<boolean>(false);

  private readonly base = `${environment.apiUrl}/Authentication`;

  login(command: LoginCommand): Observable<LoginResponse> {
    return this.http
      .post<ApiResponse<LoginResponse>>(`${this.base}/Login`, command)
      .pipe(
        map((res) => res.data!),
        tap((data) => {
          this.persistSession(data);
        }),
      );
  }

  register(command: RegisterCommand): Observable<void> {
    return this.http
      .post<ApiResponse<void>>(`${this.base}/Register`, command)
      .pipe(map(() => undefined as void));
  }

  refresh(): Observable<LoginResponse> {
    const refreshToken = this.tokenService.refreshToken;
    if (!refreshToken) {
      throw new Error('No refresh token available');
    }
    const command: RefreshCommand = { refreshToken };
    return this.http
      .post<ApiResponse<LoginResponse>>(`${this.base}/Refresh`, command)
      .pipe(
        map((res) => res.data!),
        tap((data) => {
          this.persistSession(data);
        }),
      );
  }

  logout(): void {
    const refreshToken = this.tokenService.refreshToken;
    if (refreshToken) {
      const command: LogoutCommand = { refreshToken };
      this.http
        .post<ApiResponse<void>>(`${this.base}/Logout`, command)
        .subscribe({ error: () => undefined });
    }
    this.clearSession();
    this.router.navigate(['/authentication/login']);
  }

  tryRestoreSession(): Observable<LoginResponse> {
    return this.refresh();
  }

  restoreSessionIfPresent(): Observable<void> {
    if (!this.tokenService.hasRefreshToken()) {
      return of(undefined);
    }
    return this.refresh().pipe(
      map(() => undefined),
      catchError(() => {
        this.clearSession();
        return of(undefined);
      }),
    );
  }

  private persistSession(res: LoginResponse): void {
    this.tokenService.setTokens(res.token, res.refreshToken);
    this._user.set({ email: res.email, role: res.role, expiresAt: res.expiresAt });
    this.isAuthenticated.set(true);
  }

  private clearSession(): void {
    this.tokenService.clearAll();
    this._user.set(null);
    this.isAuthenticated.set(false);
  }
}