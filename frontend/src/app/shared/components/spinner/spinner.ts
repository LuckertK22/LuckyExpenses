import { Component, input } from '@angular/core';

export type SpinnerSize = 'sm' | 'md' | 'lg';

@Component({
  selector: 'app-spinner',
  template: `
    <span class="le-spinner" [class]="'le-spinner--' + size()" aria-hidden="true"></span>
  `,
  styles: [
    `
      .le-spinner {
        display: inline-block;
        border-radius: 50%;
        border: 2px solid color-mix(in srgb, currentColor 25%, transparent);
        border-top-color: currentColor;
        animation: le-spin 0.7s linear infinite;
      }
      .le-spinner--sm {
        width: 1rem;
        height: 1rem;
      }
      .le-spinner--md {
        width: 1.5rem;
        height: 1.5rem;
      }
      .le-spinner--lg {
        width: 2.5rem;
        height: 2.5rem;
      }
      @keyframes le-spin {
        to {
          transform: rotate(360deg);
        }
      }
    `,
  ],
})
export class Spinner {
  size = input<SpinnerSize>('md');
}
