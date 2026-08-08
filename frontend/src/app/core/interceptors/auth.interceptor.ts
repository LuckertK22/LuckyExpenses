import {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  filter,
  finalize,
  switchMap,
  take,
  throwError,
} from 'rxjs';

import { AuthService } from '../services/auth.service';
import { TokenService } from '../services/token.service';
import { LoginResponse } from '../models';

let isRefreshing = false;
let refreshSubject = new BehaviorSubject<LoginResponse | null>(null);

export function authInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
) {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);
  const accessToken = tokenService.accessToken();
  const isAuthCall = isAuthenticationEndpoint(req.url);

  if (accessToken && !isAuthCall) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${accessToken}` },
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isAuthCall) {
        return handleUnauthorized(req, next, authService);
      }
      return throwError(() => error);
    }),
  );
}

function handleUnauthorized(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService,
) {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshSubject.next(null);

    return authService.refresh().pipe(
      switchMap((res: LoginResponse) => {
        refreshSubject.next(res);
        return next(cloneWithToken(req, res.token));
      }),
      catchError((err) => {
        refreshSubject.complete();
        refreshSubject = new BehaviorSubject<LoginResponse | null>(null);
        authService.logout();
        return throwError(() => err);
      }),
      finalize(() => {
        isRefreshing = false;
      }),
    );
  }

  return refreshSubject.pipe(
    filter((res): res is LoginResponse => res !== null),
    take(1),
    switchMap((res) => next(cloneWithToken(req, res.token))),
  );
}

function cloneWithToken(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
}

function isAuthenticationEndpoint(url: string): boolean {
  return (
    url.includes('/Authentication/Login') ||
    url.includes('/Authentication/Register') ||
    url.includes('/Authentication/Refresh') ||
    url.includes('/Authentication/Logout')
  );
}