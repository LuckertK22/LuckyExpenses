import {
  HttpErrorResponse,
  HttpHandlerFn,
  HttpInterceptorFn,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';

import { ToastService } from '../services/toast.service';
import { ProblemDetails } from '../models';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const message = resolveMessage(error);
      if (message) {
        toast.error(message);
      }
      return throwError(() => error);
    }),
  );
};

function resolveMessage(error: HttpErrorResponse): string | null {
  if (error.status === 0) {
    return 'No se pudo conectar con el servidor. Verifica tu conexión.';
  }

  const body = error.error as ProblemDetails | undefined;
  if (!body) {
    return `Error inesperado (${error.status}).`;
  }

  if (body.errors && Object.keys(body.errors).length > 0) {
    const first = Object.values(body.errors)[0];
    if (Array.isArray(first) && first.length > 0) {
      return first[0];
    }
  }

  return body.detail ?? body.title ?? `Error ${error.status}`;
}