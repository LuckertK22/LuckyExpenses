import { Injectable, signal } from '@angular/core';

const REFRESH_TOKEN_KEY = 'le.refresh';

@Injectable({ providedIn: 'root' })
export class TokenService {
  private readonly _accessToken = signal<string | null>(null);
  readonly accessToken = this._accessToken.asReadonly();

  get refreshToken(): string | null {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  setTokens(accessToken: string, refreshToken: string): void {
    this._accessToken.set(accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
  }

  clearAccessToken(): void {
    this._accessToken.set(null);
  }

  clearAll(): void {
    this._accessToken.set(null);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
  }

  hasRefreshToken(): boolean {
    return this.refreshToken !== null;
  }
}