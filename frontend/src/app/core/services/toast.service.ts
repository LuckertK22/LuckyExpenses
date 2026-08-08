import { Injectable, signal } from '@angular/core';

export type ToastKind = 'success' | 'error' | 'info' | 'warning';

export interface Toast {
  id: number;
  kind: ToastKind;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private readonly _toasts = signal<Toast[]>([]);
  readonly toasts = this._toasts.asReadonly();

  private counter = 0;

  success(message: string, duration = 4000): void {
    this.show('success', message, duration);
  }

  error(message: string, duration = 6000): void {
    this.show('error', message, duration);
  }

  info(message: string, duration = 4000): void {
    this.show('info', message, duration);
  }

  warning(message: string, duration = 5000): void {
    this.show('warning', message, duration);
  }

  dismiss(id: number): void {
    this._toasts.update((list) => list.filter((t) => t.id !== id));
  }

  private show(kind: ToastKind, message: string, duration: number): void {
    const id = ++this.counter;
    this._toasts.update((list) => [...list, { id, kind, message }]);
    if (duration > 0) {
      setTimeout(() => this.dismiss(id), duration);
    }
  }
}