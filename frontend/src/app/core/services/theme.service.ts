import { Injectable, signal } from '@angular/core';

export type Theme = 'light' | 'dark';

const THEME_KEY = 'le.theme';
const DATA_THEME_ATTR = 'data-theme';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly _theme = signal<Theme>(this.resolveInitialTheme());
  readonly theme = this._theme.asReadonly();

  constructor() {
    applyTheme(this._theme());
  }

  toggle(): void {
    this.set(this._theme() === 'light' ? 'dark' : 'light');
  }

  set(theme: Theme): void {
    this._theme.set(theme);
    localStorage.setItem(THEME_KEY, theme);
    applyTheme(theme);
  }

  private resolveInitialTheme(): Theme {
    const stored = localStorage.getItem(THEME_KEY) as Theme | null;
    if (stored === 'light' || stored === 'dark') {
      return stored;
    }
    const prefersDark = window.matchMedia?.('(prefers-color-scheme: dark)').matches ?? false;
    return prefersDark ? 'dark' : 'light';
  }
}

function applyTheme(theme: Theme): void {
  document.documentElement.setAttribute(DATA_THEME_ATTR, theme);
}