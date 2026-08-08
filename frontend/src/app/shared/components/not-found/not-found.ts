import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  template: `
    <div class="not-found">
      <h1>404</h1>
      <p>Página no encontrada</p>
      <a routerLink="/dashboard">Ir al dashboard</a>
    </div>
  `,
  imports: [RouterLink],
  styles: [
    `
      .not-found {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        min-height: 100vh;
        gap: var(--le-space-3);
        text-align: center;
      }
      .not-found h1 {
        font-size: var(--le-fs-4xl);
      }
    `,
  ],
})
export class NotFound {}